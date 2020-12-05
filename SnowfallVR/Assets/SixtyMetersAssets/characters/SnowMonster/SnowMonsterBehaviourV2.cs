using SixtyMetersAssets.characters.player;
using SixtyMetersAssets.Items;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace SixtyMetersAssets.characters.SnowMonster
{
    public class SnowMonsterBehaviourV2 : MonoBehaviour, GunTarget
    {
        public int health = 100;
        public float detectPlayerRadius = 10f;
        
        // Sounds
        public AudioClip painSound;
        public AudioClip deathSound;
        public AudioClip meleeAttackSound;

        private Animator _animator;
        private readonly int _dieHash = Animator.StringToHash("Die");
        private readonly int _takeDamageHash = Animator.StringToHash("Take Damage");
        private readonly int _runForwardInPlaceHash = Animator.StringToHash("Run Forward");
        private readonly int _slapAttackRight = Animator.StringToHash("Slap Attack Right");

        private AudioSource _audioSource;

        // AI
        private Transform _playerTransform;
        private PlayerBehaviour _player;
        private NavMeshAgent _monsterNavMesh;

        // Statemachine
        private SnowMonsterState _currentState = SnowMonsterState.Idle;
        private SnowMonsterState _nextState = SnowMonsterState.Idle;
        private float _nextCheck;

        // Start is called before the first frame update
        void Start()
        {
            _animator = GetComponent<Animator>();
            _audioSource = GetComponent<AudioSource>();
            if (GameObject.FindGameObjectWithTag("Player").activeInHierarchy)
            {
                _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
                _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
            }

            _monsterNavMesh = gameObject.GetComponent<NavMeshAgent>();
            _nextCheck = Time.time;
        }

        // Update is called once per frame
        void Update()
        {
            if (Time.time > _nextCheck)
            {
                if (_currentState != _nextState)
                {
                    if (_currentState == SnowMonsterState.Dead)
                    {
                        // Prevents reviving after interrupts from outside sources
                        _nextState = SnowMonsterState.Dead;
                    }

                    if (_currentState == SnowMonsterState.MovingToPlayer)
                    {
                        //Stop moving
                        _monsterNavMesh.SetDestination(gameObject.transform.position);
                    }

                    ResetAllAnimations();

                    if (_currentState != SnowMonsterState.Dead && _nextState == SnowMonsterState.Dead)
                    {
                        Die();
                    }

                    //Finally set states equal
                    Debug.Log("State transition from " + _currentState + " to " + _nextState);
                    _currentState = _nextState;
                }
                else
                {
                    switch (_currentState)
                    {
                        case SnowMonsterState.Idle:
                            ExecuteIdleState();
                            return;
                        case SnowMonsterState.MovingToPlayer:
                            ExecuteMovingToPlayerState();
                            return;
                        case SnowMonsterState.Attacking:
                            ExecuteAttackingState();
                            return;
                        case SnowMonsterState.TakingDamage:
                            ExecuteTakingDamageState();
                            return;
                        case SnowMonsterState.Dead:
                            return; // A dead monster is dead
                    }
                }
            }

            if (health <= 0)
            {
                _nextState = SnowMonsterState.Dead;
                InterruptActiveState();
            }
        }


        private void ExecuteIdleState()
        {
            if (PlayerIsInRange())
            {
                _nextState = SnowMonsterState.MovingToPlayer;
            }

            NextCheckInSeconds(0.1f);
        }

        private void ExecuteMovingToPlayerState()
        {
            _animator.SetTrigger(_runForwardInPlaceHash);
            _monsterNavMesh.transform.LookAt(_playerTransform);
            _monsterNavMesh.SetDestination(_playerTransform.position);

            //Detect state transitions
            if (!PlayerIsInRange())
            {
                _nextState = SnowMonsterState.Idle;
            }

            if (PlayerIsCloseEnoughToAttack())
            {
                _nextState = SnowMonsterState.Attacking;
            }

            //Next check should be asap
        }

        private void ExecuteAttackingState()
        {
            _monsterNavMesh.transform.LookAt(_playerTransform);
            _animator.SetTrigger(_slapAttackRight);
            _player.TakeDamage(10);
            _audioSource.PlayOneShot(meleeAttackSound);

            if (!PlayerIsCloseEnoughToAttack())
            {
                _nextState = SnowMonsterState.Idle;
            }

            NextCheckInSeconds(1f);
        }

        private void ExecuteTakingDamageState()
        {
            _monsterNavMesh.SetDestination(gameObject.transform.position); //Stop moving
            _animator.SetTrigger(_takeDamageHash);
            _audioSource.PlayOneShot(painSound);
            _nextState = SnowMonsterState.Idle;

            NextCheckInSeconds(1f);
        }

        private void NextCheckInSeconds(float seconds)
        {
            _nextCheck = Time.time + seconds;
        }

        private bool PlayerIsInRange()
        {
            float distanceToPlayer = Vector3.Distance(_playerTransform.position, transform.position);
            return distanceToPlayer <= detectPlayerRadius;
        }

        private bool PlayerIsCloseEnoughToAttack()
        {
            float distanceToPlayer = Vector3.Distance(_playerTransform.position, transform.position);
            return distanceToPlayer <= _monsterNavMesh.stoppingDistance;
        }

        private void OnTriggerEnter(Collider other)
        {
            Snowball snowball = other.gameObject.GetComponent<Snowball>();
            if (snowball != null)
            {
                TakeDamage(snowball.damage);
            }
        }

        public void TakeDamage(int damage)
        {
            _nextState = SnowMonsterState.TakingDamage;
            InterruptActiveState();

            health -= damage;
        }

        private Vector3 GetPlayerDestinationBendSafe()
        {
            Vector3 destination = _playerTransform.position;
            destination.y = 1f; //Fixes bending backwards issue with smaller enemies
            return destination;
        }

        private void InterruptActiveState()
        {
            _nextCheck = Time.time;
        }

        private void Die()
        {
            _animator.SetTrigger(_dieHash);
            _audioSource.PlayOneShot(deathSound);
            Destroy(gameObject, 5);
        }

        private void ResetAllAnimations()
        {
            _animator.ResetTrigger(_takeDamageHash);
            _animator.ResetTrigger(_runForwardInPlaceHash);
            _animator.ResetTrigger(_slapAttackRight);
        }
    }
}