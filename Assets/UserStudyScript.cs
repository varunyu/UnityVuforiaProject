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

    public GameObject timerText;
    // slidAR = 0;
    // hybrid = 1;
    public int currentSystem;
    // Start is called before the first frame update

    

    void Start()
    {

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
        timer = 0f;
        TargetWithEvents.OnTargetAlignedWithAnnotation += TargetObjectIsAlignedWithAnnotation;
        count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(count < 5)
        {
            if (timerOn)
            {
                timer += Time.deltaTime;

                timerText.GetComponent<Text>().text = timer.ToString()+ " S";
            }
        }
    }

    private void TargetObjectIsAlignedWithAnnotation()
    {
        Debug.Log("Correct!!!!");
        count++;
    }

    private void EnableTimer(bool b)
    {
        timerOn = b;
    }

}
