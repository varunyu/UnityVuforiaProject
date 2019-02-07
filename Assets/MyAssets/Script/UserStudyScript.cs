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

    public GameObject timerText;
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
        userstudyUI = (UserStudyUI)gameObject.GetComponent<UserStudyUI>();

        if (currentSystem ==0)
        {
            slidARScript = (SlidARPPController)SlidARPP.GetComponent(typeof(SlidARPPController));
            slidARScript.AnnotationIsBeingSelected += EnableEditModeTimer;

        }
        else
        {
            hybridScript = (HybridController)Hybrid.GetComponent(typeof(HybridController));
            hybridScript.AnnotationIsBeingSelected += EnableEditModeTimer;
            hybridScript.InteractInAuthoringMode += EnableAuthoringModeTimer;
        }
        //timer = 0f;
        //TargetWithEvents.OnTargetAlignedWithAnnotation += TargetObjectIsAlignedWithAnnotation;
        //count = 0;

        //targetListsNumber[listsNumber].SetActive(true);
    }

    public void StartUserStudy(int targetGroup)
    {
        ResetUserStudy();

        listsNumber = targetGroup;
        targetListsNumber[listsNumber].SetActive(true);
        userstudyBegin = true;
        EnableTargetObject(0);
    }

    private void EnableTargetObject(int i)
    {
        if (i != 0)
        {
            currentTarget.GetComponent<TargetWithEvents>().OnTargetAlignedWithAnnotation -= TargetObjectIsAlignedWithAnnotation;
            userstudyUI.ClearAll();
            currentTarget.SetActive(false);
            currentTarget = null;
        }

        currentTarget = targetListsNumber[listsNumber].transform.GetChild(i).gameObject;
        currentTarget.SetActive(true);
        currentTarget.GetComponent<TargetWithEvents>().OnTargetAlignedWithAnnotation += TargetObjectIsAlignedWithAnnotation;
        userstudyUI.RegisterNewListenser(currentTarget);
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
                timerText.GetComponent<Text>().text = editModeTimer.ToString("F2") + " S";

                MeasureDeviceMovement();

            }
            if (authoringModeTimerOn)
            {
                authoringModeTimer += Time.deltaTime;
            }
        }
    }

    private float elapsedTime = 0f;
    private float setElapsedTime = 1;
    private Vector3 prevDevicePos;
    private void MeasureDeviceMovement()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= setElapsedTime)
        {
            if (deviceMovementDistance > 0.0f)
            {
                deviceMovementDistance += Vector3.Distance(prevDevicePos, Camera.main.transform.position);
            }
            prevDevicePos = Camera.main.transform.position;
            elapsedTime = 0;
        }

    }

    public void ResetUserStudy()
    {
       
        editModeTimer = 0f;
        count = 0;
        deviceMovementDistance = 0f;
        authoringModeTimer = 0f;
    }

    private void TargetObjectIsAlignedWithAnnotation()
    {
        Debug.Log("Correct!!!!");
        count++;
        if (count < maxNumberOfTarget)
        {
            EnableTargetObject(count);
        }
    }

    private void EnableAuthoringModeTimer(bool b)
    {
        authoringModeTimerOn = b;
    }

    private void EnableEditModeTimer(bool b)
    {
        editModeTimerOn = b;
    }

}
