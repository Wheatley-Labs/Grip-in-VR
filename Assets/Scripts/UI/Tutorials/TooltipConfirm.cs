    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using VRTK;

    public class TooltipConfirm : VRTK_InteractableObject
    {
        public bool buttonPressed = false;

        public override void StartUsing(VRTK_InteractUse usingObject)
        {
            buttonPressed = true;
        }
    }