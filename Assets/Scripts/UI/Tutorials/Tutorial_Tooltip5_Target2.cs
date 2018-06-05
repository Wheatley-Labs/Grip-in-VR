using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Tooltip5_Target2 : MonoBehaviour {
    public GameObject nextTooltip;
    public GameObject target;
    public GameObject score;

	// Use this for initialization
	void Start () {

    }

    // Update is called once per frame
    void Update () {
        if (score.GetComponent<ScoreCounter>().score == 6)
        {
            nextTooltip.SetActive(true);
            Destroy(gameObject);
        }
	}
}
