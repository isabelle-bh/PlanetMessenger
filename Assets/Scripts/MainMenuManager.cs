using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public float transitionDuration = 3f; // how long camera moves for
    public MonoBehaviour orbitCameraScript;
    private bool transitioning = false; // if transitioning from menu to planet
    private float elapsed = 0f;
    private Vector3 startPos;
    private Quaternion startRot;
    private Camera mainCam;
    private Vector3 targetPos;
    private Quaternion targetRot;


    void Start()
    {
        mainCam = Camera.main;
        if (orbitCameraScript != null)
        {
            // keeping orbit camera disabled while player is in title screen
            orbitCameraScript.enabled = false;
        }
    }

    public void OnPlayButtonClicked()
    {
        transitioning = true; // if the camera is moving from title to planet
        elapsed = 0f; // time elapsed
        startPos = mainCam.transform.position; // starting position and rotations of the camera
        startRot = mainCam.transform.rotation;

        // Get orbit camera's target transform
        if (orbitCameraScript != null && orbitCameraScript is OrbitCamera orbit)
        {
            // this method fills in values for camera's target positions
            orbit.GetInitialCameraTransform(out targetPos, out targetRot);
        }
    }

    // called once per frame
    void Update()
    {
        // dont play any of this if transitioning is false
        if (!transitioning) return;

        elapsed += Time.deltaTime;
        float t = Mathf.Clamp01(elapsed / transitionDuration);
        t = Mathf.SmoothStep(0f, 1f, t); // ease-in-out curve

        mainCam.transform.position = Vector3.Lerp(startPos, targetPos, t);
        mainCam.transform.rotation = Quaternion.Slerp(startRot, targetRot, t);

        if (t >= 1f)
        {
            transitioning = false;

            if (orbitCameraScript != null)
            {
                orbitCameraScript.enabled = true; // enabling orbit behavior once transition is done
            }

            // hiding menu UI
            gameObject.SetActive(false);
        }
    }
}
