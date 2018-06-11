namespace VRTK
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    
    public class HighlightCupboardTask3 : MonoBehaviour
    {
        private Animator animator;
        public GameObject otherDoor;
        private Animator otherAnimator;
        public GameObject TooltipCupboard;
        public GameObject firstObject;

        // Use this for initialization
        void Start()
        {
            animator = GetComponent<Animator>();
            otherAnimator = otherDoor.GetComponent<Animator>();
        }

        public void StartHighlighting()
        {
            TooltipCupboard.SetActive(true);
        }

        public IEnumerator StopHighlighting()
        {
            TooltipCupboard.SetActive(false);
            Destroy(GetComponent<HighlightCupboardTask3>());
            yield return null;
        }
        private void Update()
        {
            if (firstObject.GetComponent<VRTK_InteractableObject>().IsGrabbed())
            {
                StartCoroutine("StopHighlighting");
            }
        }
    }
}