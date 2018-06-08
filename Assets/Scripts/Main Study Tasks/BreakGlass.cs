using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BreakGlass : MonoBehaviour {
    public GameObject window;
    public float stability;
    private Rigidbody rb;
    public bool alreadyBroken = false;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}

    void OnCollisionEnter(Collision col)
    {
        if (!alreadyBroken)
        {
            if (col.relativeVelocity.magnitude > stability)
            {
                alreadyBroken = true;
                Break();
            }
        }
    }

    public void Break()
    {
        Vector3 tmpPos = gameObject.transform.position;
        tmpPos.y += 0.25f;
        GameObject windowInstance;
        windowInstance = Instantiate(window, tmpPos, Quaternion.identity);
        GameObject.FindGameObjectWithTag("ExperimentManager").GetComponent<SessionManager>().AddError();
        if (!SceneManager.GetActiveScene().name.Contains("Tutorial"))
        {
            GameObject.FindGameObjectWithTag("Target").GetComponent<PlaceTarget>().ExecuteCoroutineforSpawning(1f);
        }
        Destroy(gameObject);
    }
}

