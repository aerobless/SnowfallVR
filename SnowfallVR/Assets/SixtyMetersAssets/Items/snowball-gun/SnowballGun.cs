using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SnowballGun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;

    public OVRGrabbable gun;
    public AudioSource audioSource;
    public AudioClip shotSound;
    
    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger) || Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        audioSource.PlayOneShot(shotSound, 1f);
        RaycastHit hit;
        Debug.DrawRay(gun.transform.position, gun.transform.forward,  Color.red, 5);
        if (Physics.Raycast(gun.transform.position, gun.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);
        }
    }
}