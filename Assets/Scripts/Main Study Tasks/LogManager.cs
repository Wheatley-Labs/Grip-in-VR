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
    #region Variables: Filehandling and Paths
    public string testSubject = "XX";
    public int testCycle = 1;

    private static bool logInitialized = false;
    private static string LOGFILE_DIRECTORY;
    private static string LOGFILE_NAME_TIME_FORMAT = "yyyy-MM-dd_HH-mm-ss";	    // prefix of the logfile, created when application starts (year - month - day - hour - minute - second)    

    //The different logfiles
    private static string logFramewise;
    private static string logSingleObjects;
    private static string logTotal;
    #endregion

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

    private Transform targetTransform;
    private bool targetFound = false;
    #endregion

    #region Variables: Measure Times, Distances, Grabs and Grip Adjustment
    private bool taskStarted = false;               //Whether the first object of a task has already been grabbed
    private bool objectFresh = true;                //Whether the object is freshly instantiated or has already been grabbed before
    private bool isGrabbing = false;                //Whether the right controller is currently grabbing a glass
    
    //TIMESTAMPS
    private float ts_startCurrentTask;              //Timestamp of starting current Task, i.e. when grabbing the first object
    private float ts_startGrabbingCurrent;          //Timestamp of initially grabbing the next object after the score increased
    private float ts_latestGrabbingCurrent;         //Timestamp of the latest grab of the current object, potentially after having dropped it in between

    //TIMES
    private float t_handlingCurrent = 0f;           //The accumulated time of holding the current object
    private float t_handlingTask = 0f;              //The accumulated time of handling objects in the current task
    private static float t_handlingTotal = 0f;      //The total accumulated time of holding any object
    private float t_tillScoreCurrent = 0f;          //The total time of finishing the current object from first grab to success
    private float t_tillScoreTask = 0f;             //The total time of finishing the current task from first grab to success
    private static float t_tillScoreTotal = 0f;     //The total time of finishing all tasks from first grab to success

    //GRABBING
    private int gb_grabsCurrent = 0;                //The number of grabs of the same glass before scoring
    private int gb_grabsTask = 0;                   //The number of grabs for all glasses in one task
    private static int gb_grabsTotal = 0;           //The number of grabs for all tasks

    //GRIP
    private bool gp_looseCurrent = false;           //Whether the current object has been held loosely
    private int gp_looseTask = 0;                   //The number of objects that have been held loosely in this task
    private static int gp_looseTotal = 0;           //The number of objects that have been held loosely in total
    private bool gp_firmCurrent = false;            //Whether the current object has been held firmly
    private int gp_firmTask = 0;                    //The number of objects that have been held firmly in this task
    private static int gp_firmTotal = 0;            //The number of objects that have been held firmly in total
    private bool gp_variedCurrent = false;          //Whether the grip has been varied for the currently used glass
    private int gp_variedTask = 0;                  //The number of glasses that the grip has been varied for handling in this task
    private static int gp_variedTotal = 0;          //The number of glasses that the grip has been varied for handling in total

    //DISTANCES
    private float d_handlingCurrent = 0f;           //The distance covered by the controller while handling the current object
    private float d_handlingTask = 0f;              //The distance covered by the controller while handling objects in the current task
    private static float d_handlingTotal = 0f;      //The accumulated distance covered by the controller while handling objects in total
    private float d_tillScoreCurrent = 0f;          //The distance covered by the controller from first grab until scoring for the current object
    private float d_tillScoreTask = 0f;             //The distance covered by the controller from first grab until scoring for the current task
    private static float d_tillScoreTotal = 0f;     //The accumulated distance covered by the controller from first grab until scoring in total
    private Vector3 d_prevPos;                      //The controller's position in the previous frame

    //PRECISION
    private float p_precisionCurrent = 0f;          //How precise the current object has been placed on the target
    private float p_precisionTask = 0f;             //How precise the objects of a task have been placed on the targets
    private static float p_precisionTotal = 0f;     //How precise the objects in total have been placed on the targets

    //ERRORS
    private int e_errorsCurrent = 0;                //The errors, i.e. the number of dropped glasses, when attempting the next score
    private int e_errorsTask = 0;                   //The errors of the current task
    private static int e_errorsTotal = 0;           //The accumulated errors of the cycle
    #endregion

    void Awake () {
        interactionManager = GameObject.FindGameObjectWithTag("ExperimentManager").GetComponent<InteractionManager>();

        //initialize paths
        if (!logInitialized)
        {
            //create path
            LOGFILE_DIRECTORY = Application.dataPath + "/Logs/" + testSubject;

            // check if directory exists (and create it if not)
            if (!Directory.Exists(LOGFILE_DIRECTORY))
                Directory.CreateDirectory(LOGFILE_DIRECTORY);

            logFramewise = CreateLogFile("Framewise");
            logSingleObjects = CreateLogFile("SingleObjects");
            logTotal = CreateLogFile("Total");
        }
    }

    private void Start()
    {
        sessionManager = GameObject.FindGameObjectWithTag("ExperimentManager").GetComponent<SessionManager>();
        sessionManager.OnError += NewError;
        FindCtrlR();
        FindScore();

        //Write the header to the Framewise Log
        if (!logInitialized)
        {
            WriteToLog("Framewise", "Timestamp,Current task,Current mode,Current score,errors,PosX,PosY,PosZ,RotX,RotY,RotZ,Grip pressed,Trigger pressed,Trigger axis,Grabbing object", true);
            WriteToLog("SingleObjects", "Timestamp Finished,Task,Glass#,Time till Score,Handling Time,Distance till Score,Handling Distance,Precision,Errors,Grabs,Loose Grip,Firm Grip,Varied Grip", true);
            WriteToLog("Total", "Task,Time till Score,Handling Time,Distance till Score,Handling Distance,Precision,Errors,Grabs,Loose Grips,Firm Grips,Objects with Varied Grip", true);
            logInitialized = true;
        }
    }

    void Update () {
        #region FRAMEWISE LOGGING & VARIED GRIP COUNT

        //Collect data to log
        if (ctrlRFound)
        {
            if (!scoreFound)
            {
                FindScore();
                currentScore = 0;
            }
            else
            {
                currentScore = score.score;
            }

            if (!targetFound)
            {
                FindTarget();
            }

            //Write data of current state in every frame to the Framewise Log
            WriteToLog("Framewise", Time.time + "," + SceneManager.GetActiveScene().buildIndex + "," + interactionManager.currentInteractionMode + "," +
                        currentScore + "," + sessionManager.error + "," +
                        ctrlR.transform.position.x + "," + ctrlR.transform.position.y + "," + ctrlR.transform.position.z + "," +
                        ctrlR.transform.rotation.x + "," + ctrlR.transform.rotation.y + "," + ctrlR.transform.rotation.z + "," +
                        ctrlREvents.gripPressed + "," + ctrlREvents.triggerTouched + "," + ctrlREvents.GetTriggerAxis() + "," + isGrabbing,
                        true);

            //Determine whether the grip was varied
            if (taskStarted)
            {
                if (!gp_variedCurrent)
                {
                    if (!gp_firmCurrent && isGrabbing && interactionManager.currentInteractionMode != 4)
                        CheckFirmGrip();
                    if (!gp_looseCurrent && isGrabbing && interactionManager.currentInteractionMode != 1)
                        CheckLooseGrip();

                    if(gp_firmCurrent && gp_looseCurrent)
                    {
                        gp_variedCurrent = true;
                        gp_variedTask += 1;
                        gp_variedTotal += 1;
                    }
                }
            }
        }
        else
            FindCtrlR();

        #endregion FRAMEWISE LOGGING

        #region DISTANCE LOGGING

        if (isGrabbing)
        {
            d_handlingCurrent += Vector3.Distance(ctrlR.transform.position, d_prevPos);
        }
        if (!objectFresh)
        {
            d_tillScoreCurrent += Vector3.Distance(ctrlR.transform.position, d_prevPos);
        }

        d_prevPos = ctrlR.transform.position;
        #endregion TIME LOGGING

    }

    private string CreateLogFile(string filename)
    {
        // create files for this session using time prefix based on standard UTC time
        string fullPath = LOGFILE_DIRECTORY
            + "/"
            + DateTime.Now.ToString(LOGFILE_NAME_TIME_FORMAT)
            + "_Cycle" + testCycle
            + "_Mode" + interactionManager.currentInteractionMode
            + "_" + filename
            + ".csv";
        File.Create(fullPath);

        if (File.Exists(fullPath)) Debug.Log("LogFile created at " + fullPath);
        else Debug.LogError("Error creating LogFile" + filename);

        return fullPath;
    }

    public void WriteToLog(string log, string content, bool newLine = false)
    {
        string targetLog = logFramewise;
        switch (log)
        {
            case "Framewise":
                targetLog = logFramewise;
                break;
            case "SingleObjects":
                targetLog = logSingleObjects;
                break;
            case "Total":
                targetLog = logTotal;
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

    private void FindTarget()
    {
        try
        {
            targetTransform = GameObject.FindGameObjectWithTag("Target").GetComponent<Transform>();
            targetFound = true;
        }
        catch
        {
            targetFound = false;
        }
    }

    private void ObjectGrabbed(object sender, ObjectInteractEventArgs e)
    {
        if (ctrlRGrab.GetGrabbedObject().tag == "Grabbable")
        {

            if (!SceneManager.GetActiveScene().name.Contains("Tutorial"))
            {
                if (!taskStarted)
                {
                    taskStarted = true;

                    //Start timer for current task: From first grab to success
                    ts_startCurrentTask = Time.time;
                }

                if (objectFresh)
                {
                    ts_startGrabbingCurrent = Time.time;    //Start initial timer for overall time of current object
                    gb_grabsCurrent = 0;
                    gp_variedCurrent = false;
                    gp_firmCurrent = false;
                    gp_looseCurrent = false;
                    t_handlingCurrent = 0f;
                    d_handlingCurrent = 0f;
                    d_tillScoreCurrent = 0f;
                    e_errorsCurrent = 0;
                    objectFresh = false;
                }

                ts_latestGrabbingCurrent = Time.time;       //Start timer for the latest single grab of current object
                gb_grabsCurrent += 1;
                gb_grabsTask += 1;
                gb_grabsTotal += 1;
            }

            isGrabbing = true;
        }
    }

    private void ObjectUngrabbed(object sender, ObjectInteractEventArgs e)
    {
        t_handlingCurrent += (Time.time - ts_latestGrabbingCurrent);
        isGrabbing = false;
    }

    private void Scored(int newScore, GameObject glass)
    {
        if (!SceneManager.GetActiveScene().name.Contains("Tutorial"))
        {
            t_tillScoreCurrent = (Time.time - ts_startGrabbingCurrent);

            Vector2 finalGlassPos = new Vector2 (glass.transform.position.x, glass.transform.position.z);
            Vector2 targetPos = new Vector2(targetTransform.position.x, targetTransform.position.z);
            p_precisionCurrent = Vector2.Distance(finalGlassPos,targetPos) * 100f;        //in cm

            WriteToLog("SingleObjects",
                       Time.time + "," + (SceneManager.GetActiveScene().buildIndex - 1) + "," + score.score + "," + //TASK & ITEM
                       t_tillScoreCurrent + "," + t_handlingCurrent + "," +                                         //TIMES
                       d_tillScoreCurrent + "," + d_handlingCurrent + "," + p_precisionCurrent + "," +              //DISTANCES & PRECISION
                       e_errorsCurrent + "," + gb_grabsCurrent + "," +                                              //ERRORS & GRABS
                       gp_looseCurrent + "," + gp_firmCurrent + "," + gp_variedCurrent                              //GRIP
                       , true);

            t_handlingTask += t_handlingCurrent;
            t_tillScoreTask += t_tillScoreCurrent;
            d_handlingTask += d_handlingCurrent;
            d_tillScoreTask += d_tillScoreCurrent;
            p_precisionTask += p_precisionCurrent;

            //after every task, log task values
            if (newScore == score.maxScore)
            {
                t_handlingTotal += t_handlingTask;
                t_tillScoreTotal += t_tillScoreTask;
                d_handlingTotal += d_handlingTask;
                d_tillScoreTotal += d_tillScoreTask;
                p_precisionTotal += p_precisionTask;

                WriteToLog("Total",
                            (SceneManager.GetActiveScene().buildIndex - 1) + "," +                  //TASK
                            t_tillScoreTask + "," + t_handlingTask + "," +                          //TIMES
                            d_tillScoreTask + "," + d_handlingTask + "," + p_precisionTask + "," +  //DISTANCES & PRECISION
                            e_errorsTask + "," + gb_grabsTask + "," +                               //ERRORS & GRABS
                            gp_looseTask + "," + gp_firmTask + "," + gp_variedTask                  //GRIP
                            , true);

                //after last task, log total values
                if (SceneManager.GetActiveScene().buildIndex == 5)
                {
                    WriteToLog("Total", "All," +
                                t_tillScoreTotal + "," + t_handlingTotal + "," +                            //TIMES
                                d_tillScoreTotal + "," + d_handlingTotal + "," + p_precisionTotal + "," +   //DISTANCES & PRECISION
                                e_errorsTotal + "," + gb_grabsTotal + "," +                                 //ERRORS & GRABS
                                gp_looseTotal + "," + gp_firmTotal + "," + gp_variedTotal                   //GRIP
                                , true);
                }
            }

            objectFresh = true;
        }
    }

    private void NewError()
    {
        if (!SceneManager.GetActiveScene().name.Contains("Tutorial"))
        {
            e_errorsCurrent += 1;
            e_errorsTask += 1;
            e_errorsTotal += 1;

            if (isGrabbing)
            {
                t_handlingCurrent += (Time.time - ts_latestGrabbingCurrent);
                isGrabbing = false;
            }
        }
    }

    #region Checking Grip Status

    private void CheckFirmGrip()
    {
        switch (interactionManager.currentInteractionMode)
        {
            case 1:
                gp_firmCurrent = true;
                gp_firmTask += 1;
                gp_firmTotal += 1;
                break;
            case 2:
                if (ctrlREvents.gripPressed)
                    StartCoroutine(GripStillPressed());
                break;
            case 3:
                if (ctrlREvents.GetTriggerAxis() == 1f)
                    StartCoroutine(TriggerStillClicked());
                break;
        }
    }

    private void CheckLooseGrip()
    {
        switch (interactionManager.currentInteractionMode)
        {
            case 2:
                if (!ctrlREvents.gripPressed)
                    StartCoroutine(GripStillNotPressed());
                break;
            case 3:
                if (ctrlREvents.GetTriggerAxis() < 0.9f)
                    StartCoroutine(TriggerStillSemi());
                break;
            case 4:
                gp_looseCurrent = true;
                gp_looseTask += 1;
                gp_looseTotal += 1;
                break;
        }
    }

    IEnumerator GripStillPressed()
    {
        yield return new WaitForSeconds(0.2f);
        if (ctrlREvents.gripPressed && isGrabbing && !gp_firmCurrent)
        {
            gp_firmCurrent = true;
            gp_firmTask += 1;
            gp_firmTotal += 1;
        }
    }

    IEnumerator TriggerStillClicked()
    {
        yield return new WaitForSeconds(0.2f);
        if (ctrlREvents.GetTriggerAxis() == 1f && isGrabbing && !gp_firmCurrent)
        {
            gp_firmCurrent = true;
            gp_firmTask += 1;
            gp_firmTotal += 1;
        }
    }

    IEnumerator GripStillNotPressed()
    {
        yield return new WaitForSeconds(0.2f);
        if (!ctrlREvents.gripPressed && isGrabbing && !gp_looseCurrent)
        {
            gp_looseCurrent = true;
            gp_looseTask += 1;
            gp_looseTotal += 1;
        }
    }

    IEnumerator TriggerStillSemi()
    {
        yield return new WaitForSeconds(0.2f);
        if (ctrlREvents.GetTriggerAxis() < 0.9f && isGrabbing && !gp_looseCurrent)
        {
            gp_looseCurrent = true;
            gp_looseTask += 1;
            gp_looseTotal += 1;
        }
    }

    #endregion
}   
