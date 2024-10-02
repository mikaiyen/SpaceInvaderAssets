using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Bullet velocity
    public float bulletSpeed = 10;

    // Gun shoot out position
    public GameObject ShootOutput;

    // bullet prefab
    public GameObject bulletPrefab;
    public GrabDetector gunGrabDetector;


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

    void OnFire()
    {
        // spawn a new bullet
        GameObject newBullet = Instantiate(bulletPrefab);

        // pass the game manager
        newBullet.GetComponent<BulletController>().gm = gm;

        // pass the grab detector
        newBullet.GetComponent<BulletController>().gd = gd;


        // position will be that of the gun
        newBullet.transform.position = ShootOutput.transform.position;

        // get rigid body
        Rigidbody bulletRb = newBullet.GetComponent<Rigidbody>();

        // let the bullet face to the forward when shoot
        newBullet.transform.LookAt(ShootOutput.transform.forward * 30f);

        // give the bullet velocity
        bulletRb.velocity = ShootOutput.transform.forward * bulletSpeed;

        
    }

    void GunActionManager()
    {
        // shoot gun
        if ((OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger) && gunGrabDetector.isGrabbedByRightHand) || 
        (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) && gunGrabDetector.isGrabbedByLeftHand))
        {
            // Play the shooting sound effect
            am.playSFX(am.shoot);
            OnFire();
        }
    }

    
}
