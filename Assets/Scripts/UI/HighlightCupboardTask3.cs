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

        // Use this for initialization
        void Start()
        {
            animator = GetComponent<Animator>();
            otherAnimator = otherDoor.GetComponent<Animator>();
        }

        public void StartHighlighting()
        {
            TooltipCupboard.SetActive(true);

            animator.SetBool("highlight", true);
            otherAnimator.SetBool("highlight", true);
        }

        public IEnumerator StopHighlighting()
        {
            animator.SetBool("highlight", false);
            otherAnimator.SetBool("highlight", false);
            TooltipCupboard.SetActive(false);

            yield return new WaitForSeconds(2f);
            animator.enabled = false;
            otherAnimator.enabled = false;
            
            Destroy(GetComponent<HighlightCupboardTask3>());
        }
        private void Update()
        {
            if (transform.localEulerAngles.y > 60 && otherDoor.transform.localEulerAngles.y < 290 && otherDoor.transform.localEulerAngles.y > 100)
            {
                StartCoroutine("StopHighlighting");
            }
        }
    }
}