using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowballGunTarget : MonoBehaviour
{

    public int health = 100;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject, 0);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }
}
