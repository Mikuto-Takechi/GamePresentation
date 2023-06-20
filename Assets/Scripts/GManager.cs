using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GManager : MonoBehaviour
{
    public static GManager instance;
    [SerializeField] int _scoreDefault = 0;
    [SerializeField] int _hpDefault = 20;
    private string _message;
    private int _score;
    private int _hp;
    float _timeRecord = 999999f;
    int _scoreRecord;

    public float TimeRecord
    {
        set { _timeRecord = value; }
        get { return _timeRecord; }
    }

    public int ScoreRecord
    {
        set { _scoreRecord = value; }
        get { return _scoreRecord; }
    }

    public string Message
    {
        set { _message = value; }
        get { return _message; }
    }
    public int Score
    {
        set { _score = value; }
        get { return _score; }
    }
    public int Hp
    {
        set { _hp = value; }
        get { return _hp; }
    }
    public int hpDefault
    {
        get { return _hpDefault; }
    }
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        _score = _scoreDefault;
        _hp = _hpDefault;
    }
}
