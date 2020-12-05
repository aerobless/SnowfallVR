using SixtyMetersAssets.Items;
using SixtyMetersAssets.Items.Snowball;
using UnityEngine;

namespace SixtyMetersAssets.Characters.Player
{
    public class PlayerBehaviour : MonoBehaviour, GunTarget
    {
        public int health = 100;
        public GameObject spawnPoint;

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            if (health <= 0)
            {
                Die();
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

        public void TakeDamage(int damage)
        {
            health -= damage;
            Debug.Log("Player takes damage.. health: " + health);
        }

        private void Die()
        {
            Debug.Log("the player has died");
            RespawnPlayerWithFullHealth();
        }

        private void RespawnPlayerWithFullHealth()
        {
            health = 100;
            enabled = false;
            transform.position = spawnPoint.transform.position;
            enabled = true;
        }
    }
}