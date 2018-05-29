namespace VRTK
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    
    public class HighlightCupSurface : MonoBehaviour
    {
        public GameObject tooltipCupSurface;
        private Animator animator;
        public ScoreCounter score;

        private GameObject[] cups;
        private bool highlightingStopped;

        // Use this for initialization
        void Start()
        {
            cups = GameObject.FindGameObjectsWithTag("Grabbable");
            highlightingStopped = false;

            animator = GetComponent<Animator>();
            StartHighlighting();
        }

        public void StartHighlighting()
        {
            animator.SetBool("highlight", true);
            tooltipCupSurface.SetActive(true);
        }

        public IEnumerator StopHighlighting()
        {
            animator.SetBool("highlight", false);

            yield return new WaitForSeconds(2f);
            animator.enabled = false;
        }

        public IEnumerator StopTooltip()
        {
            tooltipCupSurface.SetActive(false);
            Destroy(GetComponent<HighlightCupSurface>());
            yield return null;
        }

        private void Update()
        {
            if (!highlightingStopped)
            {
                foreach (GameObject cup in cups)
                {
                    if (cup.GetComponent<VRTK_InteractableObject>().IsGrabbed())
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