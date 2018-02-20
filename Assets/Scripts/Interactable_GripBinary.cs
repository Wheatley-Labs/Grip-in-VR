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
        public bool gravityPull = true;
        public bool invertGrip = false;

        private VRTK_ControllerEvents controllerEvents;
        private Rigidbody thisRB;
        private ConfigurableJoint thisJoint;
        private Light flashLight;

        private Transform orgParent;
        private bool isParented = false;

        private float tightness = 0f;
        
        private void Start()
        {
            thisRB = GetComponent<Rigidbody>();
            flashLight = GetComponentInChildren<Light>();
        }

        public override void OnInteractableObjectGrabbed(InteractableObjectEventArgs e)
        {
            base.OnInteractableObjectGrabbed(e);

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

            if (invertGrip)
            {
                StartCoroutine(InverseInitialization());
            }
        }

        private IEnumerator InverseInitialization()
        {
            //wait for one frame
            yield return 0;

            //then parent the object
            Parent();
        }

        public override void OnInteractableObjectUngrabbed(InteractableObjectEventArgs e)
        {
            base.OnInteractableObjectUngrabbed(e);

            if (isParented)
                Unparent();

            thisRB.useGravity = true;
            thisRB.constraints = RigidbodyConstraints.None;
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
                    if (!invertGrip)
                    {
                        if (!isParented)
                            Parent();
                    }
                    else
                    {
                        if (isParented)
                            Unparent();
                    }
                }
                else //unparent when released
                {
                    if (!invertGrip)
                    {
                        if (isParented)
                            Unparent();
                    }
                    else
                        if (!isParented)
                            Parent();
                }

                if (controllerEvents.GetComponent<VRTK_ControllerEvents>().touchpadPressed)
                {
                    TriggerLight();
                }

                //TO DO
                //
                // Goes off again, just use on click
                // also enable when trigger not pressed
                //
                //TO DO
            }
            else
            {
                NewSlerp(0.03f);
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

        void TriggerLight()
        {
            flashLight.enabled = !(flashLight.enabled);
        }
    }
}