namespace VRTK
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    
    public class HighlightCarafeSurface : MonoBehaviour
    {
        public GameObject tooltipStove;
        private Animator animator;
        public ScoreCounter score;

        private GameObject[] carafes;
        private bool highlightingStopped;

        // Use this for initialization
        void Start()
        {
            carafes = GameObject.FindGameObjectsWithTag("Grabbable");
            highlightingStopped = false;

            animator = GetComponent<Animator>();
            StartHighlighting();
        }

        public void StartHighlighting()
        {
            animator.SetBool("highlight", true);
            tooltipStove.SetActive(true);
        }

        public IEnumerator StopHighlighting()
        {
            animator.SetBool("highlight", false);
            score.GetComponent<Text>().enabled = true;

            yield return new WaitForSeconds(2f);
            animator.enabled = false;
        }

        public IEnumerator StopTooltip()
        {
            tooltipStove.SetActive(false);
            Destroy(GetComponent<HighlightCarafeSurface>());
            yield return null;
        }

        private void Update()
        {
            if (!highlightingStopped)
            {
                foreach (GameObject carafe in carafes)
                {
                    if (carafe.GetComponent<VRTK_InteractableObject>().IsGrabbed())
                    {
                        StartCoroutine(StopHighlighting());
                        highlightingStopped = true;
                        return;
                    }
                }
            }

            if (score.score > 0)
            {
                StartCoroutine(StopTooltip());
            }
        }

        
    }
}