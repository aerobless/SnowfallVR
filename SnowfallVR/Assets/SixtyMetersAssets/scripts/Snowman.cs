using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowman : MonoBehaviour
{
    public int health;

    // Start is called before the first frame update
    void Start()
    {
        if (health <= 0)
        {
            health = 100;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Snowball snowball = other.gameObject.GetComponent<Snowball>();
        if (snowball != null)
        {
            int damage = 20; //TODO: read damage from snowball, determine damage by throw speed?
            health -= damage;
            Debug.Log($"Snowman was attacked for {damage} damage. Remaining health: {health}");
        }
    }
}