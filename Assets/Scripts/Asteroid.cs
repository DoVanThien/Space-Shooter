using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private float _speedRotation = 0.5f;
    [SerializeField] private GameObject _explotionPrefab;

    private SpawnManager _spawnManager;
    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, 0f, _speedRotation, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Laser"))
        {
            Instantiate(_explotionPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject, 0.25f);
            other.gameObject.SetActive(false);
            _spawnManager.StartSpawning();
        }
    }
}
