using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowTarget : MonoBehaviour
{
    [SerializeField] private GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = target.transform.position.x;
        float y = target.transform.position.y;
        gameObject.transform.position = new Vector3(x,y,-10);
    }
}
