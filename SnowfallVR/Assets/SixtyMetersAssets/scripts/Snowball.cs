using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowball : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter(Collider other)
    {
        Snowman snowman = other.gameObject.GetComponent<Snowman>();
        if (snowman != null)
        {
            //TODO: destroy snowball only on impact with enough force
            // Destroy snowball on impact with snowman
            Destroy(gameObject, 0);
        }
    }
}
