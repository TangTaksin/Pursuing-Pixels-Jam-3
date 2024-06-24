using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class ParallelZone : MonoBehaviour
{
    bool gameStarted = false;

    Animator _animator;
    public GameObject FadeObject;

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
        TitleScript.OnPlay += OnGameStart;
        PauseSystem.OnResumed += OnResume;
        EndSceneScript.OnToTitle += Restart;
    }

    private void OnDisable()
    {
        TightRopeWalker.OnFall -= PlayerFell;
        SceneManager.sceneLoaded -= OnSceneLoaded;
        TitleScript.OnPlay -= OnGameStart;
        PauseSystem.OnResumed -= OnResume;
        EndSceneScript.OnToTitle -= Restart;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        print("scene loaded");
        _animator.Play("FadeCanvas_Out");
        player = GameObject.FindGameObjectWithTag("Player");

        if (player != null && gameStarted)
        {
            Vector3 currentPosition = player.transform.position;
            player.transform.position = new Vector3(currentPosition.x, currentPosition.y, GameManager.Instance.playerZPosition);
        }

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
            PlayFadeIn();
        }
        else
        {
            Debug.LogError("No parallel scenes are defined!");
        }
    }

    void PlayFadeIn()
    {
        FadeObject?.SetActive(true);
        _animator.Play("FadeCanvas_In");
    }

    void Restart()
    {
        gameStarted = false;
        PlayFadeIn();
    }

    void UnactivateFade()
    {
        FadeObject?.SetActive(false);
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

        var scName = SceneManager.GetActiveScene().name;

        if (scName == "FailScene")
        {
            SceneManager.LoadScene("StartScene");

            return;
        }

        if (fallCounts > fallLimits)
        {
            SceneManager.LoadScene("FailScene");

            return;
        }

        SceneManager.LoadScene(selectedScene);
    }

    void OnGameStart()
    {
        StartCoroutine(Countdown());
        _healthUI.gameObject.SetActive(true);
        gameStarted = true;
    }

    void OnResume()
    {
        if (gameStarted)
            StartCoroutine(Countdown());
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
