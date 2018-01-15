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
                thisJoint.angularXMotion = ConfigurableJointMotion.Limited;
                thisJoint.angularYMotion = ConfigurableJointMotion.Limited;
                thisJoint.angularZMotion = ConfigurableJointMotion.Limited;
            }
        }

        private void LoosenGrip()
        {
            if (gripTight)
            {
                gripTight = false;

                ConfigurableJoint thisJoint = gameObject.GetComponent<ConfigurableJoint>();
                thisJoint.angularXMotion = ConfigurableJointMotion.Free;
                thisJoint.angularYMotion = ConfigurableJointMotion.Free;
                thisJoint.angularZMotion = ConfigurableJointMotion.Free;
            }
        }
    }
}