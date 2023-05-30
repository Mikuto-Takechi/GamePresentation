using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableBlockController : MonoBehaviour
{
    //[SerializeField] GameObject _player;
    //GameObject _player;
    public int _throwSpeed = 10;
    //Rigidbody2D _rigidbody;
    //Collider2D _collider;
    void Start()
    {
        //_player = GameObject.Find("Player");
        //_rigidbody = GetComponent<Rigidbody2D>();
        //_collider = GetComponent<Collider2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            GManager.instance.Message = "近づいて木箱を左クリックすることで持ち上げる。\n持ち上げたら投げたい方向へカーソルを合わせて右クリックで投げる。";
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GManager.instance.Message = "";
        }
    }
    //void OnMouseDown()
    //{
    //    PlayerController controller = _player.GetComponent<PlayerController>();
    //    if (!controller.IsHolding)
    //    {
    //        _rigidbody.isKinematic = true;
    //        _collider.enabled = false;
    //        transform.SetParent(_player.transform);
    //        float x = _player.transform.position.x;
    //        float y = _player.transform.position.y;
    //        transform.position = new Vector3(x, y + 1, 0);
    //        controller.IsHolding = true;
    //    }
    //}
}
