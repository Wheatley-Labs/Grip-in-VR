namespace VRTK.Examples
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]

    public class Interactable_GripBinary : VRTK_InteractableObject
    {
        private VRTK_ControllerEvents controllerEvents;
        private Rigidbody thisRB;
        private ConfigurableJoint thisJoint;

        private Transform orgParent;
        private bool isParented = false;

        private float tightness = 0f;
        
        private void Start()
        {
            thisRB = GetComponent<Rigidbody>();
        }


        public override void StartUsing(VRTK_InteractUse usingObject)
        {
            base.StartUsing(usingObject);
            controllerEvents = usingObject.GetComponent<VRTK_ControllerEvents>();
            thisJoint = GetComponent<ConfigurableJoint>();
        }

        public override void StopUsing(VRTK_InteractUse previousUsingObject)
        {
            base.StopUsing(previousUsingObject);
            controllerEvents = null;
        }

        public override void OnInteractableObjectUngrabbed(InteractableObjectEventArgs e)
        {
            base.OnInteractableObjectUngrabbed(e);

            if (isParented)
            {
                Unparent();
            }
        }

        protected override void Update()
        {
            if (controllerEvents)
            {
                //Adjust position damper when object is grabbed
                tightness = controllerEvents.GetComponent<VRTK_ControllerEvents>().GetTriggerAxis();
                NewSlerp(tightness/2f);

                //Parent the object when the trigger is fully pulled
                if (controllerEvents.GetComponent<VRTK_ControllerEvents>().triggerClicked)
                {
                    if (!isParented)
                    {
                        Parent();
                    }

                }
                else //unparent when released
                {
                    if (isParented)
                    {
                        Unparent();
                    }
                }
            }
            else
            {
                NewSlerp(0.03f);
            }

        }

        private void Unparent()
        {
            transform.parent = orgParent;
            thisRB.constraints = RigidbodyConstraints.None;

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
    }
}