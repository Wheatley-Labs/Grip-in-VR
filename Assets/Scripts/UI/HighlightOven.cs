namespace VRTK
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    
    public class HighlightOven : MonoBehaviour
    {
        public GameObject tooltipOven;
        public Text scoreText;
        private Animator animator;
        public GameObject cupboardHighlighter;

        // Use this for initialization
        void Start()
        {
            animator = GetComponent<Animator>();
            StartHighlighting();
        }

        public void StartHighlighting()
        {
            animator.SetBool("highlight", true);
            tooltipOven.SetActive(true);
        }

        public IEnumerator StopHighlighting()
        {
            animator.SetBool("highlight", false);
            scoreText.enabled = true;
            yield return new WaitForSeconds(0.1f);
            animator.enabled = false;
            tooltipOven.SetActive(false);
            cupboardHighlighter.GetComponent<HighlightCupboardTask3>().StartHighlighting();

            yield return new WaitForSeconds(0.1f);
            Destroy(GetComponent<HighlightOven>());
        }

        private void Update()
        {
            if (gameObject.transform.localEulerAngles.x > 320)
            {
                StartCoroutine("StopHighlighting");
            }
        }
    }
}