using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float _initSpeed = 5f;
    [SerializeField] int _damage = -2;
    private void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.up * _initSpeed;
    }

    private void Update()
    {
        if (this.transform.position.y < -20f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            var player = collision.gameObject.GetComponent<PlayerController>();
            GManager.instance.Hp += _damage;
            player.HPTransition(_damage);
            player.HurtPlay();
        }
        Destroy(this.gameObject);
    }
}
