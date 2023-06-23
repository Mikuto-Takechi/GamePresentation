using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
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
                GManager.instance.PlaySound(0);
            }
            if (_prefabName == "Cherry")
            {
                GManager.instance.Hp += _healAmount;
                collision.gameObject.GetComponent<PlayerController>().HPTransition(_healAmount);
                GManager.instance.PlaySound(4);
            }
            Destroy(gameObject);
        }
    }
}
