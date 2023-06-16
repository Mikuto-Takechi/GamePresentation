using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextController : MonoBehaviour
{
    [SerializeField] Text _text1 = default;
    [SerializeField] Text _text2 = default;
    [SerializeField] Text _text3 = default;

    void Update()
    {
        _text1.text = $"Score:{GManager.instance.Score}";
        _text2.text = $"HP:{GManager.instance.Hp}";
        _text3.text = GManager.instance.Message;
    }
}
