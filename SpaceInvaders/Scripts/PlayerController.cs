using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Bullet velocity
    public float bulletSpeed = 10;

    // Left hand gun setup
    public GameObject leftHandShootOutput;
    public GameObject leftHandBulletPrefab;
    public GrabDetector leftHandGunGrabDetector;

    // Right hand gun setup
    public GameObject rightHandShootOutput;
    public GameObject rightHandBulletPrefab;
    public GrabDetector rightHandGunGrabDetector;


    // Reference to the AudioSource component
    public AudioManager am;

    [Range(0.01f, 1f)]
    public float speedH = 1.0f;
    [Range(0.01f, 1f)]
    public float speedV = 1.0f;

    GameManager gm;
    GrabDetector gd;


    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindObjectOfType<GameManager>();
        gd = GameObject.FindObjectOfType<GrabDetector>();
        am = GameObject.FindObjectOfType<AudioManager>();
        
    }

    void Update()
    {
        GunActionManager();
    }

    // Fire function for left hand
    void OnFireLeftHand()
    {
        // Spawn a new bullet
        GameObject newBullet = Instantiate(leftHandBulletPrefab);

        // Pass the game manager
        newBullet.GetComponent<BulletController>().gm = gm;

        // Pass the grab detector
        newBullet.GetComponent<BulletController>().gd = gd;

        // Position will be that of the left hand gun
        newBullet.transform.position = leftHandShootOutput.transform.position;

        // Get rigid body
        Rigidbody bulletRb = newBullet.GetComponent<Rigidbody>();

        // Let the bullet face forward when shot
        newBullet.transform.LookAt(leftHandShootOutput.transform.forward * 30f);

        // Give the bullet velocity
        bulletRb.velocity = leftHandShootOutput.transform.forward * bulletSpeed;
    }

    // Fire function for right hand
    void OnFireRightHand()
    {
        // Spawn a new bullet
        GameObject newBullet = Instantiate(rightHandBulletPrefab);

        // Pass the game manager
        newBullet.GetComponent<BulletController>().gm = gm;

        // Pass the grab detector
        newBullet.GetComponent<BulletController>().gd = gd;

        // Position will be that of the right hand gun
        newBullet.transform.position = rightHandShootOutput.transform.position;

        // Get rigid body
        Rigidbody bulletRb = newBullet.GetComponent<Rigidbody>();

        // Let the bullet face forward when shot
        newBullet.transform.LookAt(rightHandShootOutput.transform.forward * 30f);

        // Give the bullet velocity
        bulletRb.velocity = rightHandShootOutput.transform.forward * bulletSpeed;
    }

    // Managing gun actions for both hands
    void GunActionManager()
    {
        // Check if the right hand is grabbed and shooting
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger) && rightHandGunGrabDetector.isGrabbedByRightHand)
        {
            // Play shooting sound and fire right hand gun
            am.playSFX(am.gun1shoot);
            OnFireRightHand();
        }

        // Check if the left hand is grabbed and shooting
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) && leftHandGunGrabDetector.isGrabbedByLeftHand)
        {
            // Play shooting sound and fire left hand gun
            am.playSFX(am.gun2shoot);
            OnFireLeftHand();
        }
    }

    
}
