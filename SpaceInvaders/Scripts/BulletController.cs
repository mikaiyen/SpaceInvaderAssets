using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float maxDistance = 30;

    public GameManager gm;

    Vector3 initPos;
    // Start is called before the first frame update
    void Start()
    {
        initPos = transform.position;
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
            Destroy(gameObject);

            // 開始震動，並使用 Coroutine 來控制震動時間
            StartCoroutine(VibrateForDuration(0.5f, 0.8f, 0.5f, OVRInput.Controller.RTouch));
        }
        // check if we hit the graffiti
        else if(other.CompareTag("Graffiti")) {            
            gm.InitGame();
            Destroy(gameObject);
        }
    }

    IEnumerator VibrateForDuration(float frequency, float amplitude, float duration, OVRInput.Controller controller)
    {
        // 開始震動
        OVRInput.SetControllerVibration(frequency, amplitude, controller);

        // 等待震動時間
        yield return new WaitForSeconds(duration);

        // 停止震動
        OVRInput.SetControllerVibration(0, 0, controller);
    }
        
}
