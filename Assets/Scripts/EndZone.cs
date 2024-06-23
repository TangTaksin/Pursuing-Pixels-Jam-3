using UnityEngine;
using UnityEngine.SceneManagement;

public class EndZone : MonoBehaviour
{
    private Animator _animator;
    [SerializeField] private GameObject player;
    [SerializeField] private string nextSceneName; // The specific scene to load
    [SerializeField] private Color gizmoColor = Color.red;

    private void OnEnable()
    {
        _animator = GetComponent<Animator>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        Debug.Log("Scene loaded");
        _animator.Play("FadeCanvas_Out");
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            Debug.Log("Player fell. Triggering scene change.");
            PlayerWin();
        }
    }

    private void PlayerWin()
    {
        // Save the player's current Z position
        GameManager.Instance.SavePlayerZPosition(player.transform.position.z);
        // Play fade animation, then load the scene.
        _animator.Play("FadeCanvas_In");
        // No need to invoke here; the animation event will handle the delay.
    }

    // Animation event function called from 'FadeCanvas_In' animation
    public void LoadScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            Debug.Log("Changing to scene: " + nextSceneName);
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogError("Next scene name is not defined!");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            Gizmos.DrawWireCube(collider.bounds.center, collider.bounds.size);
        }
    }
}
