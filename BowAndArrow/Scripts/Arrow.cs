using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float m_speed = 30.0f;
    public Transform m_Tip = null;


    private Rigidbody m_Rigidbody = null;
    private bool m_IsStopped = true;
    private Vector3 m_LastPosition = Vector3.zero;
    private float teleportOffset = 1.0f; // Offset to avoid clipping through the ground

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
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
            if (hit.collider.gameObject.tag != "Player Body")
            {
                Debug.Log("Arrow hit: " + hit.collider.gameObject.name);
                Stop(hit.point);
            }
        }

        //store position
        m_LastPosition=m_Tip.position;

    }

    private void Stop(Vector3 hitPosition){
        m_IsStopped=true;

        m_Rigidbody.isKinematic=true;
        m_Rigidbody.useGravity = false;

        // Teleport the player to the arrow's landing position
        TeleportPlayer(hitPosition);

    }

    public void Fire(float pullValue){
        m_IsStopped=false;
        transform.parent = null;


        m_Rigidbody.isKinematic = false;
        m_Rigidbody.useGravity = true;
        
        m_Rigidbody.AddForce(transform.forward*(pullValue*m_speed), ForceMode.Impulse);

        Destroy(gameObject, 10.0f);
    }

    private void TeleportPlayer(Vector3 targetPosition)
    {
        // Find the player prefab using its tag
        GameObject player = GameObject.FindGameObjectWithTag("Player Body");

        if (player != null)
        {
            // Add an offset to the target position to avoid clipping into the ground
            Vector3 adjustedPosition = new Vector3(targetPosition.x, targetPosition.y + teleportOffset, targetPosition.z);

            // Move the player to the adjusted position
            player.transform.position = adjustedPosition;
        }
        else
        {
            Debug.LogError("Player prefab not found!");
        }
    }

}
