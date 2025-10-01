using UnityEngine;
using UnityEngine.InputSystem; // NEW input system

[RequireComponent(typeof(Rigidbody))]
public class DogMovement : MonoBehaviour
{
    [Header("References")]
    public Transform player;              // Drag your Player here

    [Header("Ring Settings")]
    public float desiredRadius = 2.5f;    // circle distance from player
    public float radiusTolerance = 0.25f; // slack so it doesn’t jitter

    [Header("Motion Feel")]
    public float maxSpeed = 2.0f;         // top speed
    public float positionLag = 0.3f;      // smoothing
    public float rotationSmooth = 10f;    // turning speed

    [Header("Jump Settings")]
    public float jumpForce = 5f;          // jump strength
    private bool isGrounded = true;
    private Rigidbody rb;

    private Vector3 smoothVel = Vector3.zero;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!player) return;

        // --- Circle follow logic ---
        Vector3 toDog = transform.position - player.position;
        toDog.y = 0f;

        if (toDog.sqrMagnitude < 0.0001f)
            toDog = Vector3.forward * desiredRadius;

        float dist = toDog.magnitude;
        Vector3 dir = toDog / dist;

        Vector3 targetOnRing = player.position + dir * desiredRadius;

        float inner = desiredRadius - radiusTolerance;
        float outer = desiredRadius + radiusTolerance;
        if (dist > outer) targetOnRing = player.position + dir * outer;
        else if (dist < inner) targetOnRing = player.position + dir * inner;

        targetOnRing.y = transform.position.y;

        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetOnRing,
            ref smoothVel,
            positionLag,
            maxSpeed
        );

        Vector3 look = (player.position - transform.position);
        look.y = 0f;
        if (look.sqrMagnitude > 0.0001f)
        {
            Quaternion face = Quaternion.LookRotation(look.normalized, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, face, rotationSmooth * Time.deltaTime);
        }

        // --- Jump input with new Input System ---
        var kb = Keyboard.current;
        if (kb != null && kb.jKey.wasPressedThisFrame && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }
}