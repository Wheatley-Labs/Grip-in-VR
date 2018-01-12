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
        public ConfigurableJointMotion xMotion = ConfigurableJointMotion.Free;
        [Tooltip("Whether the motion around the y axis is free, limited or locked.")]
        public ConfigurableJointMotion yMotion = ConfigurableJointMotion.Free;
        [Tooltip("Whether the motion around the z axis is free, limited or locked.")]
        public ConfigurableJointMotion zMotion = ConfigurableJointMotion.Free;
        [Tooltip("Should the connectedAnchor be calculated automatically?")]
        public bool autoConfigureConnectedAnchor = false;
        [Tooltip("The Vector3 position of the connected anchor.")]
        public Vector3 connectedAnchor = new Vector3(0f, -0.06f, 0.05f);

        protected override void CreateJoint(GameObject obj)
        {
            givenJoint = obj.AddComponent<ConfigurableJoint>();
            givenJoint.breakForce = (grabbedObjectScript.IsDroppable() ? breakForce : Mathf.Infinity);
            givenJoint.autoConfigureConnectedAnchor = autoConfigureConnectedAnchor;
            givenJoint.connectedAnchor = connectedAnchor;
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
            //thisJoint.slerpDrive.positionDamper = 0.08f;
            //To Do: figure out how this value can be set!
        }
    }

    
}