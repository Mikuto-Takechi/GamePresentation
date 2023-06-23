using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SManager : MonoBehaviour
{
    [SerializeField] GameObject _stageRecord1;
    [SerializeField] GameObject _stageRecord2;
    [SerializeField] GameObject _stageRecord3;
    [SerializeField] GameObject _stageRecord4;
    private void FixedUpdate()
    {
        if(_stageRecord1)
        {
            _stageRecord1.GetComponent<Text>().text = $"タイム：{GManager.instance._timeRecords["stage1"].ToString("F2")}\nスコア：{GManager.instance._scoreRecords["stage1"]}";
        }
        if (_stageRecord2)
        {
            _stageRecord2.GetComponent<Text>().text = $"タイム：{GManager.instance._timeRecords["stage2"].ToString("F2")}\nスコア：{GManager.instance._scoreRecords["stage2"]}";
        }
        if (_stageRecord3)
        {
            _stageRecord3.GetComponent<Text>().text = $"タイム：{GManager.instance._timeRecords["stage3"].ToString("F2")}\nスコア：{GManager.instance._scoreRecords["stage3"]}";
        }
        if (_stageRecord4)
        {
            _stageRecord4.GetComponent<Text>().text = $"タイム：{GManager.instance._timeRecords["stage4"].ToString("F2")}\nスコア：{GManager.instance._scoreRecords["stage4"]}";
        }
    }
    public void StartGame(int num)
    {
        if (num <= 0) num = 1;
        SceneManager.LoadScene(num);
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
