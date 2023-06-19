using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] int _maxHealth = 6;
    [SerializeField] int _killScore = 10;
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
    Slider _healthSlider;
    Text _healthText;
    Text _damageText;
    bool _moveLeft = false;
    int _currentHealth = 0;
    float _timer, _damageUITimer, _damageUIInterval = 1.0f;

    public int CurrentHealth
    {
        set { _currentHealth = value; }
        get { return _currentHealth; }
    }

    private void Start()
    {
        _currentHealth = _maxHealth;
        _sr = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
        _shooter = transform.GetChild(0);
        _muzzle = _shooter.GetChild(0);
        _healthSlider = transform.GetChild(1).GetChild(0).GetComponent<Slider>();
        _healthText = transform.GetChild(1).GetChild(1).GetComponent<Text>();
        _damageText = transform.GetChild(1).GetChild(2).GetComponent<Text>();
        _player = GameObject.Find("Player");
        if(_damageText) _damageText.text = "";
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        _damageUITimer += Time.deltaTime;
        _healthSlider.value = (float)_currentHealth / (float)_maxHealth;
        _healthText.text = "HP: " + _currentHealth.ToString() + " / " + _maxHealth.ToString();
        if(_currentHealth <= 0)
        {
            GManager.instance.Score += _killScore;
            Destroy(gameObject);
        }
        if(_damageUITimer > _damageUIInterval)
        {
            _damageText.text = "";
        }
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
            _lineForWall = Vector2.left;//��
        }
        else
        {
            _lineForWall = Vector2.right;//�E
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
            _lineForGround = new Vector2(-1f, -1f);//����
        }
        else
        {
            _lineForGround = new Vector2(1f, -1f);//�E��
        }
        Debug.DrawLine(start, start + _lineForGround);    // ray �� Scene ��ɕ`��
        // ���̌��o�����݂�
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
        velo.y = _rb.velocity.y;    // �����ɂ��Ă͌��݂̒l��ێ�����
        _rb.velocity = velo;        // ���x�x�N�g�����Z�b�g����
    }

    public void DamageUI(int dam)
    {
        _damageText.text = $"-{dam}";
        _damageUITimer = 0.0f;
    }
}
