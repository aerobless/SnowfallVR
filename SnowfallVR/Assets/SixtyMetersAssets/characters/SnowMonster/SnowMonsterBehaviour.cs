using System.Collections;
using System.Collections.Generic;
using SixtyMetersAssets.characters.SnowMonster;
using SixtyMetersAssets.Items;
using UnityEngine;
using UnityEngine.AI;

public class SnowMonsterBehaviour : MonoBehaviour, GunTarget
{
    public int health = 100;
    public float lookRadius = 10f;

    private Animator _animator;
    private readonly int _dieHash = Animator.StringToHash("Die");
    private readonly int _takeDamageHash = Animator.StringToHash("Take Damage");
    private readonly int _runForwardInPlaceHash = Animator.StringToHash("Run Forward");

    //AI
    private Transform _playerTransform;
    private NavMeshAgent _monsterNavMesh;
    private float _checkRate = 0.01f;
    private float _nextCheck;

    private SnowMonsterState _state = SnowMonsterState.Idle;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        if (GameObject.FindGameObjectWithTag("Player").activeInHierarchy)
        {
            _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }

        _monsterNavMesh = gameObject.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > _nextCheck && MonsterIsAlive())
        {
            _nextCheck = Time.time + _checkRate;
            FollowPlayer();
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        Snowball snowball = other.gameObject.GetComponent<Snowball>();
        if (snowball != null)
        {
            TakeDamage(snowball.damage);
        }
    }

    private bool MonsterIsAlive()
    {
        return _state != SnowMonsterState.Dead;
    }

    private void FollowPlayer()
    {
        float distanceToPlayer = Vector3.Distance(_playerTransform.position, transform.position);
        if (distanceToPlayer <= lookRadius)
        {
            _monsterNavMesh.transform.LookAt(_playerTransform);
            _monsterNavMesh.SetDestination(_playerTransform.position);
            _animator.SetTrigger(_runForwardInPlaceHash);
            
            if (distanceToPlayer <= _monsterNavMesh.stoppingDistance)
            {
                _monsterNavMesh.transform.LookAt(_playerTransform);
                _animator.ResetTrigger(_runForwardInPlaceHash);
            }
        }
        else
        {
            _animator.ResetTrigger(_runForwardInPlaceHash);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        _animator.SetTrigger(_takeDamageHash);
        if (health <= 0 && MonsterIsAlive())
        {
            Die();
        }
    }

    private void Die()
    {
        _state = SnowMonsterState.Dead;
        _animator.SetTrigger(_dieHash);
        Destroy(gameObject, 5);
    }
}