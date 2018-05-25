using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawerStaysOpenOrClosed : MonoBehaviour {

    private ConstantForce force;

	// Use this for initialization
	void Start () {
        force = GetComponent<ConstantForce>();
	}
	
	// Update is called once per frame
	void Update () {
        if (gameObject.transform.localPosition.y < -4.6f)
            force.force = Vector3.right;

        else if (gameObject.transform.localPosition.y > -3.9f)
            force.force = Vector3.left;

        else
            force.force = Vector3.zero;
	}
}
