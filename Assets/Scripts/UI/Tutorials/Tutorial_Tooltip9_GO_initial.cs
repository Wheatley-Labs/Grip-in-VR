using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK.Examples;

public class Tutorial_Tooltip9_GO_initial : MonoBehaviour
{
    private SessionManager sessionManager;
    private InteractionManager interactionManager;


    // Use this for initialization
    void Start()
    {
        interactionManager = GameObject.FindGameObjectWithTag("ExperimentManager").GetComponent<InteractionManager>();
        sessionManager = GameObject.FindGameObjectWithTag("ExperimentManager").GetComponent<SessionManager>();

        StartCoroutine(NextLevel());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator NextLevel()
    {
        yield return new WaitForSeconds(2f);
        sessionManager.LevelFinished();
    }
}
