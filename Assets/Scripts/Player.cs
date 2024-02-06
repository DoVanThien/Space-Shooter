using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _tripleShot;
    [SerializeField] private GameObject _shieldVisualizer;
    [SerializeField] private GameObject _engineRight;
    [SerializeField] private GameObject _engineLeft;
    [SerializeField] private AudioClip _laserAudioClip;

    [SerializeField] private float _speed = 6f;
    [SerializeField] private float _fireRate = 0.4f;
    [SerializeField] private int _lives = 3;
    [SerializeField] private bool _isTripleShot = false;
    [SerializeField] private bool _isShieldPower = false;
    [SerializeField] private int _score;

    private float _canFire = 0f;
    private SpawnManager _spawnManager;
    private UIManager _uIManager;
    private AudioSource _audioSource;

    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();

        if (_spawnManager == null)
        {
            Debug.LogError("SpawnManager is null");
        }

        if (_uIManager == null)
        {
            Debug.LogError("UIManager is null");
        }

        if(_audioSource == null)
        {
            Debug.LogError("AudioSource is null");
        }
        else
        {
            _audioSource.clip = _laserAudioClip;
        }
    }

    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        float currentPositionX = transform.position.x;
        float currentPositionY = transform.position.y;
        Vector3 direction = new Vector3(-verticalInput, horizontalInput, 0);

        transform.position = new Vector3(Mathf.Clamp(currentPositionX, -12.5f, -3.0f), Mathf.Clamp(currentPositionY, -1.5f, 11.5f), 0.0f);
        transform.Translate(_speed * Time.deltaTime * direction);
    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;
        //GameObject tripleLaser = ObjectPool.SharedInstance.GetTripleLaserPooledObject();
        //GameObject laser = ObjectPool.SharedInstance.GetLaserPooledObject();
        if (_isTripleShot == true)
        {
            //tripleLaser.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
            //tripleLaser.SetActive(true);
            Instantiate(_tripleShot, transform.position, Quaternion.Euler(0, 0, -90));
        }
        else
        {
            Instantiate(_laserPrefab, transform.position, Quaternion.Euler(0, 0, -90));
        }
        _audioSource.Play();
    }

    public void Damage()
    {
        if(_isShieldPower == true)
        {
            _isShieldPower = false;
            _shieldVisualizer.SetActive(false);
            return;
        }

        _lives--;
        
        _uIManager.UpdateLivesDisplay(_lives);
        if(_lives == 2)
        {
            _engineRight.SetActive(true);
        }
        else if(_lives == 1)
        {
            _engineLeft.SetActive(true);
        }
        else if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
            _uIManager.UpdateGameOverDisplay();
        }       
    }

    public void ShieldPowerActive()
    {
        _isShieldPower = true;
        _shieldVisualizer.SetActive(true);
        StartCoroutine(ShieldPowerDownRoutine());
    }

    IEnumerator ShieldPowerDownRoutine()
    {
        yield return new WaitForSeconds(10.0f);
        _isShieldPower = false;
        _shieldVisualizer.SetActive(false);
    }

    public void TripleShotActive()
    {
        _isTripleShot = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShot = false;
    }

    public void SpeedUpPowerActive()
    {
        _speed += 5.0f;
        StartCoroutine(SpeedUpPowerDownRoutine());
    }

    IEnumerator SpeedUpPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _speed = 6.0f;
    }

    public void AddScore(int points)
    {
        _score += points;
        _uIManager.UpdateScore(_score);
    }
}
