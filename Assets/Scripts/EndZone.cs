using UnityEngine;
using UnityEngine.SceneManagement;

public class EndZone : MonoBehaviour
{
    public Animator _animator;
    [SerializeField] private GameObject player;
    [SerializeField] private string nextSceneName; // The specific scene to load
    [SerializeField] private Color gizmoColor = Color.red;

    private void OnEnable()
    {
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
        _animator.Play("FadeCanvas_In_End");
    }

    // Animation event function called from 'FadeCanvas_In' animation
    public void LoadScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            
            SceneManager.LoadScene(nextSceneName);
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
