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
                thisJointDrive.positionDamper = 0.01f;
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