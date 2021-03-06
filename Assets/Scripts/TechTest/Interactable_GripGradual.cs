﻿namespace VRTK.Examples
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Interactable_GripGradual : VRTK_InteractableObject
    {
        private VRTK_ControllerEvents controllerEvents;
        private GameObject rContr;
        private GameObject lContr;
        private GameObject rContrInteract;
        private GameObject lContrInteract;
        private Rigidbody rContrRB;
        private Rigidbody lContrRB;
        private Rigidbody thisRB;

        private bool gripTight = false;
        private bool isInitialized = false;

        private Quaternion rotLastFramePhys = Quaternion.identity;
        private Quaternion rotLastFrameContr = Quaternion.identity;
        private Quaternion contrRotOffset = Quaternion.identity;
        private Quaternion dynamicContrRotOffset = Quaternion.identity;

        private Vector3 angVelLastFramePhys = Vector3.zero;
        private Vector3 angVelLastFrameContr = Vector3.zero;

        private float tightness = 0f;

        //private new void Awake()
        //{
        //    rContr = GameObject.Find("Controller (right)");
        //    lContr = GameObject.Find("Controller (left)");
        //}

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

        private new void FixedUpdate()
        {
            if (GetComponent<VRTK_InteractableObject>().IsGrabbed())
            {
                if (isInitialized)
                {
                    //The changes in rotation and angular velocity since the last frame 
                    Quaternion deltaRotPhys =  Quaternion.Inverse(rotLastFramePhys) * transform.rotation;                                           //the difference between the two rotations
                    Quaternion deltaRotContr =  Quaternion.Inverse(rotLastFrameContr) * (rContr.transform.rotation * contrRotOffset);
                    //Vector3 deltaAngVelPhys = thisRB.angularVelocity - angVelLastFramePhys;
                    //Vector3 deltaAngVelContr = rContrRB.angularVelocity - angVelLastFrameContr;
                    //Debug.Log("Delta Physics: " + deltaRotPhys);
                    //Debug.Log("Controller Last Frame:    " + rotLastFrameContr);
                    //Debug.Log("Controller Current Frame: " + rContr.transform.rotation.eulerAngles);
                    //Debug.Log("Delta Controller: " + deltaRotContr);

                    //How tight is the grip? As much as the trigger is pulled resulting in values between 0.0 and 1.0
                    tightness = rContrInteract.GetComponent<VRTK_ControllerEvents>().GetTriggerAxis();
                    //tightness = 1f;   //for testing the adjustment to the controller

                    //Setting the actual rotation for the current frame
                    //transform.localRotation = rotLastFramePhys *                         //start off with the last frame's orientation 
                    //Quaternion.Slerp(deltaRotPhys, deltaRotContr, tightness);   //Add the change in rotation according to a slerp between the controller rotation and the physical calculation by the tightness of the grip
                    
                    //Calculate the share of the physical impact according to tightness, then apply rotation
                    Quaternion scaledPhysDelta = Quaternion.Slerp(deltaRotPhys, Quaternion.identity, tightness);
                    transform.rotation = rotLastFramePhys * scaledPhysDelta;

                    //Calculate the share of the controller rotation, then apply the rotation around the center of the controller
                    Vector3 rContrRotAxis;
                    float rContrRotAngle;
                    Quaternion scaledContrDelta = Quaternion.Slerp(deltaRotContr, Quaternion.identity, (1 - tightness));
                    scaledContrDelta.ToAngleAxis(out rContrRotAngle, out rContrRotAxis);
                    transform.RotateAround(rContr.transform.position, rContrRotAxis, rContrRotAngle);

                    //Also the angular velocity must be adjusted accordingly
                    //thisRB.angularVelocity =
                    //    angVelLastFramePhys +
                    //    tightness * deltaAngVelContr +
                    //    (1 - tightness) * deltaAngVelPhys;

                    //The new rotation and angular velocity is stored for the next frame's calculations
                    rotLastFrameContr = rContr.transform.rotation * contrRotOffset;
                    rotLastFramePhys = transform.rotation;
                    //angVelLastFrameContr = rContrRB.angularVelocity;
                    //angVelLastFramePhys = thisRB.angularVelocity;
                    
                }
                else
                {
                    rContr = GameObject.FindGameObjectWithTag("Right Controller");
                    //lContr = GameObject.FindGameObjectWithTag("Left Controller");
                    rContrInteract = GameObject.Find("RightController");
                    //lContrInteract = GameObject.Find("LeftController");
                    rContrRB = rContrInteract.GetComponent<Rigidbody>();
                    //lContrRB = lContrInteract.GetComponent<Rigidbody>();
                    thisRB = gameObject.GetComponent<Rigidbody>();

                    //Current values are saved for the changes in rotation to be calculated in the next frames
                    rotLastFramePhys = transform.rotation;
                    contrRotOffset = Quaternion.Inverse(rContr.transform.rotation);
                    rotLastFrameContr = rContr.transform.rotation * contrRotOffset;

                    //Current values are saved for the changes in angular velocity to be calculated in the next frames
                    //angVelLastFramePhys = thisRB.angularVelocity;
                    //angVelLastFrameContr = rContrRB.angularVelocity;

                    isInitialized = true;
                }
            }
            else
                if (isInitialized)
            {

                isInitialized = false;
            }
        }


        /// <summary>
        /// Approach of 
        /// - first using the Angular Limits
        /// - later setting the target rotation according to current localRotation and using slerpDrive.positionSpring for applying it
        /// </summary>

        protected override void Update()
        {
            base.Update();
            //if (controllerEvents)
            //{
            //    TightenGrip();
            //    //VRTK_ControllerHaptics.TriggerHapticPulse(VRTK_ControllerReference.GetControllerReference(controllerEvents.gameObject), 0.25f, 0.1f, 0.01f);
            //}
            //else
            //{
            //    LoosenGrip();
            //}
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