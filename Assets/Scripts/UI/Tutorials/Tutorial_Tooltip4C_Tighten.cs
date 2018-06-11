using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Tutorial_Tooltip4C_Tighten : MonoBehaviour {
    public GameObject nextTooltip;
    public GameObject glass;
    public GameObject contrL;
    public GameObject contrR;

    private bool coroutineStarted = false;
    private bool highlightingStarted = false;

	// Use this for initialization
	void Start () {
        StartCoroutine(ToggleHighlighting());
    }

    // Update is called once per frame
    void Update () {
        if (highlightingStarted && glass.GetComponent<VRTK_InteractableObject>().IsGrabbed() && (IsGripLoose(contrL) || IsGripLoose(contrR)))
        {
            if (!coroutineStarted)
            {
                coroutineStarted = true;
                StartCoroutine(PotentiallyStopHighlighting());
            }
        }
	}

    IEnumerator ToggleHighlighting()
    {
        yield return new WaitForSeconds(3f);
        highlightingStarted = true;
        contrL.GetComponent<VRTK_ControllerHighlighter>().HighlightElement(SDK_BaseController.ControllerElements.Trigger, Color.cyan, 1f);
        contrR.GetComponent<VRTK_ControllerHighlighter>().HighlightElement(SDK_BaseController.ControllerElements.Trigger, Color.cyan, 1f);

        contrL.GetComponentInChildren<VRTK_ControllerTooltips>().UpdateText(VRTK_ControllerTooltips.TooltipButtons.TriggerTooltip, "Drücke den Trigger schwach\nfür einen losen Griff");
        contrR.GetComponentInChildren<VRTK_ControllerTooltips>().UpdateText(VRTK_ControllerTooltips.TooltipButtons.TriggerTooltip, "Drücke den Trigger schwach\nfür einen losen Griff");
    }

    IEnumerator PotentiallyStopHighlighting()
    {
        yield return new WaitForSeconds(2f);
        if (glass.GetComponent<VRTK_InteractableObject>().IsGrabbed() && (IsGripLoose(contrL) || IsGripLoose(contrR)))
        {
            contrL.GetComponentInChildren<VRTK_ControllerTooltips>().UpdateText(VRTK_ControllerTooltips.TooltipButtons.TriggerTooltip, "");
            contrR.GetComponentInChildren<VRTK_ControllerTooltips>().UpdateText(VRTK_ControllerTooltips.TooltipButtons.TriggerTooltip, "");
            contrL.GetComponentInChildren<VRTK_ControllerTooltips>().ToggleTips(false);
            contrR.GetComponentInChildren<VRTK_ControllerTooltips>().ToggleTips(false);

            yield return new WaitForSeconds(5f);

            contrL.GetComponent<VRTK_ControllerHighlighter>().UnhighlightElement(SDK_BaseController.ControllerElements.Trigger);
            contrR.GetComponent<VRTK_ControllerHighlighter>().UnhighlightElement(SDK_BaseController.ControllerElements.Trigger);

            nextTooltip.SetActive(true);
            Destroy(gameObject);
            yield break;
        }
        else
        {
            coroutineStarted = false;
            yield break;
        }
    }

    private bool IsGripLoose(GameObject contr)
    {
        if (contr.GetComponent<VRTK_ControllerEvents>().GetTriggerAxis() < 0.7 &&
            contr.GetComponent<VRTK_ControllerEvents>().GetTriggerAxis() > 0.2)
            return true;
        else
            return false;
    }
}
