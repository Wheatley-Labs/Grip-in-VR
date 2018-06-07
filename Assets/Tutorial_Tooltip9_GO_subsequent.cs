using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK.Examples;

public class Tutorial_Tooltip9_GO_subsequent : MonoBehaviour
{
    public GameObject moreGlasses;
    public GameObject oldGlass;
    public GameObject target;
    public GameObject score;

    private SessionManager sessionManager;
    private InteractionManager interactionManager;


    // Use this for initialization
    void Start()
    {
        interactionManager = GameObject.FindGameObjectWithTag("ExperimentManager").GetComponent<InteractionManager>();
        sessionManager = GameObject.FindGameObjectWithTag("ExperimentManager").GetComponent<SessionManager>();

        sessionManager.LevelFinished();
        StartCoroutine(ChangeGlasses());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator ChangeGlasses()
    {
        yield return new WaitForSeconds(1f);
        oldGlass.SetActive(false);
        target.SetActive(false);
        score.SetActive(false);
        moreGlasses.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        interactionManager.SetMode(interactionManager.currentInteractionMode);
    }
}
