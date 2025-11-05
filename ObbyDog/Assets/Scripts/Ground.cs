using UnityEngine;

public class Ground : MonoBehaviour
{
    // Reference to the collider (so we can ensure it's active)
    private Collider groundCollider;

    void Awake()
    {
        // Try to find the collider automatically
        groundCollider = GetComponent<Collider>();

        // If there's no collider, add a box collider as default
        if (groundCollider == null)
        {
            groundCollider = gameObject.AddComponent<BoxCollider>();
        }

        // Make sure the collider is enabled
        groundCollider.enabled = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Optional: you can add special logic here if the player hits the ground
        // Example: stop player from falling through
        if (collision.gameObject.CompareTag("Player"))
        {
            // do nothing special yet; the collider alone handles the block
        }
    }
}
