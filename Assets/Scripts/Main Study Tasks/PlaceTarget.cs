using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using VRTK.Examples;

public class PlaceTarget : MonoBehaviour {
    private Animator targetAnimator;
    private ScoreCounter scoreCounter;
    private InteractionManager interactionManager;

    public GameObject objPrefab;
    public GameObject spawnPos;
    public GameObject spawnParent;
    private bool alreadySpawned = false;

	// Use this for initialization
	void Start () {
        targetAnimator = GetComponent<Animator>();
        scoreCounter = GameObject.Find("Score").GetComponentInChildren<ScoreCounter>();
        interactionManager = GameObject.FindGameObjectWithTag("ExperimentManager").GetComponent<InteractionManager>();
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pokal") || other.CompareTag("Cup") || other.CompareTag("Carafe"))
        {
            targetAnimator.SetBool("success", true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!alreadySpawned)
        {
            if (other.CompareTag("Pokal") || other.CompareTag("Cup") || other.CompareTag("Carafe"))
            {
                if (!other.GetComponentInParent<VRTK_InteractableObject>().IsGrabbed())
                {
                    scoreCounter.UpdateScore(true);
                    alreadySpawned = true;
                    other.GetComponentInParent<MeshRenderer>().material.color = new Color32(88, 209, 90, 153);
                    other.GetComponentInParent<VRTK_InteractableObject>().isGrabbable = false;

                    if (scoreCounter.score < scoreCounter.maxScore)
                    {
                        SpawnNext();
                        StartCoroutine(DestroyLastGlass(other.transform.parent.gameObject));
                    }
                }
            }
        }
    }

    IEnumerator DestroyLastGlass(GameObject other)
    {
        yield return new WaitForSeconds(1.5f);
        alreadySpawned = false;
        targetAnimator.SetBool("success", false);
        Destroy(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Pokal") || other.CompareTag("Cup") || other.CompareTag("Carafe"))
        {
            targetAnimator.SetBool("success", false);
        }
    }

    public void SpawnNext(float waitFor = 0f)
    {
        StartCoroutine(Spawning(waitFor));
    }

    IEnumerator Spawning(float waitFor)
    {
        yield return new WaitForSeconds(waitFor);
        GameObject nextObject;
        nextObject = Instantiate(objPrefab, spawnPos.transform.position, Quaternion.identity, spawnParent.transform);
        interactionManager.SetModeSingleObject(interactionManager.currentInteractionMode, nextObject);
    }
}
