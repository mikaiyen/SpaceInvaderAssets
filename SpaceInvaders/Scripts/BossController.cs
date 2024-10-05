using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    // movement range
    public float rangeH = 5;
    public float rangeV = 1;
   
    // speed
    public float speed = 2;

    public int health = 5;

    // direction
    int direction = 1;

    // accumulated movement
    float accMovement = 0;

     // available states
    enum State { MovingHorizontally, MovingVertically, Dead};
    public Material deadmaterial;
    public Material hurtmaterial;
    public Material normalmaterial;
    
    // keep track of the current state
    State currState;

    // Game Manager
    GameManager gm;

    // Enemy Manager
    BossManager bm;

    AudioManager am;


    // Start is called before the first frame update
    void Start()
    {
        // initial state
        currState = State.MovingHorizontally;

        // game manager
        gm = GameObject.FindObjectOfType<GameManager>();

        // log error if it wasn't found
        if (gm == null)
        {
            Debug.LogError("there needs to be an GameManager in the scene");
        }

        // enemy manager
        bm = GameObject.FindObjectOfType<BossManager>();

        // log error if it wasn't found
        if (bm == null)
        {
            Debug.LogError("there needs to be an EnemyManager in the scene");
        }

        am = GameObject.FindObjectOfType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // nothing happens if the enemy is dead
        if (currState == State.Dead) return;

        // calculate movement  v = d / t --> d = v * t
        float movement = speed * Time.deltaTime;

        // update accumulate movement
        accMovement += movement;

        // are we moving horizontally?
        if (currState == State.MovingHorizontally)
        {
            // if yes, then transition to moving vertically
            if(accMovement >= rangeH)
            {
                // transition to moving vertically
                currState = State.MovingVertically;

                // reverse direction (for horizontal movement)
                direction *= -1;

                // reset acc movement
                accMovement = 0;
            }
            // if not, move the invader horizontally
            else
            {
                transform.position += transform.forward * movement * direction;
            }
        }
        // this is, if we are moving vertically
        else
        {
            // if yes, then transition to moving horizontally
            if (accMovement >= rangeV)
            {
                // transition to moving horiz
                currState = State.MovingHorizontally;

                // reset acc movement
                accMovement = 0;
            }
            // if not, move the invader vertically
            else
            {
                transform.position += Vector3.down * movement;
            }
        }
    }

    public void KillBoss()
    {
        // nothing will happen if already dead
        if (currState == State.Dead) return;

        // set the state to dead
        currState = State.Dead;

        //[implement your own effect here]
        gameObject.GetComponentInChildren<Renderer>().sharedMaterial = deadmaterial;
        am.playSFX(am.enemydeath);

        //[Example]
        Invoke("enemyfade", 0.2f);
        
        //[End of Example]

        // decrease number of enemies
        bm.numBosses--;
        
        // check winning condition
        gm.HandleBossDead();
    }

    void OnTriggerEnter(Collider other)
    {
        // nothing will happen if already dead
        if (currState == State.Dead) return;

        //check if the enemy hit the player
        if (other.CompareTag("Player Body"))
        {
            gm.GameOver();
        }

        //check if the enemy reached the floor
        else if (other.CompareTag("Ground"))
        {
            gm.GameOver();
        }
    }

    public void enemyfade(){
        Destroy(gameObject);
    }

    public void losehealth(){
        health--;
        gameObject.GetComponentInChildren<Renderer>().sharedMaterial = hurtmaterial;
        am.playSFX(am.enemydeath);
        Invoke("enemyhurt", 0.2f);

    }

    public void enemyhurt(){
        gameObject.GetComponentInChildren<Renderer>().sharedMaterial = normalmaterial;
    }
}
