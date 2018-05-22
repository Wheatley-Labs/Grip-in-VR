using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour {

    static public int currentInteractionMode;
    
    public void SetMode()
    {
        GameObject[] grabbables;

        grabbables = GameObject.FindGameObjectsWithTag("Grabbable");

        foreach (GameObject obj in grabbables)
        {
            //check for current components
            //adjust according to current mode
        }
    }
}
