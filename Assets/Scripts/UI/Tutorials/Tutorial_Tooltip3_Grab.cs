using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Tutorial_Tooltip3_Grab : MonoBehaviour {
    public GameObject nextTooltip;
    public VRTK_InteractableObject glass;
    public GameObject ContrL;
    public GameObject ContrR;

	// Use this for initialization
	void Start () {
        ContrL.GetComponent<VRTK_ControllerHighlighter>().HighlightElement(SDK_BaseController.ControllerElements.Trigger, Color.cyan, 1f);
        ContrR.GetComponent<VRTK_ControllerHighlighter>().HighlightElement(SDK_BaseController.ControllerElements.Trigger, Color.cyan, 1f);
    }
	
	// Update is called once per frame
	void Update () {
        if (glass.IsGrabbed())
        {
            //nextTooltip.SetActive(true);
            ContrL.GetComponent<VRTK_ControllerHighlighter>().UnhighlightElement(SDK_BaseController.ControllerElements.Trigger);
            ContrR.GetComponent<VRTK_ControllerHighlighter>().UnhighlightElement(SDK_BaseController.ControllerElements.Trigger);
            Destroy(gameObject);
        }
	}
}
