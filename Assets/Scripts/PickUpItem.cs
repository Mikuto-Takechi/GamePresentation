using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    [SerializeField] AudioClip _coinAudioClip = default;
    [SerializeField] AudioClip _cherryAudioClip = default;
    [SerializeField] int _healAmount = 3;
    [SerializeField] string _prefabName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            float x = collision.transform.position.x;
            float y = collision.transform.position.y;
            if (_prefabName == "Coin")
            {
                GManager.instance.Score += 1;
                AudioSource.PlayClipAtPoint(_coinAudioClip, new Vector3(x, y, -10));
            }
            if (_prefabName == "Cherry")
            {
                GManager.instance.Hp += _healAmount;
                collision.gameObject.GetComponent<PlayerController>().HPTransition(_healAmount);
                AudioSource.PlayClipAtPoint(_cherryAudioClip, new Vector3(x, y, -10));
            }
            Destroy(gameObject);
        }
    }
}
