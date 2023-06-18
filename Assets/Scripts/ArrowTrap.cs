using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTrap : MonoBehaviour
{
    [SerializeField] float _interval = 3f;
    [SerializeField] GameObject _shell = default;
    float _timer;
    private void Update()
    {
        _timer += Time.deltaTime;
        if(_timer > _interval )
        {
            Transform muzzle = transform.GetChild(0);
            Instantiate(_shell, muzzle.position, this.transform.rotation);
            _timer = 0;
        }
    }
}
