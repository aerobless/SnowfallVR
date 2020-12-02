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
    private float _stopTakingDamage;

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
        if (Time.time > _nextCheck && MonsterIsAlive() && MonsterIsNotTakingDamage())
        {
            _nextCheck = Time.time + _checkRate;
            FollowPlayer();
        }

        if (Time.time > _stopTakingDamage && MonsterIsAlive())
        {
            //TODO: make more formal state machine.. this will probably get confusing fast
            //Expires state after 2seconds if no further damage is taken
            _state = SnowMonsterState.Idle;
            _animator.ResetTrigger(_takeDamageHash);
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
    
    private bool MonsterIsNotTakingDamage()
    {
        return _state != SnowMonsterState.TakingDamage;
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
        _state = SnowMonsterState.TakingDamage;
        _monsterNavMesh.SetDestination(gameObject.transform.position); //Set current position as destination to stop moving
        _animator.ResetTrigger(_runForwardInPlaceHash);
        _animator.SetTrigger(_takeDamageHash);
        _stopTakingDamage = Time.time + 1f; //Expires after 2seconds if no further damage is taken
        health -= damage;
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