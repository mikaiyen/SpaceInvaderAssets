using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float m_speed = 30.0f;
    public Transform m_Tip = null;

    public GameObject player;

    private Rigidbody m_Rigidbody = null;
    private bool m_IsStopped = true;
    private Vector3 m_LastPosition = Vector3.zero;

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
        if(Physics.Linecast(m_LastPosition, m_Tip.position)){
            Stop();
        }

        //store position
        m_LastPosition=m_Tip.position;

    }

    private void Stop(){
        m_IsStopped=true;

        m_Rigidbody.isKinematic=true;
        m_Rigidbody.useGravity = false;

    }

    public void Fire(float pullValue){
        m_IsStopped=false;
        transform.parent = null;


        m_Rigidbody.isKinematic = false;
        m_Rigidbody.useGravity = true;
        m_Rigidbody.AddForce(transform.forward*(pullValue*m_speed), ForceMode.Impulse);

        Destroy(gameObject, 10.0f);
    }

}
