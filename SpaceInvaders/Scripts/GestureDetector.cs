using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct Gesture
{
    public string name;
    public List<Vector3> fingerDatas;
    public UnityEvent onRecognized;
}

public class GestureDetector : MonoBehaviour
{
    public float threshold = 0.1f;
    public OVRSkeleton skeleton;
    public List<Gesture> gestures;
    public bool debugMode = true;
    public AudioManager am;
    public AudioClip[] vicnaSounds;
    public GameObject handEffect;
    public GameObject windEffect;


    
    private List<OVRBone> fingerBones;
    private Gesture previousGesture;
    private bool isMoving = false;
    public Transform playerTransform; // Player's transform (assign in inspector)
    public Rigidbody playerRig;

    public float moveSpeed = 2.0f; // Speed for forward movement
    public float heightChangeSpeed = 0.2f; // Speed for height adjustment
    public float maxHeightOffset = 1.5f; // Max height offset from the ground
    private Vector3 initialPosition; // Initial position of the player

    void Start()
    {
        StartCoroutine(WaitForBones());
    }

    IEnumerator WaitForBones()
    {
        while (skeleton.Bones.Count == 0)
        {
            yield return null; // Wait for bones to initialize
        }
        
        fingerBones = new List<OVRBone>(skeleton.Bones);
        previousGesture = new Gesture();
        initialPosition = playerTransform.position; // Save the initial position
        am = GameObject.FindObjectOfType<AudioManager>();
        
    }

    void Update()
    {
        if (debugMode && Input.GetKeyDown(KeyCode.Space))
        {
            Save();
        }

        Gesture currentGesture = Recognize();
        bool hasRecognized = !currentGesture.Equals(new Gesture());

        if (hasRecognized && !currentGesture.Equals(previousGesture))
        {
            if (currentGesture.name == "HoldFist")
            {
                StartMovingForward();
            }
            else
            {
                StopMovingForward();
            }

            if(currentGesture.name == "PalmTilt"){
                handEffect.SetActive(true); // 啟動特效
            }
            else{
                handEffect.SetActive(false); // 關閉特效
            }


            Debug.Log("New gesture: " + currentGesture.name);
            previousGesture = currentGesture;
            currentGesture.onRecognized.Invoke();
        }

        



        if (isMoving)
        {
            playerRig.useGravity=false;
            // Move the player forward
            playerTransform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            

            // Slowly increase height, but limit it to a certain range
            if (playerTransform.position.y < initialPosition.y + maxHeightOffset)
            {
                playerTransform.Translate(Vector3.up * heightChangeSpeed * Time.deltaTime);
            }
        }
        else
        {
            // Gradually bring the player back down to the original height
            playerRig.useGravity=true;
            
            // if (playerTransform.position.y > initialPosition.y)
            // {
            //     playerTransform.Translate(Vector3.down * heightChangeSpeed * Time.deltaTime);
            // }
        }
    }

    void StartMovingForward()
    {
        isMoving = true;
        // Randomly select an audio clip from the vicnaSounds array
        int randomIndex = Random.Range(0, vicnaSounds.Length); // Generates a random number between 0 and the length of the array
        
        windEffect.SetActive(true); // 啟動特效
        am.playSFX(vicnaSounds[randomIndex]);

        Debug.Log("Started moving forward and ascending.");
    }

    void StopMovingForward()
    {
        isMoving = false;
        windEffect.SetActive(false); // 啟動特效
        Debug.Log("Stopped moving forward and descending.");
    }

    void Save()
    {
        if (fingerBones == null || fingerBones.Count == 0)
        {
            Debug.LogError("Finger bones not initialized!");
            return;
        }

        Gesture g = new Gesture();
        g.name = "New Gesture";
        List<Vector3> data = new List<Vector3>();
        foreach (var bone in fingerBones)
        {
            if (bone.Transform != null)
            {
                data.Add(skeleton.transform.InverseTransformPoint(bone.Transform.position));
            }
        }

        g.fingerDatas = data;
        gestures.Add(g);
        Debug.Log("Gesture saved with " + g.fingerDatas.Count + " bone positions.");
    }

    Gesture Recognize()
    {
        Gesture currentGesture = new Gesture();
        float currentMin = Mathf.Infinity;

        foreach (var gesture in gestures)
        {
            float sumDistance = 0;
            bool isDiscarded = false;
            // Check if the bone's Transform is valid
            if (fingerBones == null)
            {
                //Debug.LogWarning("Bone transform is null for finger: ");
                isDiscarded = true;
                break; // Skip this gesture if the bone's transform is invalid
            }

            for (int i = 0; i < fingerBones.Count; i++)
            {                
                Vector3 currentData = skeleton.transform.InverseTransformPoint(fingerBones[i].Transform.position);
                float distance = Vector3.Distance(currentData, gesture.fingerDatas[i]);
                if (distance > threshold)
                {
                    isDiscarded = true;
                    break;
                }
                sumDistance += distance;
            }

            if (!isDiscarded && sumDistance < currentMin)
            {
                currentMin = sumDistance;
                currentGesture = gesture;
            }
        }
        return currentGesture;
    }

    // // Method to handle player movement when "holdFist" gesture is recognized
    // public void StartMovingForward()
    // {
    //     isMovingForward = true;
    // }

    // public void StopMovingForward()
    // {
    //     isMovingForward = false;
    // }

    // Function to move the player forward
    void MovePlayerForward()
    {
        playerTransform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }
}
