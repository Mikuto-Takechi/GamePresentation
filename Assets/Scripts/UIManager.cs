using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject _gameOverCanvas;
    [SerializeField] GameObject _gameClearCanvas;
    [SerializeField] GameObject _gamePauseCanvas;
    [SerializeField] GameObject _manualCanvas;
    [SerializeField] GameObject _displayCanvas;
    [SerializeField] string _stageName;
    GameObject _timeManager;
    bool _gameClear = false;
    bool _gameOver = false;
    bool _gamePause = false;
    int _currentPageIndex = 0;

    public int CurrentPageIndex
    {
        get { return _currentPageIndex; }
    }

    private void Start()
    {
        _gameOverCanvas.SetActive(false);
        _gameClearCanvas.SetActive(false);
        _gamePauseCanvas.SetActive(false);
        _manualCanvas.SetActive(false);
        _timeManager = GameObject.Find("TimeManager");
    }
    private void FixedUpdate()
    {
        if (GManager.instance.Hp <= 0)
        {
            GameOver();
        }
    }
    private void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            GamePause();
        }
    }

    public void ClickSound()
    {
        GManager.instance.PlaySound(8);
    }

    public void GameClear()
    {
        _displayCanvas.SetActive(false);
        GManager.instance.StopBGM();
        GManager.instance.PlaySound(6);
        _gameClearCanvas.SetActive(true);

        string scoreStr = GManager.instance.Score.ToString();
        string scoreText = $"スコア：{scoreStr}";
        if (GManager.instance._scoreRecords[_stageName] < GManager.instance.Score)
        {
            GManager.instance._scoreRecords[_stageName] = GManager.instance.Score;
            scoreText = $"スコア：{scoreStr} 記録更新！！";
        }
        _gameClearCanvas.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = scoreText;

        float stageTime = _timeManager.GetComponent<TimeManager>().StageTime;
        string timeText = $"クリアタイム：{stageTime.ToString("F2")}";
        if (GManager.instance._timeRecords[_stageName] > stageTime)
        {
            GManager.instance._timeRecords[_stageName] = stageTime;
            timeText = $"クリアタイム：{stageTime.ToString("F2")} 記録更新！！";
        }
        _gameClearCanvas.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = timeText;
        _gameClear = true;
        Time.timeScale = 0.0f;
    }

    public void GamePause()
    {
        if( !_gameClear && !_gameOver && !_gamePause)
        {
            _displayCanvas.SetActive(false);
            GManager.instance.PauseBGM(true);
            GManager.instance.PlaySound(5);
            _gamePauseCanvas.SetActive(true);
            float stageTime = _timeManager.GetComponent<TimeManager>().StageTime;
            _gamePauseCanvas.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "現在のタイム：" + stageTime.ToString("F2");
            _gamePause = true;
            Time.timeScale = 0.0f;
        }
    }

    public void Return()
    {
        _displayCanvas.SetActive(true);
        GManager.instance.PauseBGM(false);
        _gamePauseCanvas.SetActive(false);
        _gamePause = false;
        _currentPageIndex = 0;
        Time.timeScale = 1.0f;
    }

    public void Manual()
    {
        _gamePauseCanvas.SetActive(!_gamePauseCanvas.activeSelf);
        _manualCanvas.SetActive(!_manualCanvas.activeSelf);
        if( _manualCanvas.activeSelf )
        {
            Page();
        }
    }
    public void Page()
    {
        if (_manualCanvas.activeSelf)
        {
            GameObject[] pages = { _manualCanvas.transform.GetChild(0).gameObject, _manualCanvas.transform.GetChild(1).gameObject };
            foreach (GameObject page in pages)
            {
                page.gameObject.SetActive(false);
            }
            pages[_currentPageIndex].gameObject.SetActive(true);
            _manualCanvas.transform.Find("Page").GetChild(0).GetComponent<Text>().text = $"ページ切り替え\n{_currentPageIndex + 1} / {pages.Length}";
            _currentPageIndex++;
            _currentPageIndex %= pages.Length;
        }
    }

    private void GameOver()
    {
        _displayCanvas.SetActive(false);
        GManager.instance.StopBGM();
        GManager.instance.PlaySound(3);
        _gameOverCanvas.SetActive(true);
        string scoreText = GManager.instance.Score.ToString();
        _gameOverCanvas.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = $"最終スコア{scoreText}";
        _gameOver = true;
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
        Time.timeScale = 1.0f;
    }

    public void Next()
    {

    }
}
