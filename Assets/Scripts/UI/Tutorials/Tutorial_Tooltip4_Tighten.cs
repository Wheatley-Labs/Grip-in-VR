using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Tutorial_Tooltip4_Tighten : MonoBehaviour {
    public GameObject nextTooltip;
    public VRTK_InteractableObject glass;
    public GameObject ContrL;
    public GameObject ContrR;

	// Use this for initialization
	void Start () {
        ContrL.GetComponent<VRTK_ControllerHighlighter>().HighlightElement(SDK_BaseController.ControllerElements.GripLeft, Color.cyan, 1f);
        ContrL.GetComponent<VRTK_ControllerHighlighter>().HighlightElement(SDK_BaseController.ControllerElements.GripRight, Color.cyan, 1f);
        ContrR.GetComponent<VRTK_ControllerHighlighter>().HighlightElement(SDK_BaseController.ControllerElements.GripLeft, Color.cyan, 1f);
        ContrR.GetComponent<VRTK_ControllerHighlighter>().HighlightElement(SDK_BaseController.ControllerElements.GripRight, Color.cyan, 1f);

        ContrL.GetComponentInChildren<VRTK_ControllerTooltips>().UpdateText(VRTK_ControllerTooltips.TooltipButtons.GripTooltip, "Drücke die Grip-Buttons\nfür einen festen Griff");
        ContrR.GetComponentInChildren<VRTK_ControllerTooltips>().UpdateText(VRTK_ControllerTooltips.TooltipButtons.GripTooltip, "Drücke die Grip-Buttons\nfür einen festen Griff");
    }

    // Update is called once per frame
    void Update () {
        if (ContrL.GetComponent<VRTK_ControllerEvents>().gripPressed || ContrR.GetComponent<VRTK_ControllerEvents>().gripPressed)
        {
            ContrL.GetComponent<VRTK_ControllerHighlighter>().UnhighlightElement(SDK_BaseController.ControllerElements.GripLeft);
            ContrL.GetComponent<VRTK_ControllerHighlighter>().UnhighlightElement(SDK_BaseController.ControllerElements.GripRight);
            ContrR.GetComponent<VRTK_ControllerHighlighter>().UnhighlightElement(SDK_BaseController.ControllerElements.GripLeft);
            ContrR.GetComponent<VRTK_ControllerHighlighter>().UnhighlightElement(SDK_BaseController.ControllerElements.GripRight);

            ContrL.GetComponentInChildren<VRTK_ControllerTooltips>().ToggleTips(false);
            ContrR.GetComponentInChildren<VRTK_ControllerTooltips>().ToggleTips(false);

            //nextTooltip.SetActive(true);
            Destroy(gameObject);
        }
	}
}
