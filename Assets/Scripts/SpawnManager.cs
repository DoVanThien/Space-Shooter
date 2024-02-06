using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject[] _powerUps;
    [SerializeField] private GameObject _enemyContainer;
    [SerializeField] private float _enemySpawnRate = 5.0f;

    private bool _isStopSpawning = false;

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUp());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(2.0f);
        while(!_isStopSpawning)
        {
            Vector3 randomPositionRespawn = new Vector3(17.5f, Random.Range(-2.0f, 11.5f), 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, randomPositionRespawn, Quaternion.Euler(0f, 0f, -90f));
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(_enemySpawnRate);
        }
    }

    IEnumerator SpawnPowerUp()
    {
        yield return new WaitForSeconds(3.0f);
        while(!_isStopSpawning)
        {
            Vector3 randomPositionRespawn = new Vector3(17.5f, Random.Range(-2.0f, 11.5f), 0);
            int randomPowerUp = Random.Range(0, 3);
            Instantiate(_powerUps[randomPowerUp], randomPositionRespawn, Quaternion.Euler(0f, 0f, -90f));
            yield return new WaitForSeconds(Random.Range(10.0f, 15.0f));
        }
    }

    public void OnPlayerDeath()
    {
        _isStopSpawning = true;
    }
}
