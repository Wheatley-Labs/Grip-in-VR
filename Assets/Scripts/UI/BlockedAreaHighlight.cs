namespace VRTK.Examples {
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

        /*
        // Destroy the game object when being placed in a blocked area
        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Grabbable"))
            {
                if (!other.GetComponent<Interactable_GripBinary>().IsGrabbed())
                {
                    if (other.GetComponent<BreakGlass>() != null)
                        other.GetComponent<BreakGlass>().Break();
                    else
                        Destroy(other);
                }
        }
        }*/
    }
}