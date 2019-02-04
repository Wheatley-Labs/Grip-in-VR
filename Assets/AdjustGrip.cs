using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

namespace Valve.VR.InteractionSystem
{
    public class AdjustGrip : MonoBehaviour
    {
        public SteamVR_Action_Single adjustGrip;
        private Hand hand;

        public float defaultDamper = 0.03f;
        private bool isParented = false;

        private void OnEnable()
        {
            if (hand == null)
                hand = this.GetComponent<Hand>();

            if (adjustGrip == null)
            {
                Debug.LogError("<b>[SteamVR Interaction]</b> No adjustGrip action assigned");
                return;
            }

            adjustGrip.AddOnChangeListener(OnAdjustGripActionChange, hand.handType);
        }

        private void OnDisable()
        {
            if (adjustGrip!= null)
                adjustGrip.RemoveOnChangeListener(OnAdjustGripActionChange, hand.handType);
        }

        private void OnAdjustGripActionChange(SteamVR_Action_Single actionIn, SteamVR_Input_Sources inputSource, float newAxis, float newDelta)
        {
            if (hand.GetComponent<ConfigurableJoint>() != null)
            {
                SetGrip(newAxis);
            }
        }

        private void SetGrip(float firmness)
        {
            float damper = defaultDamper;
            if (firmness > 0.3f)
            {
                damper = Map(firmness, 0.3f, 1f, defaultDamper, 0.5f);
            }

            ConfigurableJoint handJoint = hand.GetComponent<ConfigurableJoint>();
            JointDrive handJointDrive = handJoint.slerpDrive;
            handJointDrive.positionDamper = damper;
            handJoint.slerpDrive = handJointDrive;

            if (firmness == 1f)
            {
                //attach or parent the held object
            }

        }

        private void SetParent()
        {
            
        }

        private static float Map(float value, float inputFrom, float inputTo, float outputFrom, float outputTo)
        {
            return ((value - inputFrom) / (inputTo - inputFrom) * (outputTo - outputFrom) + outputFrom);
        }
    }
}