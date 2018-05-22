using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceTarget : MonoBehaviour {
    Animator targetAnimator;
    private ScoreCounter scoreCounter;

	// Use this for initialization
	void Start () {
        targetAnimator = GetComponent<Animator>();
        scoreCounter = GameObject.Find("Score").GetComponentInChildren<ScoreCounter>();
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pokal"))
        {
            targetAnimator.SetBool("success", true);
            scoreCounter.UpdateScore(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Pokal"))
        {
            targetAnimator.SetBool("success", false);
            scoreCounter.UpdateScore(false);
        }
    }
}
