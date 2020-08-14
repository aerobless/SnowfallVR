using System;
using System.Collections;
using UnityEngine;

public class Burnable : MonoBehaviour
{
    public Boolean isBurning;
    public Boolean burnsForever;

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
        if (burnableGameObject != null && burnableGameObject.isBurning && burnsForever == false)
        {
            print("Object has been triggered to start burning");
            isBurning = true;
            GetComponent<ParticleSystem>().Play();
            Destroy(gameObject, 10);
        }
    }
}
