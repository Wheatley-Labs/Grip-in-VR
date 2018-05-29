using System.Collections;
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
        if (gameObject.transform.localEulerAngles.x > 330)
            force.torque = Vector3.back /2;

        else if (gameObject.transform.localEulerAngles.x < 280)
            force.torque = Vector3.forward /2;

        else
            force.torque = Vector3.zero;
	}
}
