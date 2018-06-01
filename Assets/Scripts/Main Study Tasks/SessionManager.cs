using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK.Examples;

public class SessionManager : MonoBehaviour {

    public GameObject continueButton;
    public Material activeMaterial;

    public int error = 0;
    public GameObject failedTooltip;

    // Use this for initialization
    void Start () {
        //DontDestroyOnLoad(this.gameObject);
	}

    private void OnLevelWasLoaded(int level)
    {
        error = 0;
    }

    // Update is called once per frame
    void Update () {
		if (Input.GetKeyDown(KeyCode.C))
        {
            LevelFinished();
        }
	}

    public void LevelFinished()
    {
        continueButton.GetComponent<BoxCollider>().enabled = true;
        continueButton.GetComponent<MeshRenderer>().material = activeMaterial;
    }

    public void LevelFailed()
    {
        failedTooltip.SetActive(true);
    }

    public void AddError()
    {
        error += 1;

        if (error == 3)
            LevelFailed();
    }
}
