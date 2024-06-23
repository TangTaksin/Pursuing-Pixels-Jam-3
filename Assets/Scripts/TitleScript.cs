using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScript : MonoBehaviour
{
    public delegate void TitleEvent();
    public static TitleEvent OnPlay;

    public GameObject healthUI, balanceUI, countdownUI;
    public Animator cam_animator;

    private void OnEnable()
    {

        // initiate
        // hide health ui, balance ui, countdown ui
        healthUI.SetActive(false);
        balanceUI.SetActive(false);
        countdownUI.SetActive(false);

    }

    public void StartGame()
    {
        OnPlay?.Invoke();
        balanceUI.SetActive(true);
        gameObject.SetActive(false);
        cam_animator.SetTrigger("ToGameCam");
    }
}
