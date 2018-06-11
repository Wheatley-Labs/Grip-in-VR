﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OvenStaysOpenOrClosed : MonoBehaviour {

    private ConstantForce force;

	// Use this for initialization
	void Start () {
        force = GetComponent<ConstantForce>();
	}
	
	// Update is called once per frame
	void Update () {
        if (gameObject.transform.localEulerAngles.x > 320)
            force.torque = Vector3.back /2;

        else if (gameObject.transform.localEulerAngles.x < 290)
            force.torque = Vector3.forward /2;

        else
            force.torque = Vector3.zero;

        //if (gameObject.transform.localEulerAngles.x > 358 && gameObject.GetComponent<Rigidbody>().velocity.magnitude < 0.1)
        //{

        //}
    }
}
