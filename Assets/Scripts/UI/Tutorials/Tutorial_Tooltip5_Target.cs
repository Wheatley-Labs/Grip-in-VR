using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using VRTK.Examples;

public class Tutorial_Tooltip5_Target : MonoBehaviour {
    public GameObject nextTooltip;
    public VRTK_InteractableObject glass;
    public GameObject target;
    public GameObject score;

	// Use this for initialization
	void Start () {
        target.SetActive(true);
        score.SetActive(true);
    }

    // Update is called once per frame
    void Update () {
        if (score.GetComponent<ScoreCounter>().score == 1)
        {
            //nextTooltip.SetActive(true);
            Destroy(gameObject);
        }
	}
}
