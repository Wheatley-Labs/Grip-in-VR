﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using VRTK.Examples;
using UnityEngine.UI;

public class Tutorial_Tooltip7_BreakGlass : MonoBehaviour {
    public GameObject nextTooltip;
    public GameObject moreGlasses;
    private int destroyedGlasses;
    private InteractionManager interactionManager;
    private SessionManager sessionManager;
    public Text textField;


    // Use this for initialization
    void Start () {
        moreGlasses.SetActive(true);
        interactionManager = GameObject.FindGameObjectWithTag("ExperimentManager").GetComponent<InteractionManager>();
        interactionManager.SetMode(interactionManager.currentInteractionMode);

        sessionManager = GameObject.FindGameObjectWithTag("ExperimentManager").GetComponent<SessionManager>();
        sessionManager.error = 0;
        destroyedGlasses = 0;
    }

    // Update is called once per frame
    void Update () {
        if (sessionManager.error > 0)
        {
            sessionManager.error = 0;
            destroyedGlasses += 1;
        }
        
        if (destroyedGlasses == 1)
        {
            textField.text = "Alles!";
        }

        if (destroyedGlasses == 5)
        {
            nextTooltip.SetActive(true);
            Destroy(gameObject);
        }
	}
}
