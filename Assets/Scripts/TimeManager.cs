using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    float _stageTime = 0.0f;
    Text _timeText;

    public float StageTime
    {
        get { return  _stageTime; }
    }

    private void Start()
    {
        _timeText = GameObject.Find("DisplayCanvas/Time").GetComponent<Text>();
    }
    private void FixedUpdate()
    {
        _stageTime += Time.deltaTime;
        if(_timeText)
        {
            _timeText.text = $"Time: {Mathf.Ceil(_stageTime)}";
        }
    }
}
