using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowball : MonoBehaviour
{

    public int damage = 10;
    public GameObject impactEffect;
    public GameObject instance;

    private void OnTriggerEnter(Collider other)
    {
        GameObject instantiatedImpact = Instantiate(impactEffect, transform.position, Quaternion.LookRotation(transform.position.normalized));
        Destroy(instantiatedImpact, 2f);
        Destroy(instance);   
    }
}
