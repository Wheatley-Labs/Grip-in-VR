using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using VRTK.Examples;

public class Tutorial_Tooltip6_BlockedArea : MonoBehaviour {
    public GameObject nextTooltip;
    public GameObject glass;
    public GameObject target;
    public GameObject score;
    public GameObject blockedArea;
    public GameObject windowPokalPrefab;

    // Use this for initialization
    void Start () {
        blockedArea.SetActive(true);
        BreakGlass brkGls = glass.AddComponent<BreakGlass>() as BreakGlass;
        brkGls.window = windowPokalPrefab;
        brkGls.stability = 3;
    }

    // Update is called once per frame
    void Update () {
        if (glass != null)
        {
            if (glass.GetComponent<Interactable_GripBinary>().IsGrabbed())
            {
                target.SetActive(false);
                score.SetActive(false);
            }
        }

        if (GameObject.FindGameObjectWithTag("ExperimentManager").GetComponent<SessionManager>().error == 1)
        {
            nextTooltip.SetActive(true);
            Destroy(gameObject);
        }
	}
}
