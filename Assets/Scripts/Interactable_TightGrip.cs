namespace VRTK.Examples
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Interactable_TightGrip : VRTK_InteractableObject
    {
        private VRTK_ControllerEvents controllerEvents;
        private bool gripTight = false;

        public override void StartUsing(VRTK_InteractUse usingObject)
        {
            base.StartUsing(usingObject);
            controllerEvents = usingObject.GetComponent<VRTK_ControllerEvents>();
        }

        public override void StopUsing(VRTK_InteractUse previousUsingObject)
        {
            base.StopUsing(previousUsingObject);
            controllerEvents = null;
        }

        protected override void Update()
        {
            base.Update();
            if (controllerEvents)
            {
                TightenGrip();
                //VRTK_ControllerHaptics.TriggerHapticPulse(VRTK_ControllerReference.GetControllerReference(controllerEvents.gameObject), 0.25f, 0.1f, 0.01f);
            }
            else
            {
                LoosenGrip();
            }
        }

        private void TightenGrip()
        {
            if (!gripTight)
            {
                gripTight = true;

                ConfigurableJoint thisJoint = gameObject.GetComponent<ConfigurableJoint>();
                //thisJoint.angularXMotion = ConfigurableJointMotion.Limited;
                //thisJoint.angularYMotion = ConfigurableJointMotion.Limited;
                //thisJoint.angularZMotion = ConfigurableJointMotion.Limited;
                //GameObject rightController = GameObject.Find("Controller (right)");

                thisJoint.targetRotation = gameObject.transform.rotation;
                //Quaternion currentRotation = gameObject.transform.localRotation;
                //if (thisJoint.configuredInWorldSpace)
                //    SetTargetRotation(thisJoint, Quaternion.Euler(0, 0, 0), currentRotation, Space.World);
                //else
                //    SetTargetRotation(thisJoint, Quaternion.Euler(0, 0, 0), currentRotation, Space.Self);
                JointDrive thisJointDrive = thisJoint.slerpDrive;
                thisJointDrive.positionSpring = 9999f;
                thisJoint.slerpDrive = thisJointDrive;  
            }
        }

        private void LoosenGrip()
        {
            if (gripTight)
            {
                gripTight = false;

                ConfigurableJoint thisJoint = gameObject.GetComponent<ConfigurableJoint>();
                //thisJoint.angularXMotion = ConfigurableJointMotion.Free;
                //thisJoint.angularYMotion = ConfigurableJointMotion.Free;
                //thisJoint.angularZMotion = ConfigurableJointMotion.Free;
                JointDrive thisJointDrive = thisJoint.slerpDrive;
                thisJointDrive.positionSpring = 0f;
                thisJoint.slerpDrive = thisJointDrive;
            }
        }

        //code by mstevenson/ConfigurableJointExtensions.cs, retrieved 2018/02/05 12:50 from https://gist.github.com/mstevenson/495883
        static void SetTargetRotation(ConfigurableJoint thisJoint, Quaternion targetRotation, Quaternion startRotation, Space space)
        {
            // Calculate the rotation expressed by the joint's axis and secondary axis
            Vector3 right = thisJoint.axis;
            Vector3 forward = Vector3.Cross(thisJoint.axis, thisJoint.secondaryAxis).normalized;
            Vector3 up = Vector3.Cross(forward, right).normalized;
            Quaternion worldToJointSpace = Quaternion.LookRotation(forward, up);

            // Transform into world space
            Quaternion resultRotation = Quaternion.Inverse(worldToJointSpace);

            // Counter-rotate and apply the new local rotation.
            // Joint space is the inverse of world space, so we need to invert our value
            if (space == Space.World)
            {
                resultRotation *= startRotation * Quaternion.Inverse(targetRotation);
            }
            else
            {
                resultRotation *= Quaternion.Inverse(targetRotation) * startRotation;
            }

            // Transform back into joint space
            resultRotation *= worldToJointSpace;

            // Set target rotation to our newly calculated rotation
            thisJoint.targetRotation = resultRotation;
        }
    }
}