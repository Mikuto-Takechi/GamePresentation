using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SManager : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Stage1");
    }

    public void TitleScene()
    {
        SceneManager.LoadScene("TitleScene");
    }

    public void SecondStage()
    {
        SceneManager.LoadScene("Stage2");
    }
    public void StopEditor()
    {
        EditorApplication.isPlaying = false;
    }

}
