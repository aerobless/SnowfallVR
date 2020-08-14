using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burnable : MonoBehaviour
{
    public Boolean isBurning;

    // Start is called before the first frame update
    void Start()
    {
        if (!isBurning)
        {
            GetComponent<ParticleSystem>().Stop();
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        Burnable burnableGameObject = other.gameObject.GetComponent<Burnable>();
        if (burnableGameObject != null && burnableGameObject.isBurning)
        {
            print("Object has been triggered to start burning");
            isBurning = true;
            GetComponent<ParticleSystem>().Play();
        }
    }
}
