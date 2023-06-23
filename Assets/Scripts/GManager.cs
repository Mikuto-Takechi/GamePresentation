using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GManager : MonoBehaviour
{
    public static GManager instance;
    [SerializeField] int _scoreDefault = 0;
    [SerializeField] int _hpDefault = 20;
    public AudioSource _se;
    public AudioSource _loop;
    [SerializeField] List<AudioClip> _audioList;
    [SerializeField] List<AudioClip> _bgmList;
    private string _message;
    private int _score;
    private int _hp;
    public Dictionary<string, float> _timeRecords = new Dictionary<string, float>()
    {
        {"stage1",999999f },
        {"stage2",999999f },
        {"stage3",999999f },
        {"stage4",999999f }
    };
    public Dictionary<string, int> _scoreRecords = new Dictionary<string, int>()
    {
        {"stage1",0 },
        {"stage2",0 },
        {"stage3",0 },
        {"stage4",0 }
    };
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
        if (instance == null)
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
    private void FixedUpdate()
    {
        if (_hp > _hpDefault) _hp = _hpDefault;
    }
    public void PlaySound(int num)
    {
        _se.PlayOneShot(_audioList[num]);
    }
    public void PlayBGM(int num)
    {
        _loop.clip = _bgmList[num];
        _loop.Play();
    }
    public void PauseBGM(bool flag)
    {
        if (flag)
        {
            _loop.Pause();
        }
        else
        {
            _loop.UnPause();
        }
    }
    public void StopBGM()
    {
        _loop.Stop();
    }
}
