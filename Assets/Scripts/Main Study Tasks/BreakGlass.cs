using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakGlass : MonoBehaviour {
    public GameObject window;
    public float stability;
    private Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}

    void OnCollisionEnter(Collision col)
    {
        if (col.relativeVelocity.magnitude > stability)
            Break();
    }

    private void Break()
    {
        Vector3 tmpPos = gameObject.transform.position;
        tmpPos.y += 0.25f;
        GameObject windowInstance;
        windowInstance = Instantiate(window, tmpPos, Quaternion.identity);
        //windowInstance.GetComponent<BreakableWindow>().breakWindow();
        Destroy(gameObject);
    }
}

