using UnityEngine;
using UnityEngine.EventSystems;

public class PlanetClick : MonoBehaviour
{
    public GameObject planePrefab;
    public Transform spawnPoint;
    public float distanceFromCamera = 3f;
    public AudioSource audioSource;
    public AudioClip planetClickSound;


    private bool inAir = false;

    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    void OnMouseDown()
    {
        if(Time.timeScale == 0f)
        {
            return;  // ignore click
        }
        Debug.Log("Planet clicked");
        if (inAir) return;
        Debug.Log("Plane not in air");

        audioSource.PlayOneShot(planetClickSound, 1);

        GameObject plane = Instantiate(planePrefab, spawnPoint.position, spawnPoint.rotation);
        PlaneFlight flight = plane.GetComponent<PlaneFlight>(); // Use the actual instance

        plane.SetActive(true);

        if (flight == null)
        {
            return;
        }

        // Create invisible target point in front of camera
        Vector3 targetPos = Camera.main.transform.position + Camera.main.transform.forward * distanceFromCamera;
        GameObject targetPoint = new GameObject("TargetPoint");
        targetPoint.transform.position = targetPos;

        // Assign all flight values
        flight.target = targetPoint.transform;
        flight.returnPoint = spawnPoint;
        flight.onReturn = () =>
        {
            inAir = false;
            Destroy(targetPoint);
            Destroy(plane);
        };

        inAir = true;
    }
}