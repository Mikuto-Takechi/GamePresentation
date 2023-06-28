using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [SerializeField] GameObject _mainCanvas;
    [SerializeField] GameObject _stageSelectCanvas;
    [SerializeField] Slider _bgmSlider;
    [SerializeField] Slider _seSlider;
    void Start()
    {
        _stageSelectCanvas.SetActive(false);
        if (_bgmSlider) _bgmSlider.value = GManager.instance._loop.volume;
        if(_seSlider) _seSlider.value = GManager.instance._se.volume;
    }

    public void StageSelect()
    {
        _stageSelectCanvas.SetActive(!_stageSelectCanvas.activeSelf);
        _mainCanvas.SetActive(!_mainCanvas.activeSelf);
        if (_stageSelectCanvas.activeSelf)
        {
            _stageSelectCanvas.transform.Find("Stage1")?.GetComponent<Button>().Select();
        }
        if (_mainCanvas.activeSelf)
        {
            _mainCanvas.transform.Find("Start")?.GetComponent<Button>().Select();
        }
    }
    public void VolumeBGM(float volume)
    {
        GManager.instance._loop.volume = volume;
    }
    public void VolumeSE(float volume)
    {
        GManager.instance._se.volume = volume;
    }
    public void ClickSound()
    {
        GManager.instance.PlaySound(8);
    }
}
