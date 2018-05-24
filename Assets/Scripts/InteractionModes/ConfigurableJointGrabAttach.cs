// Configurable Joint Grab Attach|GrabAttachMechanics|50040
// Adapted from Fixed Joint Grab Attach by Michael Bonfert
namespace VRTK.GrabAttachMechanics
{
    using UnityEngine;

    /// <summary>
    /// The Hinge Joint Grab Attach script is used to create a Configurable Joint connection between the object and the grabbing object.
    /// </summary>
    
    [AddComponentMenu("Scripts/ConfigurableJointGrabAttach")]
    public class ConfigurableJointGrabAttach : VRTK_BaseJointGrabAttach
    {
        [Tooltip("Maximum force the joint can withstand before breaking. Infinity means unbreakable.")]
        public float breakForce = Mathf.Infinity;
        [Tooltip("Whether the motion around the x axis is free, limited or locked.")]
        public ConfigurableJointMotion xMotion = ConfigurableJointMotion.Locked;
        [Tooltip("Whether the motion around the y axis is free, limited or locked.")]
        public ConfigurableJointMotion yMotion = ConfigurableJointMotion.Locked;
        [Tooltip("Whether the motion around the z axis is free, limited or locked.")]
        public ConfigurableJointMotion zMotion = ConfigurableJointMotion.Locked;
        [Tooltip("Should the connectedAnchor be calculated automatically?")]
        public bool autoConfigureConnectedAnchor = false;
        [Tooltip("The Vector3 position of the anchor.")]
        public Vector3 anchor = new Vector3(0f, -0.00f, 0.00f);
        [Tooltip("The Vector3 position of the connected anchor.")]
        public Vector3 connectedAnchor = new Vector3(0f, -0.00f, 0.00f);
        [Tooltip("The strength of the dangeling damping.")]
        public float positionDamper = 0.03f;
        [Tooltip("Whether the joint is configured in World Space.")]
        public bool configuredInWorldSpace = false;

        protected override void CreateJoint(GameObject obj)
        {
            givenJoint = obj.AddComponent<ConfigurableJoint>();
            givenJoint.breakForce = (grabbedObjectScript.IsDroppable() ? breakForce : Mathf.Infinity);
            givenJoint.autoConfigureConnectedAnchor = autoConfigureConnectedAnchor;

            if (precisionGrab)
            {
                connectedAnchor = Vector3.zero;
                anchor = obj.transform.InverseTransformPoint(controllerAttachPoint.position);
            }
            
            givenJoint.connectedAnchor = connectedAnchor;
            givenJoint.anchor = anchor;

            base.CreateJoint(obj);
            ConfigureJoint();
        }

        void ConfigureJoint()
        {
            ConfigurableJoint thisJoint = gameObject.GetComponent<ConfigurableJoint>();
            thisJoint.xMotion = xMotion;
            thisJoint.yMotion = yMotion;
            thisJoint.zMotion = zMotion;            
            thisJoint.rotationDriveMode = RotationDriveMode.Slerp;
            JointDrive thisJointDrive = thisJoint.slerpDrive;
            thisJointDrive.positionDamper = positionDamper;
            thisJoint.slerpDrive = thisJointDrive;
            thisJoint.configuredInWorldSpace = configuredInWorldSpace;

            if (precisionButCentered)
            {
                 thisJoint.anchor =  new Vector3 (0f, thisJoint.anchor.y, 0f);
            }
        }
    }

    
}