namespace VRTK.Examples
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(VRTK_InteractControllerAppearance))]

    public class Interactable_GripBinary : VRTK_InteractableObject
    {
        public bool gravityPull = true;
        public bool triggerToGrab = false;
        public bool gripToTighten = false;
        public bool hideController = false;

        private VRTK_ControllerEvents controllerEvents;
        private VRTK_InteractGrab grabbingController;
        private bool objectGrabbed = false;
        private bool objectUsed = false;

        private Rigidbody thisRB;
        private ConfigurableJoint thisJoint;
        private Light flashLight;

        private Transform orgParent;
        private bool isParented = false;

        private float tightness = 0f;
        public float defaultDangelingDamper = 0.03f;
        private float lastLightTrigger;

        protected override void Awake()
        {
            if (gripToTighten)
            {
                triggerToGrab = true;
                useOverrideButton = VRTK_ControllerEvents.ButtonAlias.GripPress;
            }

            if (triggerToGrab)
                grabOverrideButton = VRTK_ControllerEvents.ButtonAlias.TriggerTouch;
        }

        private void Start()
        {
            thisRB = GetComponent<Rigidbody>();

            flashLight = GetComponentInChildren<Light>();
            lastLightTrigger = Time.timeSinceLevelLoad;

            if (hideController)
                GetComponent<VRTK_InteractControllerAppearance>().hideControllerOnGrab = true;
        }

        public override void Grabbed(VRTK_InteractGrab currentGrabbingObject = null)
        {
            base.Grabbed(currentGrabbingObject);
            grabbingController = currentGrabbingObject;
            objectGrabbed = true;

            if (gravityPull)
            {
                thisRB.useGravity = true;
                thisRB.constraints = RigidbodyConstraints.None;
            }
            else
            {
                thisRB.useGravity = false;
                thisRB.constraints = RigidbodyConstraints.FreezeRotation;
            }
        }

        public override void Ungrabbed(VRTK_InteractGrab previousGrabbingObject = null)
        {
            base.Ungrabbed(previousGrabbingObject);
            grabbingController = null;
            objectGrabbed = false;

            if (isParented)
                Unparent();

            thisRB.useGravity = true;
            thisRB.constraints = RigidbodyConstraints.None;
        }

        public override void StartUsing(VRTK_InteractUse usingObject)
        {
            base.StartUsing(usingObject);
            objectUsed = true;
            controllerEvents = usingObject.GetComponent<VRTK_ControllerEvents>();
            thisJoint = GetComponent<ConfigurableJoint>();
        }

        public override void StopUsing(VRTK_InteractUse previousUsingObject)
        {
            base.StopUsing(previousUsingObject);
            controllerEvents = null;
            objectUsed = false;
        }

        protected override void Update()
        {
            if (objectUsed)
            {
                //Parent the object when the trigger is fully pulled
                if (!gripToTighten)
                {
                    if (controllerEvents.GetComponent<VRTK_ControllerEvents>().triggerClicked)
                    {
                        if (!isParented)
                            Parent();
                    }
                    else //unparent when released
                    {
                        if (isParented)
                            Unparent();
                    }
                }
                else
                {
                    if (controllerEvents.GetComponent<VRTK_ControllerEvents>().gripPressed)
                    {
                        if (!isParented)
                            Parent();
                    }
                }
            }
            else if (isParented && gripToTighten)
                Unparent();

            //Adjust damper
            if (objectGrabbed && !gripToTighten)
            {
                //Adjust position damper when object is grabbed
                tightness = grabbingController.GetComponent<VRTK_ControllerEvents>().GetTriggerAxis();
                if (tightness < 0.2f)
                    NewSlerp(defaultDangelingDamper);
                else if (tightness < 0.7)
                    NewSlerp((tightness * tightness) /2);
                else
                    NewSlerp(tightness / 2);
            }
            //Turn on the flashlight if a Light component is found in the children
            if (objectGrabbed && grabbingController.GetComponent<VRTK_ControllerEvents>().touchpadPressed && Time.timeSinceLevelLoad > (lastLightTrigger + 0.4f))
            {
                if (flashLight != null)
                    TriggerFlashlight();
            }
        }

        private void Unparent()
        {
            transform.parent = orgParent;
            if (gravityPull)
            {
                thisRB.constraints = RigidbodyConstraints.None;
                thisRB.useGravity = true;
            }
            else
                thisRB.useGravity = false;

            isParented = false;
        }

        private void Parent()
        {
            thisRB.constraints = RigidbodyConstraints.FreezeRotation;
            orgParent = transform.parent;
            transform.parent = GetUsingObject().transform;

            isParented = true;
        }

        void NewSlerp(float value)
        {
            if (thisJoint != null)
            {
                JointDrive thisJointDrive = thisJoint.slerpDrive;
                thisJointDrive.positionDamper = value;
                thisJoint.slerpDrive = thisJointDrive;
            }
        }

        private void TriggerFlashlight()
        {
            flashLight.enabled = !(flashLight.enabled);
            lastLightTrigger = Time.timeSinceLevelLoad;
        }
    }
}