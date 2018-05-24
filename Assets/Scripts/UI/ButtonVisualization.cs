namespace VRTK.Examples
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class ButtonVisualization : MonoBehaviour
    {
        private GameObject gripPressed;
        private GameObject triggerPressed;

        // Use this for initialization
        void Start()
        {
            gripPressed = GameObject.Find("Grip Pressed");
            triggerPressed = GameObject.Find("Trigger Pressed");
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}