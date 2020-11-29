using System.Collections;
using System.Collections.Generic;
using SixtyMetersAssets.characters.SnowMonster;
using SixtyMetersAssets.Items;
using UnityEngine;
using UnityEngine.AI;

public class SnowMonsterBehaviour : MonoBehaviour, GunTarget
{
    public int health = 100;

    private Animator _animator;
    private int dieHash = Animator.StringToHash("Die");
    private int takeDamageHash = Animator.StringToHash("TakeDamage");
    
    //AI
    private Transform _playerTransform;
    private NavMeshAgent _monsterNavMesh;
    private float _checkRate = 0.01f;
    private float _nextCheck;
    

    private SnowMonsterState _state = SnowMonsterState.Alive;
    
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
        if (Time.time > _nextCheck)
        {
            _nextCheck = Time.time + _checkRate;
            FollowPlayer();
        }
    }

    private void FollowPlayer()
    {
        //Todo: Add distance check etc.
        _monsterNavMesh.transform.LookAt(_playerTransform);
        _monsterNavMesh.destination = _playerTransform.position;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        _animator.SetTrigger(takeDamageHash);
        if (health <= 0 && _state == SnowMonsterState.Alive)
        {
            Die();
        }
        
    }

    private void Die()
    {
        _state = SnowMonsterState.Dead;
        _animator.SetTrigger(dieHash);
        Destroy(gameObject, 5);
    }
}
