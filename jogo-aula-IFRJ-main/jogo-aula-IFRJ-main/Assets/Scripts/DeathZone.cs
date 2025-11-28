using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathZoneController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // This code will execute when another collider enters this trigger.
        // 'other' refers to the Collider component of the object that entered.

        // Example: Check the tag of the entering object
        if (other.CompareTag("Player"))
        {
            ReloadCurrentScene();
        }
    }

    void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        // Or by build index:
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}