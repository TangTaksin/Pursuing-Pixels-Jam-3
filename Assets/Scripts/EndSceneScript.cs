using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndSceneScript : MonoBehaviour
{
    public delegate void EndEvent();
    public static EndEvent OnToTitle;

    public void BackToTitle()
    {
        OnToTitle?.Invoke();
    }
}
