namespace VRTK.Examples
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class ButtonVisualization : MonoBehaviour
    {
        public GameObject gripPressedImg;
        public GameObject triggerPressedImg;
        public Slider triggerForceSlider;
        private VRTK_ControllerEvents ContrL;
        private VRTK_ControllerEvents ContrR;

        private float triggerAxisValueL;
        private float triggerAxisValueR;

        // Use this for initialization
        void Start()
        {
            ContrL = GameObject.Find("LeftController").GetComponent<VRTK_ControllerEvents>();
            ContrR = GameObject.Find("RightController").GetComponent< VRTK_ControllerEvents>();

            gripPressedImg.SetActive(false);
            triggerPressedImg.SetActive(false);
            triggerForceSlider.gameObject.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            triggerAxisValueL = ContrL.GetTriggerAxis();
            triggerAxisValueR = ContrR.GetTriggerAxis();
            if (triggerAxisValueL > 0.0f || triggerAxisValueR > 0.0f)
            {
                triggerPressedImg.SetActive(true);
                triggerForceSlider.gameObject.SetActive(true);
                triggerForceSlider.value = Mathf.Max(triggerAxisValueL, triggerAxisValueR);
            }
            else
            {
                triggerPressedImg.SetActive(false);
                triggerForceSlider.gameObject.SetActive(false);
            }

            if (ContrL.gripPressed || ContrR.gripPressed)
            {
                gripPressedImg.SetActive(true);
            }
            else
            {
                gripPressedImg.SetActive(false);
            }
        }
    }
}