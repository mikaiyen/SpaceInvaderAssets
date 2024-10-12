using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float m_speed = 30.0f;
    public Transform m_Tip = null;
    public GameObject arrowEffect;  // 引用在箭矢中的特效
    public GameObject ritualEffect;
    public AudioManager am;


    private Rigidbody m_Rigidbody = null;
    private bool m_IsStopped = true;
    private Vector3 m_LastPosition = Vector3.zero;
    private float teleportOffset = 1.0f; // Offset to avoid clipping through the ground

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        am = GameObject.FindObjectOfType<AudioManager>();
    }

    private void FixedUpdate(){
        if(m_IsStopped){
            return;
        }

        //rotate
        m_Rigidbody.MoveRotation(Quaternion.LookRotation(m_Rigidbody.velocity, transform.up));

        //collision
        if(Physics.Linecast(m_LastPosition, m_Tip.position, out RaycastHit hit)){
            // Check if the arrow is hitting the player, if not, stop the arrow
            if (hit.collider.gameObject.tag != "PlayerBody")
            {
                Debug.Log("Arrow hit: " + hit.collider.gameObject.name);
                Stop(hit.point, hit.normal);
            }
        }

        //store position
        m_LastPosition=m_Tip.position;

    }

    private void Stop(Vector3 hitPosition, Vector3 hitNormal){
        m_IsStopped=true;

        m_Rigidbody.isKinematic=true;
        m_Rigidbody.useGravity = false;

        // Teleport the player to the arrow's landing position
        TeleportPlayer(hitPosition, hitNormal);

    }

    public void Fire(float pullValue){
        m_IsStopped=false;
        transform.parent = null;


        m_Rigidbody.isKinematic = false;
        m_Rigidbody.useGravity = true;
        
        m_Rigidbody.AddForce(transform.forward*(pullValue*m_speed), ForceMode.Impulse);
        am.playSFX(am.arrowShoot);

        // 啟動箭矢上的特效
        if (arrowEffect != null)
        {
            arrowEffect.SetActive(true); // 啟動特效
        }


        Destroy(gameObject, 10.0f);
    }

    private void TeleportPlayer(Vector3 targetPosition, Vector3 hitNormal)
    {
        // Find the player prefab using its tag
        GameObject player = GameObject.FindGameObjectWithTag("PlayerBody");

        if (player != null)
        {
            // Add an offset to the target position to avoid clipping into the ground
            Vector3 adjustedPosition = new Vector3(targetPosition.x, targetPosition.y + teleportOffset, targetPosition.z);

            // Move the player to the adjusted position
            player.transform.position = adjustedPosition;
            Debug.Log("Teleported");

            if (ritualEffect != null)
            {
                // 使特效跟地面平行
                ritualEffect.transform.position = adjustedPosition;

                // 根據地面法線方向來調整特效的旋轉，使其與地面平行
                Quaternion groundRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(player.transform.forward, hitNormal), hitNormal);
                ritualEffect.transform.rotation = groundRotation;
                ritualEffect.SetActive(true); // 啟動特效
                am.playSFX(am.teleportSound);
            }
        }
        else
        {
            Debug.LogError("Player prefab not found!");
        }
    }

}
