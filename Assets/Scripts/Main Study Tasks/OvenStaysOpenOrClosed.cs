using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class OvenStaysOpenOrClosed : MonoBehaviour {

    private ConstantForce force;
    private VRTK_InteractableObject interactableObject;

    // Use this for initialization
    void Start() {
        force = GetComponent<ConstantForce>();
        interactableObject = GetComponent<VRTK_InteractableObject>();
    }

    // Update is called once per frame
    void Update() {
        if (gameObject.transform.localEulerAngles.x > 320)
            force.torque = Vector3.back / 2;

        else if (gameObject.transform.localEulerAngles.x < 290)
            force.torque = Vector3.forward / 2;

        else
            force.torque = Vector3.zero;

        if (gameObject.transform.localEulerAngles.x > 358 && gameObject.GetComponent<Rigidbody>().velocity.magnitude < 0.1 && !interactableObject.IsGrabbed())
        {
            StartCoroutine(MoveAway());
        }
    }
        
    IEnumerator MoveAway()
    {
        yield return new WaitForSeconds(0.3f);
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
        Destroy(GetComponent<HingeJoint>());
        foreach (Collider col in GetComponents<Collider>())
        {
            Destroy(col);
        }

        force.force = Vector3.left * 0.5f;

        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
