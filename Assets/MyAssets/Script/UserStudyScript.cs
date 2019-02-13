using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserStudyScript: MonoBehaviour
{
    [SerializeField]
    private int count;
    [SerializeField]
    private int maxNumberOfTarget;

    private bool editModeTimerOn;
    private float editModeTimer;

    private bool authoringModeTimerOn;
    private float authoringModeTimer;

    private float deviceMovementDistance;

    public GameObject SlidARPP;
    public GameObject Hybrid;
    private SlidARPPController slidARScript;
    private HybridController hybridScript;
    private UserStudyUI userstudyUI;
    private DDAS dDAS;

    public GameObject edit_Mode_timerText;
    public GameObject authoring_Mode_timerText;
    public GameObject device_Mov_DisText;
    // slidAR = 0;
    // hybrid = 1;
    [SerializeField]
    private int currentSystem;
    // Start is called before the first frame update

    [SerializeField]
    private GameObject[] targetListsNumber;

    [SerializeField]
    private int listsNumber;

    public bool userstudyBegin;
    private GameObject currentTarget;

    void Start()
    {

        currentSystem = PlayerPrefs.GetInt("ChosenSystem");
        //targetGroup = PlayerPrefs.GetInt("TargetGroup");
        listsNumber = PlayerPrefs.GetInt("TargetGroup");

        Debug.Log("System "+currentSystem);
        Debug.Log("TargetList "+listsNumber);


        userstudyUI = (UserStudyUI)gameObject.GetComponent<UserStudyUI>();
        dDAS = (DDAS)gameObject.GetComponent<DDAS>();

        if (currentSystem ==0)
        {
            SlidARPP.SetActive(true);
            slidARScript = (SlidARPPController)SlidARPP.GetComponent(typeof(SlidARPPController));
            slidARScript.AnnotationIsBeingSelected += EnableEditModeTimer;
            slidARScript.InteractInAuthoringMode += EnableAuthoringModeTimer;
        }
        else
        {
            Hybrid.SetActive(true);
            hybridScript = (HybridController)Hybrid.GetComponent(typeof(HybridController));
            hybridScript.AnnotationIsBeingSelected += EnableEditModeTimer;
            hybridScript.InteractInAuthoringMode += EnableAuthoringModeTimer;
        }
        //timer = 0f;
        //TargetWithEvents.OnTargetAlignedWithAnnotation += TargetObjectIsAlignedWithAnnotation;
        //count = 0;

        //targetListsNumber[listsNumber].SetActive(true);
        //dDAS.SetupData(maxNumberOfTarget);
        //StartUserStudy(0);
    }

    public void StartUserStudy()
    {
        SetupUserStudyData();
    }

    public void SetupUserStudyData()
    {
        ResetUserStudy();


        targetListsNumber[listsNumber].SetActive(true);



        maxNumberOfTarget = targetListsNumber[listsNumber].transform.childCount - 1;
        dDAS.SetupData(maxNumberOfTarget);
        EnableTargetObject(0);
        userstudyBegin = true;
    }

    private void EnableTargetObject(int i)
    {
        if (i != 0)
        {
            DisableCurrentTarget();
        }

        currentTarget = targetListsNumber[listsNumber].transform.GetChild(i).gameObject;
        currentTarget.SetActive(true);
        currentTarget.GetComponent<TargetWithEvents>().OnTargetAlignedWithAnnotation += TargetObjectIsAlignedWithAnnotation;
        userstudyUI.RegisterNewListenser(currentTarget);
    }

    private void DisableCurrentTarget()
    {
        currentTarget.GetComponent<TargetWithEvents>().OnTargetAlignedWithAnnotation -= TargetObjectIsAlignedWithAnnotation;
        userstudyUI.ClearAll();
        currentTarget.SetActive(false);
        currentTarget = null;
    }


    // Update is called once per frame
    void Update()
    {

    }
    private void FixedUpdate()
    {
        if (userstudyBegin)
        {
            RunningUserStudy();
        }
    }

    private void RunningUserStudy()
    {
        if (count < maxNumberOfTarget)
        {
            if (editModeTimerOn)
            {
                editModeTimer += Time.deltaTime;
                edit_Mode_timerText.GetComponent<Text>().text = editModeTimer.ToString("F2") + " S";

                MeasureDeviceMovement();

            }
            if (authoringModeTimerOn)
            {
                authoringModeTimer += Time.deltaTime;
                authoring_Mode_timerText.GetComponent<Text>().text = authoringModeTimer.ToString("F2") + " S";
            }
        }
    }

    private float elapsedTime = 0f;
    private float setElapsedTime = 0.5f;
    private Vector3 prevDevicePos;
    private void MeasureDeviceMovement()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= setElapsedTime)
        {
            if(prevDevicePos != null || (Vector3.Distance(prevDevicePos, Camera.main.transform.position)>=1f))
            {
                deviceMovementDistance += Vector3.Distance(prevDevicePos, Camera.main.transform.position);
            }
            /*
            if (deviceMovementDistance > 0.0f)
            {
                deviceMovementDistance += Vector3.Distance(prevDevicePos, Camera.main.transform.position);
            }*/
            prevDevicePos = Camera.main.transform.position;
            elapsedTime = 0;
            device_Mov_DisText.GetComponent<Text>().text = deviceMovementDistance.ToString("F2") + " cm";
        }

    }

    public void ResetUserStudy()
    {
        count = 0;
        editModeTimer = 0f;
        deviceMovementDistance = 0f;
        authoringModeTimer = 0f;
    }

    public void TargetObjectIsAlignedWithAnnotation()
    {
        //Debug.Log("Correct!!!!");


        SaveCurrentData();
        editModeTimer = 0f;
        deviceMovementDistance = 0f;
        authoringModeTimer = 0f;
        EnableEditModeTimer(false);
        /*
        count++;
        if (count < maxNumberOfTarget)
        {
            EnableTargetObject(count);
        }*/
        if (!IsFinish())
        {

            EnableTargetObject(count);
        }
        else
        {
            Debug.Log("Finish!!!!");
            DisableCurrentTarget();
            dDAS.UploadDataToInternet();
            userstudyUI.ShowFinishTesxt(true);
        }
    }

    private bool IsFinish()
    {
        count++;
        if(count+1 <= maxNumberOfTarget)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void SaveCurrentData()
    {
        dDAS.SaveTrialData(authoringModeTimer, editModeTimer, deviceMovementDistance,count);
    }
    

    public void EnableAuthoringModeTimer(bool b)
    {
        authoringModeTimerOn = b;
    }

    public void EnableEditModeTimer(bool b)
    {
        Debug.Log("EditModeTimer "+ b );
        editModeTimerOn = b;
    }


}
