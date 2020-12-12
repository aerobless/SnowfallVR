using System.Collections;
using Photon.Pun;
using SixtyMetersAssets.Items.Snowball;
using UnityEngine;

public class SnowballGun : MonoBehaviour
{
    
    public OVRGrabbable gun;
    public AudioSource audioSource;
    public AudioClip shotSound;
    public ParticleSystem muzzleFlash;
    public GameObject projectile;
    public GameObject projectileSpawnPoint;

    private float projectileSpeed = 20f;

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
        //muzzleFlash.Play();
        ParticleSystem instantiatedMuzzleFlash = Instantiate(muzzleFlash, projectileSpawnPoint.transform.position, transform.rotation);
        instantiatedMuzzleFlash.Play();
        Destroy(instantiatedMuzzleFlash, 1f);
        OVRInput.SetControllerVibration(0.5f, 0.5f, OVRInput.Controller.RTouch);
        StartCoroutine(EndVibration(0.05f));
        
        GameObject instantiatedProjectile = PhotonNetwork.Instantiate("items/Snowball", projectileSpawnPoint.transform.position , transform.rotation);
        instantiatedProjectile.GetComponent<Rigidbody>().velocity = gun.transform.forward.normalized * projectileSpeed;

        instantiatedProjectile.GetComponent<Snowball>().instance = instantiatedProjectile;
        
        Destroy(instantiatedProjectile, 3f); // Destroy the projectile after max 3seconds even if it doesn't make contact
    }

    private static IEnumerator EndVibration(float time)
    {
        yield return new WaitForSeconds(time);
        // Code to execute after the delay
        OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
    }
}