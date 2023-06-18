using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 1.0f;
    [SerializeField] Vector2 _lineForWall = Vector2.right;
    [SerializeField] Vector2 _lineForGround = new Vector2(1f, -1f);
    [SerializeField] LayerMask _tileMapLayer = 0;
    [SerializeField] float _interval = 3f;
    [SerializeField] GameObject _shell = default;
    GameObject _player;
    Rigidbody2D _rb;
    SpriteRenderer _sr;
    Transform _shooter;
    Transform _muzzle;
    bool _moveLeft = false;
    float _timer;

    private void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
        _shooter = transform.GetChild(0);
        _muzzle = _shooter.GetChild(0);
        _player = GameObject.Find("Player");
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_sr.isVisible && Camera.current.name != "SceneCamera" && Camera.current.name != "Preview Camera")
        {
            Move();
            _shooter.up = _player.transform.position - transform.position;
            if (_timer > _interval)
            {
                Instantiate(_shell, _muzzle.position, _shooter.rotation);
                _timer = 0;
            }
        }
    }

    void MoveWithTurn()
    {
        Vector2 start = this.transform.position;
        if (_moveLeft)
        {
            _lineForWall = Vector2.left;//左
        }
        else
        {
            _lineForWall = Vector2.right;//右
        }
        Debug.DrawLine(start, start + _lineForWall);
        RaycastHit2D hit = Physics2D.Linecast(start, start + _lineForWall, _tileMapLayer);

        if (hit.collider)
        {
            _moveLeft = !_moveLeft;
        }
    }

    void MoveOnFloorWithTurn()
    {
        Vector2 start = this.transform.position;
        if (_moveLeft)
        {
            _lineForGround = new Vector2(-1f, -1f);//左下
        }
        else
        {
            _lineForGround = new Vector2(1f, -1f);//右下
        }
        Debug.DrawLine(start, start + _lineForGround);    // ray を Scene 上に描く
        // 床の検出を試みる
        RaycastHit2D hit = Physics2D.Linecast(start, start + _lineForGround, _tileMapLayer);

        if (!hit.collider)
        {
            _moveLeft = !_moveLeft;
        }
    }

    private void Move()
    {
        MoveWithTurn();
        MoveOnFloorWithTurn();
        Vector2 velo = Vector2.zero;
        if (_moveLeft)
        {
            velo = Vector2.left * _moveSpeed;
        }
        else
        {
            velo = Vector2.right * _moveSpeed;
        }
        velo.y = _rb.velocity.y;    // 落下については現在の値を保持する
        _rb.velocity = velo;        // 速度ベクトルをセットする
    }
}
