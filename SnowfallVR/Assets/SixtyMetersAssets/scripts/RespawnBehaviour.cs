using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnBehaviour : MonoBehaviour
{
    public Transform spawnPoint;
    public float minHeightForDeath;
    public OVRPlayerController player;
    
    void Start()
    {
    }
    
    void Update()
    {
        if (player.transform.position.y < minHeightForDeath)
        {
            player.enabled = false;
            transform.position = spawnPoint.position;
            player.enabled = true;
            Debug.Log("Player respawned after falling!"+player.transform.position);
        }
    }
}
