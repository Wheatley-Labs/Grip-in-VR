using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceTarget : MonoBehaviour {
    Animator targetAnimator;

	// Use this for initialization
	void Start () {
        targetAnimator = GetComponent<Animator>();
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pokal"))
        {
            targetAnimator.SetBool("success", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Pokal"))
        {
            targetAnimator.SetBool("success", false);
        }
    }
}
