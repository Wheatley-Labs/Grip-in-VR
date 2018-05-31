using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Tutorial_Tooltip_Generic : MonoBehaviour {
    private TooltipConfirm continueButton;
    public GameObject nextTooltip;

	// Use this for initialization
	void Start () {
        continueButton = GetComponentInChildren<TooltipConfirm>();
	}
	
	// Update is called once per frame
	void Update () {
        if (continueButton.buttonPressed)
        {
            nextTooltip.SetActive(true);
            Destroy(gameObject);
        }
	}
}
