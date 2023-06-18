using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableBlockController : MonoBehaviour
{
    [SerializeField] float _messageDistance = 2.0f;
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
        _itemName.Add("MoveableBlock", "木箱");
        _itemName.Add("Axe", "斧");
    }

    private void Update()
    {
        float distance = Vector2.Distance((Vector2)_player.transform.position, (Vector2)transform.position);

        if (transform.position.y < -30f)
        {
            transform.position = _initPos;
        }
        if (distance <= _messageDistance)
        {
            _deleteMessage = false;
            GManager.instance.Message = $"近づいて{_itemName[transform.name]}を左クリックすることで持ち上げる。\n持ち上げたら投げたい方向へカーソルを合わせて右クリックで投げる。";
        }
        else if(distance > _messageDistance && !_deleteMessage)
        {
            _deleteMessage = true;
            GManager.instance.Message = "";
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Obstacle" && _projectile && gameObject.name == "Axe")
        {
            Vector3 pos = GameObject.Find("Main Camera").transform.position;
            AudioClip clip = collision.gameObject.GetComponent<AudioSource>().clip;
            AudioSource.PlayClipAtPoint(clip, pos);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag != "Player") _projectile = false;
    }
}
