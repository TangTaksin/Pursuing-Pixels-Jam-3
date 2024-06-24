using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseSystem : MonoBehaviour
{
    bool isPaused = false;
    bool countingDown = false;

    public delegate void pauseEvent();
    public static pauseEvent OnPaused;
    public static pauseEvent OnResumed;

    public GameObject pauseContainer;
    public Slider music_Slider, sfx_Slider, mouseSen_Slider;
    public TextMeshProUGUI music_txt, sfx_text, mouseSen_txt;

    TightRopeWalker _walker;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
        ParallelZone.onCountdownStart += () => { countingDown = true; };
        ParallelZone.onCountdownOver += () => { countingDown = false; };

        mouseSen_Slider.onValueChanged.AddListener(delegate { OnSenValueChanged(); });
    }

    void OnSceneLoad(Scene scene, LoadSceneMode loadSceneMode)
    {
        var find = GameObject.FindGameObjectWithTag("Player");
        if (find)
            find.TryGetComponent<TightRopeWalker>(out _walker);

        if (_walker)
            _walker.SetSensitivity(mouseSen_Slider.value);

        mouseSen_txt.text = string.Format("x{0:0.00}", mouseSen_Slider.value);
    }

    void OnSenValueChanged()
    {
        _walker.SetSensitivity(mouseSen_Slider.value);
        mouseSen_txt.text = string.Format("x{0:0.00}", mouseSen_Slider.value);
    }

    public void OnPause()
    {
        if (countingDown)
            return;

        isPaused = !isPaused;

        if (isPaused)
        {
            OnPaused?.Invoke();
            pauseContainer.SetActive(true);
        }
        else
        {
            OnResumed?.Invoke();
            pauseContainer.SetActive(false);
        }
    }
}
