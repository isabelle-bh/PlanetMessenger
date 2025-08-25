using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
    [SerializeField] private Transform lookAtTransform;
    [SerializeField] private float sensitivity = 1.2f;
    [SerializeField] private float maxOrbitDistance = 2000f;
    [SerializeField] private float minOrbitDistance = 1300f;
    [SerializeField] private float inertiaDamping = 5f;

    public GameObject messageUI;

    private float orbitRadius = 1500f;
    private float yaw = 0f;
    private float pitch = 20f;

    private float yawVelocity = 0f;
    private float pitchVelocity = 0f;
    void Update()
    {
        if (messageUI.activeSelf) return;
       
        if (lookAtTransform == null) return;

        if (Input.GetMouseButton(0))
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            yawVelocity += mouseX * sensitivity;
            pitchVelocity += -mouseY * sensitivity;
        }

        yaw += yawVelocity * Time.deltaTime * 60f;
        pitch += pitchVelocity * Time.deltaTime * 60f;

        pitch = Mathf.Clamp(pitch, -89, 89);

        yawVelocity = Mathf.Lerp(yawVelocity, 0f, Time.deltaTime * inertiaDamping);
        pitchVelocity = Mathf.Lerp(pitchVelocity, 0f, Time.deltaTime * inertiaDamping);

        orbitRadius -= Input.mouseScrollDelta.y * 50f;
        orbitRadius = Mathf.Clamp(orbitRadius, minOrbitDistance, maxOrbitDistance);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 offset = rotation * new Vector3(0, 0, -orbitRadius);
        transform.position = lookAtTransform.position + offset;

        transform.LookAt(lookAtTransform);
    }

    public void GetInitialCameraTransform(out Vector3 position, out Quaternion rotation)
    {
        Quaternion rot = Quaternion.Euler(pitch, yaw, 0);
        Vector3 offset = rot * new Vector3(0, 0, -orbitRadius);
        position = lookAtTransform.position + offset;
        rotation = Quaternion.LookRotation(lookAtTransform.position - position);
    }

}
