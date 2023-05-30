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
    private void FixedUpdate()
    {
        Debug.Log(_hp);
        if(_hp <= 0)
        {
            Title();
        }
    }
    public void Title()
    {
        SceneManager.LoadScene("TitleScene");
        _score = _scoreDefault;
        _hp = _hpDefault;
    }
}
