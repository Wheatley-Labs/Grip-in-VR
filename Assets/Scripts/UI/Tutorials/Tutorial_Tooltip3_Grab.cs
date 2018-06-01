using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using VRTK.Examples;

public class Tutorial_Tooltip3_Grab : MonoBehaviour {
    public InteractionManager interactionManager;
    public GameObject nextTooltipModeA;
    public GameObject nextTooltipModeB;
    public GameObject nextTooltipModeC;
    public VRTK_InteractableObject glass;
    public GameObject ContrL;
    public GameObject ContrR;

	// Use this for initialization
	void Start () {
        glass.gameObject.SetActive(true);

        ContrL.GetComponent<VRTK_ControllerHighlighter>().HighlightElement(SDK_BaseController.ControllerElements.Trigger, Color.cyan, 1f);
        ContrR.GetComponent<VRTK_ControllerHighlighter>().HighlightElement(SDK_BaseController.ControllerElements.Trigger, Color.cyan, 1f);

        ContrL.GetComponentInChildren<VRTK_ControllerTooltips>().UpdateText(VRTK_ControllerTooltips.TooltipButtons.TriggerTooltip, "Drücke den Trigger,\num Objekte zu greifen");
        ContrR.GetComponentInChildren<VRTK_ControllerTooltips>().UpdateText(VRTK_ControllerTooltips.TooltipButtons.TriggerTooltip, "Drücke den Trigger,\num Objekte zu greifen");
    }

    // Update is called once per frame
    void Update () {
        if (glass.IsGrabbed())
        {
            ContrL.GetComponent<VRTK_ControllerHighlighter>().UnhighlightElement(SDK_BaseController.ControllerElements.Trigger);
            ContrR.GetComponent<VRTK_ControllerHighlighter>().UnhighlightElement(SDK_BaseController.ControllerElements.Trigger);

            ContrL.GetComponentInChildren<VRTK_ControllerTooltips>().UpdateText(VRTK_ControllerTooltips.TooltipButtons.TriggerTooltip, "");
            ContrR.GetComponentInChildren<VRTK_ControllerTooltips>().UpdateText(VRTK_ControllerTooltips.TooltipButtons.TriggerTooltip, "");

            switch (interactionManager.currentInteractionMode) {
                case 1:
                    nextTooltipModeA.SetActive(true);
                    break;
                case 2:
                    nextTooltipModeB.SetActive(true);
                    break;
                case 3:
                    nextTooltipModeC.SetActive(true);
                    break;
            }
            Destroy(gameObject);
        }
	}
}
