using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    [SerializeField] Text _text1 = default;
    [SerializeField] Text _text2 = default;
    [SerializeField] Text _text3 = default;

    void Update()
    {
        _text1.text = GManager.instance.Score.ToString();
        _text2.text = GManager.instance.Hp.ToString();
        _text3.text = GManager.instance.Message;
    }
}
