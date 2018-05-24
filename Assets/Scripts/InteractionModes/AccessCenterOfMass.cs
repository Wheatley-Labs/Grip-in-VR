using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccessCenterOfMass : MonoBehaviour {

    public GameObject centerOfMass;
    private Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = transform.InverseTransformPoint(centerOfMass.transform.position);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
