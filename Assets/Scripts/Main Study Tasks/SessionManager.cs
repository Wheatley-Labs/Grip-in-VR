using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK.Examples;
using VRTK;
using UnityEngine.SceneManagement;
using System;

public class SessionManager : MonoBehaviour {

    public int error = 0;
    public bool levelFinished = false;

    public GameObject CtrlR;

    public delegate void errorEventHandler();
    public event errorEventHandler OnError;

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

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(ReloadAsyncThisScene());
        }

        if (CtrlR == null)
        {
            try
            {
                CtrlR = GameObject.FindGameObjectWithTag("Right Controller");
            }
            catch (Exception e)
            {
                Debug.LogException(e, this);
            }
        }

        if (levelFinished)
        {
            if (CtrlR.GetComponent<VRTK_ControllerEvents>().touchpadPressed)
            {
                StartCoroutine(LoadAsyncScene());
            }
        }
    }

    public void LevelFinished()
    {
        levelFinished = true;

        CtrlR.GetComponentInChildren<VRTK_ControllerTooltips>().UpdateText(VRTK_ControllerTooltips.TooltipButtons.TouchpadTooltip, "Touchpad drücken\nzum Fortfahren");
        CtrlR.GetComponent<VRTK_ControllerHighlighter>().highlightTouchpad = Color.cyan;
    }

    //public void LevelFailed()
    //{
    //    failedTooltip.SetActive(true);
    //}

    public void AddError()  
    {
        error += 1;

        if (OnError != null)
        {
            OnError();
        }
    }

    IEnumerator ReloadAsyncThisScene()
    {
        AsyncOperation loadState = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        while (!loadState.isDone)
        {
            yield return null;
        }
    }

    IEnumerator LoadAsyncScene()
    {
        if (SceneManager.GetActiveScene().name.Contains("Tutorial"))
        {
            AsyncOperation loadState = SceneManager.LoadSceneAsync("Main-Study_Task1");

            while (!loadState.isDone)
            {
                yield return null;
            }
        }

        else
        {
            int currentScene = SceneManager.GetActiveScene().buildIndex;
            AsyncOperation loadState = SceneManager.LoadSceneAsync(currentScene + 1);

            while (!loadState.isDone)
            {
                yield return null;
            }
        }
    }
}
