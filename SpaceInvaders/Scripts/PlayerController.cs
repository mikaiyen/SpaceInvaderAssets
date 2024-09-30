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

    [Range(0.01f, 1f)]
    public float speedH = 1.0f;
    [Range(0.01f, 1f)]
    public float speedV = 1.0f;

    GameManager gm;


    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindObjectOfType<GameManager>();
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

        // position will be that of the gun
        newBullet.transform.position = ShootOutput.transform.position;

        // get rigid body
        Rigidbody bulletRb = newBullet.GetComponent<Rigidbody>();

        // let the bullet face to the forward when shoot
        newBullet.transform.LookAt(ShootOutput.transform.right * 30f);

        // give the bullet velocity
        bulletRb.velocity = ShootOutput.transform.forward * bulletSpeed;
    }

    void GunActionManager()
    {
        // shoot gun
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger) && gunGrabDetector.isGrabbed)
        {
            OnFire();
        }
    }

    
}
