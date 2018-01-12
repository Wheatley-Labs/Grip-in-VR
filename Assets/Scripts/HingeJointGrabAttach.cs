// Hinge Joint Grab Attach|GrabAttachMechanics|50040
// Adapted from Fixed Joint Grab Attach by Michael Bonfert
namespace VRTK.GrabAttachMechanics
{
    using UnityEngine;

    /// <summary>
    /// The Hinge Joint Grab Attach script is used to create a Hinge Joint connection between the object and the grabbing object.
    /// </summary>
    
    [AddComponentMenu("Scripts/HingeJointGrabAttach")]
    public class HingeJointGrabAttach : VRTK_BaseJointGrabAttach
    {
        [Tooltip("Maximum force the joint can withstand before breaking. Infinity means unbreakable.")]
        public float breakForce = Mathf.Infinity;

        protected override void CreateJoint(GameObject obj)
        {
            givenJoint = obj.AddComponent<HingeJoint>();
            givenJoint.breakForce = (grabbedObjectScript.IsDroppable() ? breakForce : Mathf.Infinity);
            base.CreateJoint(obj);
        }
    }
}