using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupboardLStaysOpenOrClosed : MonoBehaviour {

    private ConstantForce force;

	// Use this for initialization
	void Start () {
        force = GetComponent<ConstantForce>();
	}

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.localEulerAngles.y < 20)
            force.torque = Vector3.down / 10;

        else if (gameObject.transform.localEulerAngles.y > 50)
            force.torque = Vector3.up / 10;

        else
            force.torque = Vector3.zero;
    }
}
