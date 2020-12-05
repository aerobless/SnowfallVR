using SixtyMetersAssets.Characters.Player;
using SixtyMetersAssets.Items;
using SixtyMetersAssets.Items.Snowball;
using UnityEngine;
using UnityEngine.AI;

namespace SixtyMetersAssets.Characters.SnowMonster
{
    public class SnowMonsterBehaviour : MonoBehaviour, GunTarget
    {
        public int health = 100;
        public float lookRadius = 10f;

        private Animator _animator;
        private readonly int _dieHash = Animator.StringToHash("Die");
        private readonly int _takeDamageHash = Animator.StringToHash("Take Damage");
        private readonly int _runForwardInPlaceHash = Animator.StringToHash("Run Forward");
        private readonly int _slapAttackRight = Animator.StringToHash("Slap Attack Right");

        //AI
        private Transform _playerTransform;
        private PlayerBehaviour _player;
        private NavMeshAgent _monsterNavMesh;
        private float _checkRate = 0.01f;

        private float _nextCheck;
        private float _stopTakingDamage;
        private float _nextAttack;

        private SnowMonsterState _state = SnowMonsterState.Idle;

        // Start is called before the first frame update
        void Start()
        {
            _animator = GetComponent<Animator>();
            if (GameObject.FindGameObjectWithTag("Player").activeInHierarchy)
            {
                _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
                _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
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
                _animator.ResetTrigger(_takeDamageHash);
                FollowPlayer();
            }

            if (Time.time > _nextAttack && MonsterIsAttacking())
            {
                Attack();
            }
        }

        private bool MonsterIsAttacking()
        {
            return _state == SnowMonsterState.Attacking;
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
    
        private bool MonsterIsIdle()
        {
            return (_state != SnowMonsterState.Attacking && _state 
                != SnowMonsterState.Dead && _state 
                != SnowMonsterState.TakingDamage) || _state == SnowMonsterState.Idle;
        }

        private void FollowPlayer()
        {
            float distanceToPlayer = Vector3.Distance(_playerTransform.position, transform.position);
            if (distanceToPlayer <= lookRadius)
            {
                var destination = GetPlayerDestinationBendSafe();
                _monsterNavMesh.transform.LookAt(destination);
                _monsterNavMesh.SetDestination(destination);
                _animator.SetTrigger(_runForwardInPlaceHash);

                if (distanceToPlayer <= _monsterNavMesh.stoppingDistance && MonsterIsIdle())
                {
                    Debug.Log(_state);
                    StartAttacking();
                }
            }
            else
            {
                _animator.ResetTrigger(_runForwardInPlaceHash);
            }
        }

        private Vector3 GetPlayerDestinationBendSafe()
        {
            Vector3 destination = _playerTransform.position;
            destination.y = 1f; //Fixes bending backwards issue with smaller enemies
            return destination;
        }

        public void TakeDamage(int damage)
        {
            _state = SnowMonsterState.TakingDamage;
            _monsterNavMesh.SetDestination(gameObject.transform
                .position); //Set current position as destination to stop moving
            _animator.ResetTrigger(_runForwardInPlaceHash);
            _animator.SetTrigger(_takeDamageHash);
            _stopTakingDamage = Time.time + 1f; //Expires after 2seconds if no further damage is taken
            health -= damage;
            if (health <= 0 && MonsterIsAlive())
            {
                Die();
            }
        }

        private void StartAttacking()
        {
            Debug.Log("Start Attacking");
            _state = SnowMonsterState.Attacking;
            _monsterNavMesh.transform.LookAt(GetPlayerDestinationBendSafe());
            _animator.ResetTrigger(_runForwardInPlaceHash);
            _nextAttack = Time.time;
        }

        private void Attack()
        {
            float distanceToPlayer = Vector3.Distance(_playerTransform.position, transform.position);
            if (distanceToPlayer < _monsterNavMesh.stoppingDistance)
            {
                Debug.Log("Attack");
                _monsterNavMesh.transform.LookAt(GetPlayerDestinationBendSafe());
                _animator.ResetTrigger(_runForwardInPlaceHash);
                _animator.SetTrigger(_slapAttackRight);
                _player.TakeDamage(10);
                _nextAttack = Time.time + 1f;   
            }
            else
            {
                _animator.ResetTrigger(_slapAttackRight);
                _state = SnowMonsterState.Idle;
            }
        }

        private void Die()
        {
            _state = SnowMonsterState.Dead;
            _animator.SetTrigger(_dieHash);
            Destroy(gameObject, 5);
        }
    }
}