using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWater : MonoBehaviour
{
    float _x = 0;
    float _y = 0;
    float _z = 0;
    Vector3 _initPos = Vector3.zero;
    [SerializeField] float _cycle = 1;

    private void Start()
    {
        _initPos = transform.position;
    }
    private void FixedUpdate()
    {
        _x = _cycle * Mathf.Cos(Time.time) + _initPos.x;
        _y = transform.position.y;
        _z = transform.position.z;
        transform.position = new Vector3(_x, _y, _z);
    }
}
