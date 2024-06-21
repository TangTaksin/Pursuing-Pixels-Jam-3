using UnityEngine;
using UnityEngine.SceneManagement;

public class ParallelZone : MonoBehaviour
{
    // Array of scene names to switch to
    public string[] parallelScenes;

    // Reference to the player
    public GameObject player;

    // Gizmo color
    public Color gizmoColor = Color.red;

    // Function to be called when the player enters the trigger zone
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            Debug.Log("Player fell. Triggering scene change.");
            PlayerFell();
        }
    }

    // Function to randomly change to another scene
    private void PlayerFell()
    {
        if (parallelScenes.Length > 0)
        {
            // Save the player's current Z position
            GameManager.Instance.SavePlayerZPosition(player.transform.position.z);

            // Select a random scene from the array
            int randomIndex = Random.Range(0, parallelScenes.Length);
            string selectedScene = parallelScenes[randomIndex];

            Debug.Log("Changing to scene: " + selectedScene);

            // Load the selected scene
            SceneManager.LoadScene(selectedScene);
        }
        else
        {
            Debug.LogError("No parallel scenes are defined!");
        }
    }

    // Draw gizmo in the editor
    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;

        // Draw a cube where the BoxCollider is
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            Gizmos.DrawWireCube(collider.bounds.center, collider.bounds.size);
        }
    }
}
