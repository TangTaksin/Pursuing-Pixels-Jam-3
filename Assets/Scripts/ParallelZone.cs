using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class ParallelZone : MonoBehaviour
{
    bool gameStarted = true;

    Animator _animator;

    public int fallLimits;
    int fallCounts;

    public HealthUI _healthUI;
    public TextMeshProUGUI _countdownTxt;

    public float totalCountdownTime = 3;

    // Array of scene names to switch to
    public string[] parallelScenes;

    // Reference to the player
    public GameObject player;

    // Gizmo color
    public Color gizmoColor = Color.red;

    public delegate void TimerEvent();
    public static TimerEvent onCountdownStart;
    public static TimerEvent onCountdownOver;

    private void OnEnable()
    {
        _animator = GetComponent<Animator>();
        TightRopeWalker.OnFall += PlayerFell;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        TightRopeWalker.OnFall -= PlayerFell;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        print("scene loaded");
        _animator.Play("FadeCanvas_Out");
        player = GameObject.FindGameObjectWithTag("Player");

        if (fallCounts <= fallLimits && gameStarted)
            StartCoroutine(Countdown());

        _healthUI.UpdateHealthUI(fallCounts, fallLimits);
    }

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
        fallCounts++;

        if (parallelScenes.Length > 0)
        {
            // Save the player's current Z position
            GameManager.Instance.SavePlayerZPosition(player.transform.position.z);

            // Play fade animation, then load the scene.
            _animator.Play("FadeCanvas_In");
            
        }
        else
        {
            Debug.LogError("No parallel scenes are defined!");
        }
    }

    //This will be execute from animation event.
    public void LoadScene()
    {
        // Select a random scene from the array
        int randomIndex = Random.Range(0, parallelScenes.Length);
        string selectedScene = parallelScenes[randomIndex];

        Debug.Log("Changing to scene: " + selectedScene);

        if (selectedScene == SceneManager.GetActiveScene().name)
        {
            if (randomIndex == 0)
                randomIndex++;
            else if (randomIndex == parallelScenes.Length-1)
                randomIndex--;

            selectedScene = parallelScenes[randomIndex];
        }

        if (fallCounts > fallLimits)
        {
            selectedScene = "FailScene";
        }

        SceneManager.LoadScene(selectedScene);
    }

    IEnumerator Countdown()
    {
        onCountdownStart?.Invoke();

        _countdownTxt.gameObject.SetActive(true);
        _countdownTxt.text = "Prepare\n3";
        yield return new WaitForSeconds(totalCountdownTime/3);

        _countdownTxt.text = "Prepare\n2";
        yield return new WaitForSeconds(totalCountdownTime/3);

        _countdownTxt.text = "Prepare\n1";
        yield return new WaitForSeconds(totalCountdownTime/3);

        _countdownTxt.gameObject.SetActive(false);
        onCountdownOver?.Invoke();
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
