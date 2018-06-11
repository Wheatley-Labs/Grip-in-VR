using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupboardRStaysOpenOrClosed : MonoBehaviour {

    private ConstantForce force;

	// Use this for initialization
	void Start () {
        force = GetComponent<ConstantForce>();
	}
	
	// Update is called once per frame
	void Update () {
        if (gameObject.transform.localEulerAngles.y > 330)
            force.torque = Vector3.up /6;

        else if (gameObject.transform.localEulerAngles.y < 300)
            force.torque = Vector3.down/6;

        else
            force.torque = Vector3.zero;
	}
}
