using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlankController : MonoBehaviour
{
    [SerializeField] Transform[] _targets;
    [SerializeField] float _moveSpeed = 1.0f;
    [SerializeField] float _rotateSpeed = 1.0f;
    [SerializeField] float _stoppingDistance = 0.05f;
    [SerializeField] float _l = 5;
    [SerializeField] bool _toggleRotateMove = false;
    [SerializeField] int _dNum = 1;
    [SerializeField] int _num = 1;
    int _currentTargetIndex = 0;
    float _th = 0.0f;

    private void Update()
    {
        if (!_toggleRotateMove)
        {
            PatrolMove();
        }
        else
        {
            RotateMove();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Holdable" || collision.gameObject.tag == "Enemy")
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Holdable" || collision.gameObject.tag == "Enemy")
        {
            collision.transform.SetParent(null);
        }
    }

    void PatrolMove()
    {
        float distance = Vector2.Distance(this.transform.position, _targets[_currentTargetIndex].position);

        if (distance > _stoppingDistance)
        {
            Vector3 dir = (_targets[_currentTargetIndex].transform.position - this.transform.position).normalized * _moveSpeed;
            this.transform.Translate(dir * Time.deltaTime);
        }
        else
        {
            _currentTargetIndex++;
            _currentTargetIndex %= _targets.Length;
        }
    }

    void RotateMove()
    {
        float angle = _num * (360 / _dNum) + 90f;
        float rad = angle * Mathf.Deg2Rad;
        float x = _targets[_currentTargetIndex].position.x + _l * Mathf.Cos(_th * _rotateSpeed + rad);
        float y = _targets[_currentTargetIndex].position.y + _l * Mathf.Sin(_th * _rotateSpeed + rad);
        gameObject.transform.position = new Vector3(x, y, 0);
        _th += Time.deltaTime;
    }
}
