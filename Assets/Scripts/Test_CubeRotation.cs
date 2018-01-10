namespace VRTK.Examples
{
    using UnityEngine;
    using System.Collections;

    public class Test_CubeRotation : VRTK_InteractableObject
    {
        
        public override void StartUsing(VRTK_InteractUse usingObject)
        {
            base.StartUsing(usingObject);
            transform.localScale = new Vector3(1F, 3F, 1F);
            Debug.Log("Object in use, should be bigger");
        }

        protected void Start()
        {
            
        }
        
    }
}