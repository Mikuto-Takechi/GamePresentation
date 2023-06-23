using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoaded : MonoBehaviour
{
    [SerializeField] int _playBGM = 0;
    void Start()
    {
        GManager.instance.PlayBGM(_playBGM);
    }
}
