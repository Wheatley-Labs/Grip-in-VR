using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using VRTK.Examples;

public class Tutorial_Tooltip6_BlockedArea : MonoBehaviour {
    public GameObject nextTooltip;
    public GameObject oldGlass;
    public GameObject newGlass;
    public GameObject target;
    public GameObject score;
    public GameObject blockedArea;
    private bool glassesChanged = false;

    private InteractionManager interactionManager;

    // Use this for initialization
    void Start () {
        blockedArea.SetActive(true);
        interactionManager = GameObject.FindGameObjectWithTag("ExperimentManager").GetComponent<InteractionManager>();

        StartCoroutine(changeGlasses());
    }

    // Update is called once per frame
    void Update () {
        if (GameObject.FindGameObjectWithTag("ExperimentManager").GetComponent<SessionManager>().error == 1)
        {
            nextTooltip.SetActive(true);
            Destroy(gameObject);
        }
	}

    IEnumerator changeGlasses()
    {
        yield return new WaitForSeconds(2f);
        oldGlass.SetActive(false);
        target.SetActive(false);
        score.SetActive(false);
        yield return new WaitForSeconds(2f);
        newGlass.SetActive(true);
        interactionManager.SetModeSingleObject(interactionManager.currentInteractionMode, newGlass);
    }
}
