﻿using Photon.Pun;
using UnityEngine;

namespace SixtyMetersAssets.Characters.SnowMonster
{
    public class MonsterSpawner : MonoBehaviour
    {
        public GameObject monsterToSpawn;
        
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(transform.position, transform.localScale);
        }

        public void Spawn()
        {
            PhotonNetwork.Instantiate("Characters/"+monsterToSpawn.name, transform.position, transform.rotation);
        }
    }
}
