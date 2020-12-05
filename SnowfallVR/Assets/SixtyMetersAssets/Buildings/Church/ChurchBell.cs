using System.Collections;
using SixtyMetersAssets.Characters.SnowMonster;
using SixtyMetersAssets.Items.Snowball;
using UnityEngine;

namespace SixtyMetersAssets.Buildings.Church
{
    public class ChurchBell : MonoBehaviour
    {
        public AudioClip churchBellSound;

        private AudioSource _audioSource;
        private bool _bellsAreRinging;
        private MonsterSpawner[] _monsterSpawners;

        // Start is called before the first frame update
        void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            _monsterSpawners = FindObjectsOfType<MonsterSpawner>();
        }

        // Update is called once per frame
        void Update()
        {
        }

        private void OnTriggerEnter(Collider other)
        {
            Snowball snowball = other.gameObject.GetComponent<Snowball>();
            if (snowball != null && _bellsAreRinging == false)
            {
                RingChurchBell();
            }
        }

        private void RingChurchBell()
        {
            _audioSource.PlayOneShot(churchBellSound);
            _bellsAreRinging = true;
            StartCoroutine(StopBellFromRinging(7f));
            SpawnMonsters();
        }

        private IEnumerator StopBellFromRinging(float time)
        {
            yield return new WaitForSeconds(time);
            // Code to execute after the delay
            _bellsAreRinging = false;
        }

        private void SpawnMonsters()
        {
            foreach (var monsterSpawner in _monsterSpawners)
            {
                monsterSpawner.Spawn();
            }
        }
    }
}