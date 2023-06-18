using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VannerController : MonoBehaviour
{
    GameObject _uiManager;
    private void Start()
    {
        _uiManager = GameObject.Find("UIManager");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _uiManager.GetComponent<UIManager>().GameClear();
        }
    }
}
