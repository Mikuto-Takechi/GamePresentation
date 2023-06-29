using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldableItem : MonoBehaviour
{
    [SerializeField] float _messageDistance = 2.0f;
    [SerializeField] int _hitDamage = -3;
    [SerializeField] ContactFilter2D filter2d;
    [SerializeField] Material _outlineMat;
    Material _defaultMat;
    public int _throwSpeed = 10;
    public bool _projectile = false;
    private bool _deleteMessage = false;
    GameObject _player;
    Dictionary<string, string> _itemName = new Dictionary<string, string>();
    Vector3 _initPos;

    private void Start()
    {
        _initPos = transform.position;
        _player = GameObject.Find("Player");
        _itemName.Add("MoveableBlock", "近づいて木箱を左クリックすることで持ち上げる。\n持ち上げたら投げたい方向へカーソルを合わせて右クリックすることで投げることができる。");
        _itemName.Add("Axe", "斧は木箱と同様に持ち上げて投げることができる。\n投げた後に敵に当たればダメージを与え、障害物に当たれば壊すことができる。");
        _defaultMat = GetComponent<SpriteRenderer>().material;
    }

    private void Update()
    {
        float distance = Vector2.Distance((Vector2)_player.transform.position, (Vector2)transform.position);
        InTheAir();

        if (transform.position.y < -30f)
        {
            transform.position = _initPos;
        }
        if (distance <= _messageDistance)
        {
            _deleteMessage = false;
            GManager.instance.Message = _itemName[gameObject.name];
        }
        else if (distance > _messageDistance && !_deleteMessage)
        {
            _deleteMessage = true;
            GManager.instance.Message = "";
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (gameObject.name == "Axe")
        {
            if (collision.gameObject.tag == "Obstacle" && _projectile)
            {
                GManager.instance.PlaySound(1);
                Destroy(collision.gameObject);
            }
            if (collision.gameObject.tag == "Enemy" && _projectile)
            {
                GManager.instance.PlaySound(7);
                var enemy = collision.gameObject.GetComponent<EnemyController>();
                enemy.CurrentHealth += _hitDamage;
                enemy.HPTransition(_hitDamage);
            }
        }
    }

    void InTheAir()
    {
        if (gameObject.name == "Axe")
        {
            var isTouched = GetComponent<Rigidbody2D>().IsTouching(filter2d);
            if (isTouched)
            {
                if (_projectile) _projectile = false;
            }
            else
            {
                if (!_projectile) _projectile = true;
            }
        }
    }
    public void SwitchHighLight(bool flag)
    {
        if (flag)
        {
            GetComponent<SpriteRenderer>().material = _outlineMat;
        }
        else
        {
            GetComponent<SpriteRenderer>().material = _defaultMat;
        }
    }
}
