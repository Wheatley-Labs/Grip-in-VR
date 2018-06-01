namespace VRTK.Examples {
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using VRTK.GrabAttachMechanics;
    using UnityEngine.UI;

    public class InteractionManager : MonoBehaviour {

        public int currentInteractionMode = 2;

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
            GetComponentInChildren<Text>().text = "Mode " + currentInteractionMode;

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
                    VRTK_InteractableObject cmp11 = obj.AddComponent<VRTK_InteractableObject>() as VRTK_InteractableObject;
                    VRTK_FixedJointGrabAttach cmp12 = obj.AddComponent<VRTK_FixedJointGrabAttach>() as VRTK_FixedJointGrabAttach;

                    cmp11.isGrabbable = true;
                    cmp11.holdButtonToGrab = true;
                    cmp11.grabAttachMechanicScript = cmp12;
                    cmp11.isUsable = false;

                    cmp12.precisionGrab = true;
                    cmp12.precisionButCentered = true;
                }

                //ENABLE MODE #2 - TIGHTEN WITH GRIP
                else if (newMode == 2)
                {
                    Interactable_GripBinary cmp21 = obj.AddComponent<Interactable_GripBinary>() as Interactable_GripBinary;
                    ConfigurableJointGrabAttach cmp22 = obj.AddComponent<ConfigurableJointGrabAttach>() as ConfigurableJointGrabAttach;

                    ConfigureVariableModes(cmp21, cmp22);
                    cmp21.gripToTighten = true;
                    cmp21.useOverrideButton = VRTK_ControllerEvents.ButtonAlias.GripPress;
                    ConfigureAnchor(cmp22);
                }

                //ENABLE MODE #3 - ONLY TRIGGER
                else if (newMode == 3)
                {
                    Interactable_GripBinary cmp31 = obj.AddComponent<Interactable_GripBinary>() as Interactable_GripBinary;
                    ConfigurableJointGrabAttach cmp32 = obj.AddComponent<ConfigurableJointGrabAttach>() as ConfigurableJointGrabAttach;

                    ConfigureVariableModes(cmp31, cmp32);
                    cmp31.gripToTighten = false;
                    ConfigureAnchor(cmp32);
                }

                else
                {
                    Debug.Log("No Interaction Mode specified");
                    return;
                }
            }
        }

        private void ConfigureVariableModes(Interactable_GripBinary cmpx1, ConfigurableJointGrabAttach cmpx2)
        {
            cmpx1.isGrabbable = true;
            cmpx1.holdButtonToGrab = true;
            cmpx1.grabAttachMechanicScript = cmpx2;
            cmpx1.isUsable = true;
            cmpx1.holdButtonToUse = true;
            cmpx1.useOnlyIfGrabbed = true;
            cmpx1.gravityPull = true;
            cmpx1.triggerToGrab = true;
            
            cmpx2.precisionGrab = true;
            cmpx2.precisionButCentered = true;
        }

        private void ConfigureAnchor(ConfigurableJointGrabAttach cmp)
        {
            if (cmp.gameObject.name.Contains("Pokal"))
            {
                cmp.anchor.y = 0.13f;
                cmp.connectedAnchor.y = -0.04f;
                cmp.connectedAnchor.z = 0.025f;
            }
        }
    }
}