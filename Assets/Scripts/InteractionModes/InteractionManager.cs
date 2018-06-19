namespace VRTK.Examples {
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using VRTK.GrabAttachMechanics;
    using UnityEngine.UI;
    using UnityEngine.SceneManagement;

    public class InteractionManager : MonoBehaviour {

        public int currentInteractionMode = 2;
        private static int handoverInteractionMode = 2;

        private void Start()
        {

        }

        private void OnLevelWasLoaded(int level)
        {
            if (SceneManager.GetActiveScene().name.Contains("Tutorial"))
                handoverInteractionMode = currentInteractionMode;
            else
                currentInteractionMode = handoverInteractionMode;

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
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                currentInteractionMode = 4;
                SetMode(4);
            }
        }

        public void SetMode(int newMode)
        {
            handoverInteractionMode = newMode;
            GetComponentInChildren<Text>().text = "Mode " + currentInteractionMode;

            GameObject[] grabbables;

            grabbables = GameObject.FindGameObjectsWithTag("Grabbable");

            foreach (GameObject obj in grabbables)
            {
                SetModeSingleObject(newMode, obj);
            }
        }

        public void SetModeSingleObject(int newMode, GameObject obj)
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
            if (obj.GetComponent<VRTK_InteractableObject>() != null)
            {
                Destroy(obj.GetComponent<VRTK_InteractableObject>());
            }
            if (obj.GetComponent<VRTK_FixedJointGrabAttach>())
            {
                Destroy(obj.GetComponent<VRTK_FixedJointGrabAttach>());
            }

            // ENABLE MODE #1 - BASELINE: ONLY FIRM 
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
                cmp21.isUsable = true;
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
                cmp31.isUsable = true;
                cmp31.gripToTighten = false;
                cmp31.useOverrideButton = VRTK_ControllerEvents.ButtonAlias.TriggerPress;
                ConfigureAnchor(cmp32);
            }

            //ENABLE MODE #4 - BASELINE: ONLY LOOSE
            else if (newMode == 4)
            {
                Interactable_GripBinary cmp41 = obj.AddComponent<Interactable_GripBinary>() as Interactable_GripBinary;
                ConfigurableJointGrabAttach cmp42 = obj.AddComponent<ConfigurableJointGrabAttach>() as ConfigurableJointGrabAttach;

                ConfigureVariableModes(cmp41, cmp42);
                cmp41.isUsable = false;
                ConfigureAnchor(cmp42);
            }

            else
            {
                Debug.Log("No Interaction Mode specified");
                return;
            }
        }

        private void ConfigureVariableModes(Interactable_GripBinary cmpX1, ConfigurableJointGrabAttach cmpX2)
        {
            cmpX1.isGrabbable = true;
            cmpX1.holdButtonToGrab = true;
            cmpX1.grabAttachMechanicScript = cmpX2;
            cmpX1.holdButtonToUse = true;
            cmpX1.useOnlyIfGrabbed = true;
            cmpX1.gravityPull = true;
            cmpX1.triggerToGrab = true;
            
            cmpX2.precisionGrab = true;
            cmpX2.precisionButCentered = true;
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