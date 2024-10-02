using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float maxDistance = 30;

    public GameManager gm;

    public GrabDetector gd;

    public OVRInput.Controller grabbedController;

    // Reference to the AudioSource component
    public AudioManager am;


    Vector3 initPos;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("BulletController script is attached to: " + gameObject.name);
        initPos = transform.position;
        am = GameObject.FindObjectOfType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // difference in all coordinate
        float diffX = Mathf.Abs(initPos.x - transform.position.x);
        float diffY = Mathf.Abs(initPos.y - transform.position.y);
        float diffZ = Mathf.Abs(initPos.z - transform.position.z);

        // destroy if it's too far away
        if(diffX >= maxDistance || diffY >= maxDistance || diffZ >= maxDistance)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // check if we hit an enemy
        if(other.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemyController>().KillEnemy();

            // 開始震動右手控制器，頻率和強度可以自行調整
            grabbedController=gd.GetGrabbedController();
            OVRInput.SetControllerVibration(0.5f, 0.8f, grabbedController);
            Debug.Log("hit with "+grabbedController);
            // 在 0.5 秒後停止震動
            Invoke("StopVibration", 0.5f);
            Destroy(gameObject,0.7f);
        }
        // check if we hit the graffiti
        else if(other.CompareTag("Graffiti")) {            
            gm.InitGame();
            Destroy(gameObject);
        }
    }

    public void StopVibration()
    {
        // 停止右手控制器的震動
        OVRInput.SetControllerVibration(0, 0, grabbedController);
        Debug.Log("invoke");
    }
        
}
