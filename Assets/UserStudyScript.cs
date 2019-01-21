using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserStudyScript: MonoBehaviour
{
    private int count;
    private bool timerOn;
    private float timer;

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
            slidARScript.AnnotationIsBeingSelected += EnableTimer;
        }
        else
        {
            hybridScript = (HybridController)Hybrid.GetComponent(typeof(HybridController));
            hybridScript.AnnotationIsBeingSelected += EnableTimer;
        }
        //timer = 0f;
        //TargetWithEvents.OnTargetAlignedWithAnnotation += TargetObjectIsAlignedWithAnnotation;
        //count = 0;

        //targetListsNumber[listsNumber].SetActive(true);
    }

    public void StartUserStudy(int targetGroup)
    {
        timer = 0f;
        count = 0;
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
        if (userstudyBegin)
        {
            RunningUserStudy();
        }
    }

    private void RunningUserStudy()
    {
        if (count < 5)
        {
            if (timerOn)
            {
                timer += Time.deltaTime;

                timerText.GetComponent<Text>().text = timer.ToString("F2") + " S";
            }
        }
    }

    public void ResetUserStudy()
    {
        count = 0;
        timer = 0f;
    }

    private void TargetObjectIsAlignedWithAnnotation()
    {
        Debug.Log("Correct!!!!");
        count++;
        if (count < 5)
        {
            EnableTargetObject(count);
        }
    }

    private void EnableTimer(bool b)
    {
        timerOn = b;
    }

}
