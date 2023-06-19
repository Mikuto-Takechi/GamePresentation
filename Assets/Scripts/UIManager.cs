using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] AudioClip _gameOverSound;
    [SerializeField] AudioClip _gameClearSound;
    [SerializeField] GameObject _gameOverCanvas;
    [SerializeField] GameObject _gameClearCanvas;
    [SerializeField] GameObject _displayCanvas;
    [SerializeField] GameObject _bGM;
    GameObject _timeManager;
    AudioSource _audioSource;

    private void Start()
    {
        _gameOverCanvas.SetActive(false);
        _gameClearCanvas.SetActive(false);
        _audioSource = gameObject.GetComponent<AudioSource>();
        _timeManager = GameObject.Find("TimeManager");
    }
    private void FixedUpdate()
    {
        if (GManager.instance.Hp <= 0)
        {
            GameOver();
        }
    }

    public void GameClear()
    {
        _displayCanvas.SetActive(false);
        _bGM.SetActive(false);
        _audioSource.PlayOneShot(_gameClearSound);
        _gameClearCanvas.SetActive(true);
        string scoreText = GManager.instance.Score.ToString();
        _gameClearCanvas.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = $"最終スコア：{scoreText}";
        float stageTime = _timeManager.GetComponent<TimeManager>().StageTime;
        _gameClearCanvas.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = $"クリアタイム：{Mathf.Ceil(stageTime)}";
        Time.timeScale = 0.0f;
    }

    private void GameOver()
    {
        _displayCanvas.SetActive(false);
        _bGM.SetActive(false);
        _audioSource.PlayOneShot(_gameOverSound);
        _gameOverCanvas.SetActive(true);
        string scoreText = GManager.instance.Score.ToString();
        _gameOverCanvas.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = $"最終スコア{scoreText}";
        Time.timeScale = 0.0f;
    }
    public void Title()
    {
        SceneManager.LoadScene("TitleScene");
        GManager.instance.Score = 0;
        GManager.instance.Hp = GManager.instance.hpDefault;
        Time.timeScale = 1.0f;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GManager.instance.Score = 0;
        GManager.instance.Hp = GManager.instance.hpDefault;
        _gameOverCanvas.SetActive(false);
        _displayCanvas.SetActive(true);
        _bGM.SetActive(true);
        Time.timeScale = 1.0f;
    }

    public void Next()
    {

    }
}
