using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class swingarm : MonoBehaviour
{
    // gameobject
    public GameObject LeftHand;
    public GameObject RightHand;
    public GameObject CenterEyeCamera;
    public GameObject ForwardDirection;

    // Vector3 position
    private Vector3 LeftHand_PreviousFramePosition;
    private Vector3 RightHand_PreviousFramePosition;
    private Vector3 Player_PreviousFramePosition;
    private Vector3 LeftHand_ThisFramePosition;
    private Vector3 RightHand_ThisFramePosition;
    private Vector3 Player_ThisFramePosition;

    // Speed
    public float speed=70;
    private float handspeed;


    // Start is called before the first frame update
    void Start()
    {
        //set org position
        Player_PreviousFramePosition=transform.position;
        LeftHand_PreviousFramePosition=LeftHand.transform.position;
        RightHand_PreviousFramePosition=RightHand.transform.position;        
    }

    // Update is called once per frame
    void Update()
    {
        // get forward dir
        float yRotation=CenterEyeCamera.transform.eulerAngles.y;
        ForwardDirection.transform.eulerAngles = new Vector3(0, yRotation, 0);

        // get curr position of hands
        LeftHand_ThisFramePosition = LeftHand.transform.position;
        RightHand_ThisFramePosition = RightHand.transform.position;
        //position of player
        Player_ThisFramePosition=transform.position;

        //get distance 2 hands and player has move since last frame
        var playerDistancedMoved = Vector3.Distance(Player_ThisFramePosition,Player_PreviousFramePosition);
        var leftHandDistanceMoved = Vector3.Distance(LeftHand_PreviousFramePosition,LeftHand_ThisFramePosition);
        var rightHandDistanceMoved = Vector3.Distance(RightHand_PreviousFramePosition,RightHand_ThisFramePosition);

        // Add them up
        handspeed = ((leftHandDistanceMoved-playerDistancedMoved)+(rightHandDistanceMoved-playerDistancedMoved));

        if(Time.timeSinceLevelLoad>1f){
            transform.position+=ForwardDirection.transform.forward*handspeed*speed*Time.deltaTime;
        }

        // set previous position
        LeftHand_PreviousFramePosition=LeftHand_ThisFramePosition;
        RightHand_PreviousFramePosition=RightHand_ThisFramePosition;
        Player_PreviousFramePosition=Player_ThisFramePosition;

    }
}
