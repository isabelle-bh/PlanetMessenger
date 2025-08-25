using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlaneFlight : MonoBehaviour
{
    public Transform target;          // The invisible point in front of the camera
    public Transform returnPoint;     // Where the plane goes back to
    public GameObject messageUI;      // Your message UI object
    public Action onReturn;           // Callback when the plane finishes returning

    public float speed = 300f;
    private bool goingToTarget = true;
    private bool messageDismissed = false;
    private string messageText;

    void Start()
    {
        if (messageUI != null)
        {
            Button btn = messageUI.GetComponentInChildren<Button>();
            if (btn != null)
            {
                btn.onClick.AddListener(DismissMessage);
            }
        }
    }

    void Update()
    {
        if (goingToTarget)
        {
            transform.LookAt(Camera.main.transform.position);

            MoveTowards(target.position, () =>
            {
                goingToTarget = false;

                // Position the message UI at the target point
                if (messageUI != null)
                {
                    messageUI.transform.position = target.position;
                    messageUI.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward); // face the player

                    TextMeshProUGUI text = messageUI.GetComponentInChildren<TextMeshProUGUI>();
                    if (text != null)
                    {
                        messageText = MessageManager.Instance.GetNextMessage();
                        text.text = messageText;
                        messageUI.SetActive(true);
                    }
                }
            });
        }
        else if (messageDismissed)
        {
            transform.LookAt(returnPoint.transform.position);
            MoveTowards(returnPoint.position, () =>
            {
                onReturn?.Invoke();
            });
        }
    }

    public void DismissMessage()
    {
        if (messageUI != null)
            messageUI.SetActive(false);

        messageDismissed = true;
    }

    private void MoveTowards(Vector3 destination, Action onArrived)
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, destination) < 0.0000001f)
        {
            onArrived?.Invoke();
        }
    }
}
