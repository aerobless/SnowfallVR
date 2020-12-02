using System.Collections;
using System.Collections.Generic;
using SixtyMetersAssets.Items;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour, GunTarget
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
            Debug.Log("the player has died");
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }
}