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
    private static float t_startCycle;              //Timestamp of loading the scene of Task 1
    private static float t_startTask1;              //Timestamp of starting Task when i.e. grabbing the first object
    private float t_startTask2;
    private float t_startTask3;
    private float t_startTask4;
    private bool t_taskStarted = false;             //Whether the first object of a task has already been grabbed
    private float t_startGrabbingCurrent;           //Timestamp of grabbing the next object after the score increased

    private static float t_totalGrabbing = 0f;      //The total accumulated time of holding any object
    private static float t_allTasks = 0f;           //The total time of finishing the tasks from first grab to success

    private int g_grabsCurrent = 0;                 //The number of grabs of the same glass before scoring
    private int g_currentTaskGrabs = 0;             //The number of grabs for all glasses in one task
    private static int g_totalGrabs = 0;            //The number of grabs for all tasks

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
            WriteToLog("SingleTimes", "Task,Glass#,Time,Grabs,Varied Grip", true);
            logInitialized = true;
        }

        if (SceneManager.GetActiveScene().name.Contains("Task1"))
        {
            t_startCycle = Time.time;
        }
    }

	void Update () {
        #region OVERALL LOGGING

        //Collect data to log
        if (ctrlRFound)
        {
            bool isGrabbing = false;
            if (ctrlR.GetComponent<VRTK_InteractGrab>().GetGrabbedObject() != null)
                isGrabbing = true;
            else
                isGrabbing = false;

            if (!scoreFound)
            {
                FindScore();
                currentScore = 99;
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
            if (!t_taskStarted)
            {
                t_startTask1 = Time.time;           ///////////LEFT OFF HERE... DOES THIS MAKE SENSE?
                t_taskStarted = true;
            }
        }
    }

    private void ObjectUngrabbed(object sender, ObjectInteractEventArgs e)
    {
        
    }
}
