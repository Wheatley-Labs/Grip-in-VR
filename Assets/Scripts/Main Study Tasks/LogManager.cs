using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using VRTK;
using VRTK.Examples;
using UnityEngine.SceneManagement;

/*
 * partially used code from Nico Reski:
 * LoggingSystem.cs
 * Project: Log2CSV - Simple Logging System for Unity applications
 * https://github.com/nicoversity/unity_log2csv/blob/master/LoggingSystem.cs
 */

public class LogManager : MonoBehaviour {
    public string testSubject = "XX";

    private static bool logInitialized = false;
    private static string LOGFILE_DIRECTORY;
    private static string LOGFILE_NAME_TIME_FORMAT = "yyyy-MM-dd_HH-mm-ss";	// prefix of the logfile, created when application starts (year - month - day - hour - minute - second)    

    //The different logfiles
    private static string logOverall;
    private static string logSingleTimes;
    private static string logTotalTimes;
    private static string logDistance;
    private static string logErrors;

    #region References to logged GameObjects
    private SessionManager sessionManager;
    private InteractionManager interactionManager;
    private GameObject ctrlR;
    private bool ctrlRFound = false;
    private VRTK_ControllerEvents ctrlREvents;
    private VRTK_InteractGrab ctrlRGrab;
    private ScoreCounter score;
    private bool scoreFound = false;
    private int currentScore;
    #endregion

    #region Variables: Measure Times, Grabs and Grip Adjustment
    //all times exclude the tutorials
    private static float ts_startCycle;             //Timestamp of loading the scene of Task 1
    private static float ts_startTask1;             //Timestamp of initially starting the tasks, i.e. when grabbing the first object of task 1
    private float ts_startCurrentTask;              //Timestamp of starting current Task, i.e. when grabbing the first object
    private float ts_startGrabbingCurrent;          //Timestamp of initially grabbing the next object after the score increased
    private float ts_latestGrabbingCurrent;         //Timestamp of the latest grab of the current object, potentially after having dropped it in between
    private bool taskStarted = false;               //Whether the first object of a task has already been grabbed
    private bool objectFresh = true;                //Whether the object is freshly instantiated or has already been grabbed before

    private float t_currentGrabbing = 0f;           //The accumulated time of holding the current object
    private static float t_totalGrabbing = 0f;      //The total accumulated time of holding any object
    private static float t_allTasks = 0f;           //The total time of finishing the tasks from first grab to success

    private bool isGrabbing = false;                //Whether the right controller is currently grabbing a glass
    private int g_grabsCurrent = 0;                 //The number of grabs of the same glass before scoring
    private int g_currentTaskGrabs = 0;             //The number of grabs for all glasses in one task
    private static int g_totalGrabs = 0;            //The number of grabs for all tasks

    private bool g_looseGripCurrent = false;        //Whether the current object has been held loosely
    private bool g_firmGripCurrent = false;         //Whether the current object has been held firmly
    private bool g_variedGripCurrent = false;       //Whether the grip has been varied for the currently used glass
    private int g_currentTaskVariedGrip = 0;        //The number of glasses that the grip has been varied for handling in this task
    private static int g_totalVariedGrip = 0;       //The number of glasses that the grip has been varied for handling in total
    #endregion

    void Awake () {
        //initialize paths
        if (!logInitialized)
        {
            //create path
            LOGFILE_DIRECTORY = Application.dataPath + "/Logs/" + testSubject;

            // check if directory exists (and create it if not)
            if (!Directory.Exists(LOGFILE_DIRECTORY))
                Directory.CreateDirectory(LOGFILE_DIRECTORY);

            logOverall = CreateLogFile("Overall");
            logSingleTimes = CreateLogFile("SingleTimes");
            logTotalTimes = CreateLogFile("TotalTimes");
            logDistance = CreateLogFile("Distance");
            logErrors = CreateLogFile("Errors");
        }
    }

    private void Start()
    {
        sessionManager = GameObject.FindGameObjectWithTag("ExperimentManager").GetComponent<SessionManager>();
        interactionManager = GameObject.FindGameObjectWithTag("ExperimentManager").GetComponent<InteractionManager>();
        FindCtrlR();
        FindScore();

        //Write the header to the Overall Log
        if (!logInitialized)
        {
            WriteToLog("Overall", "Timestamp,Current task,Current mode,Current score,errors,PosX,PosY,PosZ,RotX,RotY,RotZ,Grip pressed,Trigger pressed,Trigger axis,Grabbing object", true);
            WriteToLog("SingleTimes", "Task,Glass#,Time till Score,Handling Time,Grabs,Varied Grip", true);
            logInitialized = true;
        }

        if (SceneManager.GetActiveScene().name.Contains("Task1"))
        {
            ts_startCycle = Time.time;
        }
    }

	void Update () {
        #region OVERALL LOGGING

        //Collect data to log
        if (ctrlRFound)
        {
            if (!scoreFound)
            {
                FindScore();
                currentScore = 0;
            }
            else
                currentScore = score.score;

            //Write data of current state in every frame to the Overall Log
            WriteToLog("Overall", Time.time + "," + SceneManager.GetActiveScene().buildIndex + "," + interactionManager.currentInteractionMode + "," +
                        currentScore + "," + sessionManager.error + "," +
                        ctrlR.transform.position.x + "," + ctrlR.transform.position.y + "," + ctrlR.transform.position.z + "," +
                        ctrlR.transform.rotation.x + "," + ctrlR.transform.rotation.y + "," + ctrlR.transform.rotation.z + "," +
                        ctrlREvents.gripPressed + "," + ctrlREvents.triggerTouched + "," + ctrlREvents.GetTriggerAxis() + "," + isGrabbing,
                        true);

            //Determine whether the grip was varied
            if (taskStarted && (interactionManager.currentInteractionMode == 2 || interactionManager.currentInteractionMode == 3))
            {
                if (!g_variedGripCurrent)
                {
                    if (!g_firmGripCurrent && isGrabbing)
                        CheckFirmGrip();
                    if (!g_looseGripCurrent && isGrabbing)
                        CheckLooseGrip();

                    if(g_firmGripCurrent && g_looseGripCurrent)
                    {
                        g_variedGripCurrent = true;
                        g_currentTaskVariedGrip += 1;
                        g_totalVariedGrip += 1;
                    }
                }
            }
        }
        else
            FindCtrlR();
        #endregion OVERALL LOGGING

        #region TIME LOGGING
        /////////////////////////////////////////////
        #endregion TIME LOGGING

    }
    private string CreateLogFile(string filename)
    {
        // create files for this session using time prefix based on standard UTC time
        string fullPath = LOGFILE_DIRECTORY
            + "/"
            + System.DateTime.Now.ToString(LOGFILE_NAME_TIME_FORMAT)
            + "_"
            + filename
            + ".csv";
        File.Create(fullPath);

        if (File.Exists(fullPath)) Debug.Log("LogFile created at " + fullPath);
        else Debug.LogError("Error creating LogFile" + filename);

        return fullPath;
    }

    public void WriteToLog(string log, string content, bool newLine = false)
    {
        string targetLog = logOverall;
        switch (log)
        {
            case "Overall":
                targetLog = logOverall;
                break;
            case "SingleTimes":
                targetLog = logSingleTimes;
                break;
            case "TotalTimes":
                targetLog = logTotalTimes;
                break;
            case "Distance":
                targetLog = logDistance;
                break;
            case "Errors":
                targetLog = logErrors;
                break;
        }

        if (File.Exists(targetLog))
        {
            using (StreamWriter sw = new StreamWriter(targetLog, true))
            {
                if (newLine)
                    sw.WriteLine(content);
                else
                    sw.Write(content);
                sw.Flush();
            }
        }
        else
            Debug.Log("Error writing to log. File does not exist: " + targetLog);
    }

    private void FindCtrlR()
    {
        try
        {
            ctrlR = GameObject.FindGameObjectWithTag("Right Controller");
            ctrlREvents = ctrlR.GetComponent<VRTK_ControllerEvents>();
            ctrlRGrab = ctrlR.GetComponent<VRTK_InteractGrab>();
            ctrlRFound = true;

            ctrlRGrab.ControllerStartGrabInteractableObject += ObjectGrabbed;
            ctrlRGrab.ControllerStartUngrabInteractableObject += ObjectUngrabbed;
        }
        catch
        {
            ctrlRFound = false;
        }
    }

    private void FindScore()
    {
        try
        {
            score = GameObject.FindGameObjectWithTag("Score").GetComponent<ScoreCounter>();
            scoreFound = true;

            score.OnScore += Scored;
        }
        catch (NullReferenceException ex)
        {
            scoreFound = false;
        }
    }

    private void ObjectGrabbed(object sender, ObjectInteractEventArgs e)
    {
        if (ctrlRGrab.GetGrabbedObject().tag == "Grabbable")
        {
            isGrabbing = true;

            if (!SceneManager.GetActiveScene().name.Contains("Tutorial"))
            {
                if (!taskStarted)
                {
                    taskStarted = true;

                    //Start timer for current task: From first grab to success
                    ts_startCurrentTask = Time.time;

                    if (SceneManager.GetActiveScene().buildIndex == 2)
                    {   //Additional timer for Task 1
                        ts_startTask1 = Time.time;
                    }
                }

                if (objectFresh)
                {
                    ts_startGrabbingCurrent = Time.time;    //Start initial timer for overall time of current object
                    g_grabsCurrent = 0;
                    g_variedGripCurrent = false;
                    g_firmGripCurrent = false;
                    g_looseGripCurrent = false;
                    t_currentGrabbing = 0f;
                    objectFresh = false;
                }

                ts_latestGrabbingCurrent = Time.time;       //Start timer for the latest single grab of current object
                g_grabsCurrent += 1;
                g_currentTaskGrabs += 1;
                g_totalGrabs += 1;
            }
        }
    }

    private void ObjectUngrabbed(object sender, ObjectInteractEventArgs e)
    {
        t_currentGrabbing += (Time.time - ts_latestGrabbingCurrent);
        isGrabbing = false;
    }

    private void Scored()
    {
        WriteToLog("SingleTimes",
                   (SceneManager.GetActiveScene().buildIndex - 1) + "," + score.score + "," +
                   (Time.time - ts_startGrabbingCurrent) + ","  + t_currentGrabbing + "," + 
                   g_grabsCurrent + "," + g_variedGripCurrent
                   , true);
        Debug.Log((SceneManager.GetActiveScene().buildIndex - 1) + "," + score.score + "," +
                   (Time.time - ts_startGrabbingCurrent) + "," + t_currentGrabbing + "," +
                   g_grabsCurrent + "," + g_variedGripCurrent);
        objectFresh = true;
    }

    #region Checking Grip Status

    private void CheckFirmGrip()
    {
        if (interactionManager.currentInteractionMode == 2)
        {
            if (ctrlREvents.gripPressed)
                StartCoroutine(GripStillPressed());
        }

        else if (interactionManager.currentInteractionMode == 3)
            if (ctrlREvents.GetTriggerAxis() == 1f)
                StartCoroutine(TriggerStillClicked());
    }

    private void CheckLooseGrip()
    {
        if (interactionManager.currentInteractionMode == 2)
        {
            if (!ctrlREvents.gripPressed)
                StartCoroutine(GripStillNotPressed());
        }

        if (interactionManager.currentInteractionMode == 3)
            if (ctrlREvents.GetTriggerAxis() < 0.9f)
                StartCoroutine(TriggerStillSemi());
    }

    IEnumerator GripStillPressed()
    {
        yield return new WaitForSeconds(0.2f);
        if (ctrlREvents.gripPressed && isGrabbing)
            g_firmGripCurrent = true;
    }

    IEnumerator TriggerStillClicked()
    {
        yield return new WaitForSeconds(0.2f);
        if (ctrlREvents.GetTriggerAxis() == 1f && isGrabbing)
            g_firmGripCurrent = true;
    }

    IEnumerator GripStillNotPressed()
    {
        yield return new WaitForSeconds(0.2f);
        if (!ctrlREvents.gripPressed && isGrabbing)
            g_looseGripCurrent = true;
    }

    IEnumerator TriggerStillSemi()
    {
        yield return new WaitForSeconds(0.2f);
        if (ctrlREvents.GetTriggerAxis() < 0.9f && isGrabbing)
            g_looseGripCurrent = true;
    }

    #endregion
}

/* DON'T FORGET
 * 
 * 
 * */
