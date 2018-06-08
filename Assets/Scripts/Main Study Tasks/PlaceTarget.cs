using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using VRTK.Examples;
using UnityEngine.SceneManagement;

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
                    Transform objTransform = other.GetComponentInParent<Transform>();
                    if (other.GetComponentInParent<Rigidbody>().angularVelocity.magnitude < 0.1f 
                        && objTransform.localEulerAngles.x < 3f && objTransform.localEulerAngles.z < 3f)
                    {
                        scoreCounter.UpdateScore(true);
                        alreadySpawned = true;
                        other.GetComponentInParent<MeshRenderer>().material.color = new Color32(88, 209, 90, 153);
                        other.GetComponentInParent<VRTK_InteractableObject>().isGrabbable = false;

                        if (scoreCounter.score < scoreCounter.maxScore)
                        {
                            StartCoroutine(SpawnNext());
                            StartCoroutine(DestroyLastGlass(other.transform.parent.gameObject));
                        }
                    }
                }
            }
        }
    }

    IEnumerator DestroyLastGlass(GameObject other)
    {
        other.isStatic = true;
        yield return new WaitForSeconds(0.8f);
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

    //workaround for BreakGlass-script to trigger the spawning externally
    public void ExecuteCoroutineforSpawning(float waitFor = 0f)
    {
        StartCoroutine(SpawnNext(waitFor));
    }

    private IEnumerator SpawnNext(float waitFor = 0f)
    {
        yield return new WaitForSeconds(waitFor);
        GameObject nextObject;
        nextObject = Instantiate(objPrefab, spawnPos.transform.position, Quaternion.identity, spawnParent.transform);
        interactionManager.SetModeSingleObject(interactionManager.currentInteractionMode, nextObject);
        if (SceneManager.GetActiveScene().name.Contains("Tutorial")){
            Destroy(nextObject.GetComponent<BreakGlass>());
        }
    }
}
