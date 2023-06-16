using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableBlockController : MonoBehaviour
{
    public int _throwSpeed = 10;
    public bool _projectile = false;
    void Start()
    {
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            GManager.instance.Message = "近づいて木箱を左クリックすることで持ち上げる。\n持ち上げたら投げたい方向へカーソルを合わせて右クリックで投げる。";
        }
        if(collision.gameObject.tag == "Obstacle" && _projectile && gameObject.name == "Axe")
        {
            Vector3 pos = GameObject.Find("Main Camera").transform.position;
            AudioClip clip = collision.gameObject.GetComponent<AudioSource>().clip;
            AudioSource.PlayClipAtPoint(clip, pos);
            Destroy(collision.gameObject);
        }
        if(collision.gameObject.tag != "Player")_projectile = false;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GManager.instance.Message = "";
        }
    }
}
