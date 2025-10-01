using UnityEngine;
using UnityEngine.InputSystem; // NEW input system

public class CameraFollow : MonoBehaviour
{
    [Header("Targets")]
    public Transform player;                       // Drag your Player here
    public Vector3 offset = new Vector3(0, 3, -6); // Where the camera sits relative to player

    [Header("Feel")]
    public float followSmooth = 10f;               // Follow smoothing
    public float rotateSpeed = 60f;                // Degrees per second with arrow keys
    public bool allowTilt = true;                  // Up/Down arrows tilt if true
    public float minPitch = -20f;
    public float maxPitch = 45f;

    private float yaw;   // left/right rotation around player (Y axis)
    private float pitch; // up/down tilt

    void Start()
    {
        // Initialize yaw/pitch from current camera orientation (optional)
        Vector3 e = transform.eulerAngles;
        yaw = e.y;
        pitch = Mathf.Clamp(e.x, minPitch, maxPitch);
    }

    void LateUpdate()
    {
        if (!player) return;

        var kb = Keyboard.current;
        if (kb != null)
        {
            // Left/Right rotate around player
            if (kb.leftArrowKey.isPressed)  yaw  -= rotateSpeed * Time.deltaTime;
            if (kb.rightArrowKey.isPressed) yaw  += rotateSpeed * Time.deltaTime;

            // Optional Up/Down tilt
            if (allowTilt)
            {
                if (kb.upArrowKey.isPressed)    pitch += rotateSpeed * Time.deltaTime;
                if (kb.downArrowKey.isPressed)  pitch -= rotateSpeed * Time.deltaTime;
                pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
            }
            else
            {
                pitch = Mathf.Clamp(pitch, 0f, 0f); // keep level if tilt disabled
            }
        }

        // Compute desired camera position from yaw/pitch + offset
        Quaternion rot = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 desiredPos = player.position + rot * offset;

        // Smooth follow + look at player
        transform.position = Vector3.Lerp(transform.position, desiredPos, followSmooth * Time.deltaTime);
        transform.LookAt(player.position + Vector3.up * 1.5f);
    }
}
