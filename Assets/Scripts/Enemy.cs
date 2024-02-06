using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _enemySpeed = 5f;
    [SerializeField] private int _lived = 3;
    [SerializeField] private AudioClip _explotionAudioClip;
    [SerializeField] private GameObject _doubleShot;

    private Player _player;
    private AudioSource _audioSource;
    Animator _anim;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();

        if (_anim == null)
        {
            Debug.LogError("Animator is null");
        }

        if(_audioSource == null)
        {
            Debug.LogError("AudioSource is null");
        }
        else
        {
            _audioSource.clip = _explotionAudioClip;
        }
    }

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        
        if(_player == null)
        {
            Debug.LogError("Player is null");
        }
    }

    void Update()
    {
        transform.Translate(_enemySpeed * Time.deltaTime * Vector3.down);

        if(transform.position.x < -17.5f)
        {
            RespawnObject();
        }

        //EnemyShotAtPlayer();
    }

    void RespawnObject()
    {
        float randomRangePositionY = Random.Range(-2.0f, 11.5f);
        Vector3 randomPositionY = new Vector3(17.5f, randomRangePositionY, 0);
        transform.position = randomPositionY;
        _lived = 3;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Player player = other.transform.GetComponent<Player>();
            if(player != null)
            {
                player.Damage();
            }
            SetAnimatorEnemyDestroy();
            GetComponent<Collider>().enabled = false;
            Destroy(this.gameObject, 2.0f);
            _audioSource.Play();
        }
        else if(other.CompareTag("Laser"))
        {
            //Transform parent = other.transform.parent;
            //if(parent != null)
            //{
            //    parent.gameObject.SetActive(false);
            //}
            Destroy(other.gameObject);

            if(_lived > 1)
            {
                _lived--;
            }
            else if(_lived == 1 && _player != null)
            {
                SetAnimatorEnemyDestroy();
                GetComponent<Collider>().enabled = false;
                Destroy(this.gameObject, 2.0f);
                _audioSource.Play();
                _player.AddScore(1);
            }
        }
    }

    private void SetAnimatorEnemyDestroy()
    {
        _anim.SetTrigger("OnEnemyDeath");
        _enemySpeed = 0f;
    }

    //void EnemyShotAtPlayer()
    //{
    //    _cooldownTime -= Time.time;
    //    if(_cooldownTime > 0) return;

    //    _cooldownTime = _fireRate;
    //    Instantiate(_doubleShot, gameObject.transform.position - new Vector3(), Quaternion.identity);
    //    Debug.Log("Enemy shot");
    //}
}
