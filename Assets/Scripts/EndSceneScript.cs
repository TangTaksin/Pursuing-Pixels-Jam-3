using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndSceneScript : MonoBehaviour
{
    public delegate void EndEvent();
    public static EndEvent OnToTitle;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void BackToTitle()
    {
        OnToTitle?.Invoke();
    }
}
