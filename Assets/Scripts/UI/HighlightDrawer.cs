namespace VRTK
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    
    public class HighlightDrawer : MonoBehaviour
    {
        public GameObject tooltipDrawer;
        public Text scoreText;
        private Animator animator;

        // Use this for initialization
        void Start()
        {
            animator = GetComponent<Animator>();
        }

        public void StartHighlighting()
        {
            animator.SetBool("highlight", true);
            tooltipDrawer.SetActive(true);
        }

        public IEnumerator StopHighlighting()
        {
            animator.SetBool("highlight", false);
            scoreText.enabled = true;

            yield return new WaitForSeconds(2f);
            animator.enabled = false;
            tooltipDrawer.SetActive(false);
            Destroy(GetComponent<HighlightDrawer>());
        }

        private void Update()
        {
            if (transform.localPosition.y < -4.5f)
            {
                StartCoroutine("StopHighlighting");
            }
        }
    }
}