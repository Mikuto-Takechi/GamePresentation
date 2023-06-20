using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SManager : MonoBehaviour
{
    [SerializeField] GameObject _recordUI;
    private void FixedUpdate()
    {
        if(_recordUI)
        {
            _recordUI.GetComponent<Text>().text = $"現在最速クリアタイム：{GManager.instance.TimeRecord.ToString("F2")}\n現在最高スコア：{GManager.instance.ScoreRecord}";
        }
    }
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
