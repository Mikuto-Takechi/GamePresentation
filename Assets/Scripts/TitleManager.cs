using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    [SerializeField] GameObject _mainCanvas;
    [SerializeField] GameObject _stageSelectCanvas;
    void Start()
    {
        _stageSelectCanvas.SetActive(false);
    }

    public void StageSelect()
    {
        _stageSelectCanvas.SetActive(!_stageSelectCanvas.activeSelf);
        _mainCanvas.SetActive(!_mainCanvas.activeSelf);
    }
}
