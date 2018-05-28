namespace VRTK
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    
    public class HighlightCupboard : MonoBehaviour
    {
        private Animator animator;
        public GameObject otherDoor;
        private Animator otherAnimator;
        public HighlightDrawer drawerHighlighter;
        public GameObject TooltipCupboard;

        // Use this for initialization
        void Start()
        {
            animator = GetComponent<Animator>();
            otherAnimator = otherDoor.GetComponent<Animator>();

            StartHighlighting();
        }

        public void StartHighlighting()
        {
            animator.SetBool("highlight", true);
            otherAnimator.SetBool("highlight", true);
        }

        public IEnumerator StopHighlighting()
        {
            animator.SetBool("highlight", false);
            otherAnimator.SetBool("highlight", false);
            TooltipCupboard.SetActive(false);
            drawerHighlighter.StartHighlighting();

            yield return new WaitForSeconds(2f);
            animator.enabled = false;
            otherAnimator.enabled = false;
            
            Destroy(otherDoor.GetComponent<HighlightCupboard>());
            Destroy(GetComponent<HighlightCupboard>());
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