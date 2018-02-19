namespace VRTK.Examples
{

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Test_ChildGrab_right : MonoBehaviour
    {
        GameObject obj;
        Rigidbody objRB;
        ConfigurableJoint objJoint;

        GameObject rContrInteract;

        float tightness = 0f;

        void Start()
        {
            obj = GameObject.FindGameObjectWithTag("Grabbable");
            objRB = obj.GetComponent<Rigidbody>();
            objJoint = obj.GetComponent<ConfigurableJoint>();
            rContrInteract = GameObject.Find("RightController");

        }

        void Update()
        {
            tightness = rContrInteract.GetComponent<VRTK_ControllerEvents>().GetTriggerAxis();
            
            if (tightness < 0.05)
            {
                JointDrive thisJointDrive = objJoint.slerpDrive;
                thisJointDrive.positionDamper = 0.0f;
                objJoint.slerpDrive = thisJointDrive;

                AllowRotation();
            }
            else if (tightness < 0.35)
            {
                JointDrive thisJointDrive = objJoint.slerpDrive;
                thisJointDrive.positionDamper = tightness * tightness;
                objJoint.slerpDrive = thisJointDrive;

                AllowRotation();
            }
            else if (tightness < 0.7)
            {
                JointDrive thisJointDrive = objJoint.slerpDrive;
                thisJointDrive.positionDamper = tightness;
                objJoint.slerpDrive = thisJointDrive;

                AllowRotation();
            }
            else if (tightness < 0.9)
            {
                JointDrive thisJointDrive = objJoint.slerpDrive;
                thisJointDrive.positionDamper = tightness * 3f;
                objJoint.slerpDrive = thisJointDrive;

                AllowRotation();
            }
            else
            {
                JointDrive thisJointDrive = objJoint.slerpDrive;
                thisJointDrive.positionDamper = 9999f;
                objJoint.slerpDrive = thisJointDrive;

                FreezeRotation();
            }
            
        }

/*      //Dimis attempt to scale the velocity according to the tightness of the grip
        //Problem: Objects needs to be a child, to use the parent's rotation --> weird joint physics
        void FixedUpdate()
        {
            if (rContrInteract.GetComponent<VRTK_ControllerEvents>().gripClicked)
            {
                objRB.MovePosition(objJoint.connectedAnchor);
                tightness = rContrInteract.GetComponent<VRTK_ControllerEvents>().GetTriggerAxis();
                Debug.Log("tightness" + tightness);
                objRB.velocity *= Mathf.Clamp01(1f - tightness);
                objRB.angularVelocity *= Mathf.Clamp01(1f - tightness);
            }
        }
*/        
        void FreezeRotation()
        {
            objRB.constraints = RigidbodyConstraints.FreezeRotation;
        }

        void AllowRotation()
        {
            objRB.constraints = RigidbodyConstraints.None;
        }
    }
}