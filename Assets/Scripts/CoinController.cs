using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    [SerializeField] AudioSource _coinAudioSource = default;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            float x = collision.transform.position.x;
            float y = collision.transform.position.y;
            GManager.instance.Score += 1;
            AudioSource.PlayClipAtPoint(_coinAudioSource.clip, new Vector3(x,y,-10));
            Destroy(gameObject);
        }
    }
}
