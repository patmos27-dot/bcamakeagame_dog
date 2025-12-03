using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseScreen : MonoBehaviour
{
    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu"); // Loads your Main Menu scene
    }
}

