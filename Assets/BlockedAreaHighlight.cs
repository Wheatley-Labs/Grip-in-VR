using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockedAreaHighlight : MonoBehaviour {
    MeshRenderer meshRend;

    private void Start()
    {
        meshRend = GetComponent<MeshRenderer>();
        meshRend.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Grabbable"))
        {
            meshRend.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Grabbable"))
        {
            meshRend.enabled = false;
        }
    }
}
