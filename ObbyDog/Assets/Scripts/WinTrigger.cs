using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    [SerializeField] private GameObject winPanel; // drag your WinPanel here

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
    {
        Hud.StopTimer();          // freezes your HUD timer
        // Optional: freeze gameplay AFTER showing panel
        Time.timeScale = 0f;
    }

    }
}
