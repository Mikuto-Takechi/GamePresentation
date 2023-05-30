using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlankController : MonoBehaviour
{
    [SerializeField] bool _moveX = false;
    [SerializeField] bool _moveY = false;
    Animator _animator = default;
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(_moveX)
        {
            _animator.SetBool("Horizontal", true);
        }
        else _animator.SetBool("Horizontal", false);

        if (_moveY)
        {
            _animator.SetBool("Vertical", true);
        }
        else _animator.SetBool("Vertical", false);
    }
}
