using UnityEngine;

// class to manage the main menu behaviour, such as disabling the orbit camera,
// adding functionality to buttons,

public class MainMenuManager : MonoBehaviour
{
    public float transitionDuration = 3f; // how long camera moves for
    public MonoBehaviour orbitCameraScript;
    public GameObject pauseManager;
    public PauseManager pauseManagerObj;
    public GameObject unlockedMessagesUI;

    public GameObject mainMenuUI;
    private bool transitioning = false; // if transitioning from menu to planet
    private float elapsed = 0f;
    private Vector3 startPos;
    private Quaternion startRot;
    private Camera mainCam;
    private Vector3 menuCamPos;
    private Quaternion menuCamRot;
    private Vector3 targetPos;
    private Quaternion targetRot;
    private bool goingToGame;
    public GameObject pauseUI;
    void Start()
    {
        mainCam = Camera.main;
        menuCamPos = mainCam.transform.position;
        menuCamRot = mainCam.transform.rotation;
        Debug.Log("menu cam position: " + menuCamPos + " menu cam rotation: " + menuCamRot);

        if (orbitCameraScript != null)
        {
            // keeping orbit camera disabled while player is in title screen
            orbitCameraScript.enabled = false;
        }
        pauseManager.SetActive(false);
        unlockedMessagesUI.SetActive(false);
    }

    // to start the game
    public void OnPlayButtonClicked()
    {
        goingToGame = true;
        transitioning = true; // if the camera is moving from title to planet
        elapsed = 0f; // time elapsed
        startPos = mainCam.transform.position; // starting position and rotations of the camera
        startRot = mainCam.transform.rotation;

        // get orbit camera's target transform
        if (orbitCameraScript != null && orbitCameraScript is OrbitCamera orbit)
        {
            // this method fills in values for camera's target positions
            orbit.GetInitialCameraTransform(out targetPos, out targetRot);
        }
    }

    public void OnBackToMenuButtonClicked()
    {
        Debug.Log("Going back to menu");
        gameObject.SetActive(true); // show menu when returning
        pauseManagerObj.UnPause();
        pauseManager.SetActive(false);
        transitioning = true; // if the camera is moving from title to planet
        goingToGame = false;
        elapsed = 0f; // time elapsed
        startPos = mainCam.transform.position; // starting position and rotations of the camera
        startRot = mainCam.transform.rotation;
        targetPos = menuCamPos;
        targetRot = menuCamRot;
        orbitCameraScript.enabled = false;
    }

    // to quit the game
    public void OnQuitButtonClicked()
    {
        Application.Quit();
    }

    public void OnUnlockedMessagesButtonClicked()
    {
        unlockedMessagesUI.SetActive(true);
    }

    public void OnCloseButtonClicked()
    {
        unlockedMessagesUI.SetActive(false);
    }

    // called once per frame
    void Update()
    {
        if (!transitioning) return;

        elapsed += Time.deltaTime;
        float t = Mathf.Clamp01(elapsed / transitionDuration);
        t = Mathf.SmoothStep(0f, 1f, t); // ease-in-out curve

        mainCam.transform.position = Vector3.Lerp(startPos, targetPos, t);
        mainCam.transform.rotation = Quaternion.Slerp(startRot, targetRot, t);

        if (t >= 1f)
        {
            transitioning = false;

            if (goingToGame)
            {
                pauseManager.SetActive(true);
                orbitCameraScript.enabled = true;
                gameObject.SetActive(false); // hide menu when entering game
            }
            else
            {
                Debug.Log("back in menu");
            }
        }
    }
}
