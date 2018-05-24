namespace VRTK.Examples {
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using VRTK.GrabAttachMechanics;

    public class InteractionManager : MonoBehaviour {

        static public int currentInteractionMode = 2;

        private void Start()
        {

        }

        private void OnLevelWasLoaded(int level)
        {
            SetMode(currentInteractionMode);

        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                currentInteractionMode = 1;
                SetMode(1);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                currentInteractionMode = 2;
                SetMode(2);
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                currentInteractionMode = 3;
                SetMode(3);
            }
        }

        public void SetMode(int newMode)
        {
            GameObject[] grabbables;

            grabbables = GameObject.FindGameObjectsWithTag("Grabbable");

            foreach (GameObject obj in grabbables)
            {
                //DELETE ALL EXISTING INTERACTION COMPONENTS
                if (obj.GetComponent<Interactable_GripBinary>() != null)
                {
                    Destroy(obj.GetComponent<Interactable_GripBinary>());
                }
                if (obj.GetComponent<ConfigurableJointGrabAttach>())
                {
                    Destroy(obj.GetComponent<ConfigurableJointGrabAttach>());
                }
                if(obj.GetComponent<VRTK_InteractableObject>() != null)
                {
                    Destroy(obj.GetComponent<VRTK_InteractableObject>());
                }
                if (obj.GetComponent<VRTK_FixedJointGrabAttach>())
                {
                    Destroy(obj.GetComponent<VRTK_FixedJointGrabAttach>());
                }

                // ENABLE MODE #1 - BASELINE
                if (newMode == 1)
                {
                    obj.AddComponent<VRTK_InteractableObject>();
                    obj.AddComponent<VRTK_FixedJointGrabAttach>();
                }

                //ENABLE MODE #2 - TIGHTEN WITH GRIP
                else if (newMode == 2)
                {
                    obj.AddComponent<Interactable_GripBinary>();
                    obj.AddComponent<ConfigurableJointGrabAttach>();
                    
                    GetComponent<Interactable_GripBinary>().gripToTighten = true;
                }

                //ENABLE MODE #3 - ONLY TRIGGER
                else if (newMode == 3)
                {
                    obj.AddComponent<Interactable_GripBinary>();
                    obj.AddComponent<ConfigurableJointGrabAttach>();

                    GetComponent<Interactable_GripBinary>().gripToTighten = false;
                }

                else
                {
                    Debug.Log("No Interaction Mode specified");
                    return;
                }
            }
        }
    }
}