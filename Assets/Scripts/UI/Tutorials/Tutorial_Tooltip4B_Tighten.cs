﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Tutorial_Tooltip4B_Tighten : MonoBehaviour {
    public GameObject nextTooltip;
    public GameObject glass;
    public GameObject ContrL;
    public GameObject ContrR;

    private bool highlightingStarted = false;
    private bool stoppingStarted = false;

	// Use this for initialization
	void Start () {
        StartCoroutine(ToggleHighlighting());
    }

    // Update is called once per frame
    void Update () {
        if (highlightingStarted && !stoppingStarted && glass.GetComponent<VRTK_InteractableObject>().IsGrabbed() && (ContrL.GetComponent<VRTK_ControllerEvents>().gripPressed || ContrR.GetComponent<VRTK_ControllerEvents>().gripPressed))
        {
            stoppingStarted = true;
            StartCoroutine(StopHighlighting());
        }
	}

    IEnumerator ToggleHighlighting()
    {
        yield return new WaitForSeconds(3f);
        highlightingStarted = true;
        ContrL.GetComponent<VRTK_ControllerHighlighter>().HighlightElement(SDK_BaseController.ControllerElements.GripLeft, Color.cyan, 1f);
        ContrL.GetComponent<VRTK_ControllerHighlighter>().HighlightElement(SDK_BaseController.ControllerElements.GripRight, Color.cyan, 1f);
        ContrR.GetComponent<VRTK_ControllerHighlighter>().HighlightElement(SDK_BaseController.ControllerElements.GripLeft, Color.cyan, 1f);
        ContrR.GetComponent<VRTK_ControllerHighlighter>().HighlightElement(SDK_BaseController.ControllerElements.GripRight, Color.cyan, 1f);

        ContrL.GetComponentInChildren<VRTK_ControllerTooltips>().UpdateText(VRTK_ControllerTooltips.TooltipButtons.GripTooltip, "Drücke zusätzlich die Grip-Buttons\nfür einen festen Griff");
        ContrR.GetComponentInChildren<VRTK_ControllerTooltips>().UpdateText(VRTK_ControllerTooltips.TooltipButtons.GripTooltip, "Drücke zusätzlich die Grip-Buttons\nfür einen festen Griff");
    }

    IEnumerator StopHighlighting()
    {
        yield return new WaitForSeconds(1f);
        if (!(ContrL.GetComponent<VRTK_ControllerEvents>().gripPressed || ContrR.GetComponent<VRTK_ControllerEvents>().gripPressed))
        {
            stoppingStarted = false;
            yield break;
        }

        ContrL.GetComponentInChildren<VRTK_ControllerTooltips>().UpdateText(VRTK_ControllerTooltips.TooltipButtons.GripTooltip, "");
        ContrR.GetComponentInChildren<VRTK_ControllerTooltips>().UpdateText(VRTK_ControllerTooltips.TooltipButtons.GripTooltip, "");
        ContrL.GetComponentInChildren<VRTK_ControllerTooltips>().ToggleTips(false);
        ContrR.GetComponentInChildren<VRTK_ControllerTooltips>().ToggleTips(false);

        yield return new WaitForSeconds(5f);
        ContrL.GetComponent<VRTK_ControllerHighlighter>().UnhighlightElement(SDK_BaseController.ControllerElements.GripLeft);
        ContrL.GetComponent<VRTK_ControllerHighlighter>().UnhighlightElement(SDK_BaseController.ControllerElements.GripRight);
        ContrR.GetComponent<VRTK_ControllerHighlighter>().UnhighlightElement(SDK_BaseController.ControllerElements.GripLeft);
        ContrR.GetComponent<VRTK_ControllerHighlighter>().UnhighlightElement(SDK_BaseController.ControllerElements.GripRight);

        nextTooltip.SetActive(true);
        Destroy(gameObject);
    }
}
