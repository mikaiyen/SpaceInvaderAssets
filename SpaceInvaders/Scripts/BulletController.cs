using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

            // 開始震動控制器，頻率和強度可以自行調整
            if(gd.IsBothGrab()==true)
            {
                OVRInput.SetControllerVibration(0.2f, 0.2f, OVRInput.Controller.LTouch);
                OVRInput.SetControllerVibration(0.2f, 0.2f, OVRInput.Controller.RTouch);
                // 在 0.5 秒後停止震動
                Invoke("StopBothVibration", 0.2f);
            }
            else
            {
                grabbedController=gd.GetGrabbedController();
                OVRInput.SetControllerVibration(0.2f, 0.2f, grabbedController);
                // 在 0.5 秒後停止震動
                Invoke("StopVibration", 0.2f);
            }
            Destroy(gameObject,0.2f);
        }
        // check if we hit Boss
        else if(other.CompareTag("Boss"))
        {
            other.gameObject.GetComponent<BossController>().losehealth();
            if(other.gameObject.GetComponent<BossController>().health==0){
                other.gameObject.GetComponent<BossController>().KillBoss();
            }

            // 開始震動控制器，頻率和強度可以自行調整
            if(gd.IsBothGrab()==true)
            {
                OVRInput.SetControllerVibration(0.2f, 0.2f, OVRInput.Controller.LTouch);
                OVRInput.SetControllerVibration(0.2f, 0.2f, OVRInput.Controller.RTouch);
                // 在 0.5 秒後停止震動
                Invoke("StopBothVibration", 0.2f);
            }
            else
            {
                grabbedController=gd.GetGrabbedController();
                OVRInput.SetControllerVibration(0.2f, 0.2f, grabbedController);
                // 在 0.5 秒後停止震動
                Invoke("StopVibration", 0.2f);
            }
            Destroy(gameObject,0.2f);
        }
        // check if we hit the graffiti
        else if(other.CompareTag("Graffiti")) {   
            if(gm.nowstate=="NotStarted"){
                gm.InitGame();
                Destroy(gameObject);
            }
            else if(gm.nowstate=="WonGame"){
                Destroy(gameObject);
                //load next level
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
            }
            else if(gm.nowstate=="GameOver"){
                Destroy(gameObject);
                //load next level
                SceneManager.LoadScene(1);
            }
            
            
        }
    }

    public void StopVibration()
    {
        // 停止右手控制器的震動
        OVRInput.SetControllerVibration(0, 0, grabbedController);
    }

    public void StopBothVibration()
    {
        // 停止控制器的震動
        OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.LTouch);
        OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
    }
        
}
