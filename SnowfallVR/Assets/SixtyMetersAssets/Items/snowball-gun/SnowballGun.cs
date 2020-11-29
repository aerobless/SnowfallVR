using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SixtyMetersAssets.Items;
using UnityEngine;

public class SnowballGun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public int damagePerHit = 10;

    public OVRGrabbable gun;
    public AudioSource audioSource;
    public AudioClip shotSound;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;

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
        muzzleFlash.Play();

        //TODO: remove debug statement later on
        Debug.DrawRay(gun.transform.position, gun.transform.forward, Color.red, 5);

        RaycastHit hit;
        if (Physics.Raycast(gun.transform.position, gun.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name); //TODO: remove, logs the name of the hit object
            GunTarget target = hit.transform.GetComponent<GunTarget>();
            if (target != null)
            {
                target.TakeDamage(damagePerHit);
            }
            
            GameObject instantiatedImpact = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(instantiatedImpact, 2f);
        }
    }
}