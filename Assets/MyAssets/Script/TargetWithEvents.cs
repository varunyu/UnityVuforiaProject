﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetWithEvents : MonoBehaviour
{
    public event Action OnTargetAlignedWithAnnotation;
    public event Action<bool> UpdatePOSIndicator;
    public event Action<bool> UpdateROTIndicator;

    private bool isCollider;
    private GameObject colliderObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (isCollider)
        {
            if(CheckPos())
            {
                if (OnTargetAlignedWithAnnotation != null)
                {
                    OnTargetAlignedWithAnnotation();
                }
                this.gameObject.SetActive(false);
                colliderObject = null;
            }
        }
    }
    /*
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("HITTT");
        isCollider = true;
        colliderObject = collision.gameObject;
    }

    private void OnCollisionExit(Collision collision)
    {
        isCollider = false;
        colliderObject = null;
    }*/

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("HITTT");
        isCollider = true;
        colliderObject = other.gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        isCollider = false;
        colliderObject = null;

        if (UpdatePOSIndicator != null)
        {
            UpdatePOSIndicator(false);
        }
        if (UpdateROTIndicator != null)
        {
            UpdateROTIndicator(false);
        }

    }




    private bool CheckPos()
    {
        if(UpdatePOSIndicator!= null)
        {
            UpdatePOSIndicator(CheckTranslation());
        }
        if (UpdateROTIndicator != null)
        {
            UpdateROTIndicator(CheckOrientation());
        }


        if (CheckTranslationWithDelay()&& CheckOrientation())
        {
            Destroy(colliderObject);
            return true;
        }
        else return false;
    }

    private float timer = 0;
    private float delayTime = 1f;
    private float minDis = 3f;
    private bool CheckTranslationWithDelay()
    {
        timer += Time.deltaTime;
        if (timer >= delayTime)
        {
            if (Vector3.Distance(colliderObject.transform.position, this.transform.position) <= minDis)
            {
                timer = 0;
                return true;
            }

        }
        return false;
    }

    private bool CheckTranslation()
    {
        if (Vector3.Distance(colliderObject.transform.position, this.transform.position) <= minDis)
        {
            return true;
            
        }

        return false;
    }

    private float minRot = 12f;
    private bool CheckOrientation()
    {
        if (Quaternion.Angle(colliderObject.transform.rotation, this.transform.rotation) <= minRot)
        {
            return true;
        }

        return false;
    }
}
