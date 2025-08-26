using UnityEngine;
using TMPro;
using System.Text;
using System;

public class UnlockedMessagesUI : MonoBehaviour
{
    public TextMeshProUGUI messagesText; // Reference to one TMP text component

    void Awake()
    {
        var unlocked = MessageManager.Instance.GetUnlockedMessages();

        // Build a combined string
        StringBuilder sb = new StringBuilder();
        foreach (string msg in unlocked)
        {
            Debug.Log(msg);
            sb.AppendLine(msg); // Adds a newline after each message
        }

        // Set all unlocked messages into the one text box
        messagesText.text = sb.ToString();
    }

    void Update()
    {
    }
}
