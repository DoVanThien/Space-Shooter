using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float _speed = 8f;

    void Update()
    {
        transform.Translate(_speed * Time.deltaTime * Vector3.up);
        if(transform.position.x > 16f)
        {
            Transform parent = transform.parent;
            if (parent != null)
            {
                Destroy(parent.gameObject);
            }
            Destroy(gameObject);
        }
    }
}
