using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class MeasureDistance : MonoBehaviour {
    private float coveredDistance;
    private Vector3 prevPos;
    private VRTK_InteractGrab interactGrab;
    private VRTK_ControllerEvents controllerEvents;
    private ControllerInteractionEventArgs eventArg;

    // Use this for initialization
    void Start () {
        coveredDistance = 0f;
        prevPos = transform.position;
        interactGrab = GetComponent<VRTK_InteractGrab>();
        controllerEvents = GetComponent<VRTK_ControllerEvents>();
	}
	
	// Update is called once per frame
	void Update () {
        if (controllerEvents.touchpadPressed)
        {
            print("Last distance: " + coveredDistance);
            coveredDistance = 0f;
            prevPos = transform.position;
        }

        if (interactGrab.IsGrabButtonPressed())
        {
            coveredDistance += Vector3.Distance(transform.position, prevPos);
            prevPos = transform.position;
        }
	}
    
    
}
