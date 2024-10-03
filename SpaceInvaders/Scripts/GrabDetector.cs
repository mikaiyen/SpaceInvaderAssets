using Oculus.Interaction;
using UnityEngine;

public class GrabDetector : MonoBehaviour
{
    public bool isGrabbed = false;
    public bool isGrabbedByRightHand = false;
    public bool isGrabbedByLeftHand = false;
    public IInteractable[] interactable;
    void Start()
    {
        interactable = GetComponentsInChildren<IInteractable>();    
    }
    void Update()
    {
        isGrabbedByRightHand = OVRInput.Get(OVRInput.Button.SecondaryHandTrigger);
        isGrabbedByLeftHand = OVRInput.Get(OVRInput.Button.PrimaryHandTrigger);
        isGrabbed = isGrabbedByRightHand || isGrabbedByLeftHand;

        for(int i = 0; i < interactable.Length; i++)
        {
            if(interactable[i].State == InteractableState.Select)
            {
                isGrabbed = true;
                break;
            }
            if(i == interactable.Length - 1)
            {
                isGrabbed = false; 
            }
        }
    }

    public OVRInput.Controller GetGrabbedController()
    {
        if (isGrabbedByLeftHand)
        {
            Debug.Log("Left hand is grabbing the gun.");
            return OVRInput.Controller.LTouch;
        }
        else if (isGrabbedByRightHand)
        {
            Debug.Log("Right hand is grabbing the gun.");
            return OVRInput.Controller.RTouch;
        }
        Debug.Log("No hand.");
        return OVRInput.Controller.None;
    }

    public bool IsBothGrab()
    {
        if (isGrabbedByLeftHand&&isGrabbedByRightHand)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
