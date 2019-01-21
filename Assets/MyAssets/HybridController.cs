using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class HybridController : MonoBehaviour {

    public event System.Action<bool> AnnotationIsBeingSelected;


    public enum AppState
    {
        NONE,
        ADD,
        AUTORING,
        EDIT,
        /*
         NONE =0
         ADD =1
         AUTORING =2
         EDIT =3
         * */
    }
    private AppState currState;
    private GameObject sObject;
    private TranslationAndIntial traAIni;
    private OrientationControl orienCont;

    private UIFollowObject UIFObj;

    public GameObject crossHairObj;
    public GameObject objCenterIn2D;
    public GameObject confirmButton;

    private int nFinger;
    private float rayDis;
    private bool isObjectVerticalToG;

    private Touch touch1;
    private Touch touch2;

    private Vector2 t1PrevPos;
    private Vector2 t2PrevPos;
    private Vector2 prevDir;
    private Vector2 currentDir;
    float prevMagnitude;
    float cMagnitude;
    float diffMagnitude;
    private float minPitcgDis = 10f;

    void Start () {
        currState = AppState.NONE;
        traAIni = (TranslationAndIntial)gameObject.GetComponent(typeof(TranslationAndIntial));
        orienCont = (OrientationControl)gameObject.GetComponent(typeof(OrientationControl));
        UIFObj = (UIFollowObject)objCenterIn2D.GetComponent(typeof(UIFollowObject));
    }

    public void ChangeState(int i)
    {
        currState = (AppState)i;
    }

   
	// Update is called once per frame
	void Update () {



        if (Input.touchCount > 0)
        {
            nFinger = Input.touchCount;
            if (nFinger > 0)
            {
                touch1 = Input.GetTouch(0);

                if (EventSystem.current.IsPointerOverGameObject(touch1.fingerId))
                {
                    return;
                }

                switch (currState)
                {
                    case AppState.NONE:
                        {

                            break;
                        }
                    case AppState.ADD:
                        {
                            if (touch1.phase == TouchPhase.Began)
                            {

                                SelectedObject(traAIni.ObjectInstantiate(touch1));
                                SetInitialOrientation();
                                confirmButton.SetActive(true);
                                ChangeState(2);
                            }
                            break;
                        }
                    case AppState.AUTORING:
                        {
                            if (touch1.phase == TouchPhase.Began || touch1.phase == TouchPhase.Moved)
                            {
                                var tmpTouch = touch1.position;
                                tmpTouch.y += 100;
                                sObject.transform.position = traAIni.GetRealWorldPos(tmpTouch);
                            }
                            break;
                        }
                    case AppState.EDIT:
                        {
                            if (nFinger == 1)
                            {
                                if (Mathf.Abs(touch1.deltaPosition.x) >= 2.3f)
                                {
                                    sObject.transform.RotateAround(sObject.transform.position, Camera.main.transform.up,
                                        touch1.deltaPosition.x * 17.0f * Time.deltaTime * -1);
                                }

                                if (Mathf.Abs(touch1.deltaPosition.y) >= 2.3f)
                                {

                                    sObject.transform.RotateAround(sObject.transform.position, Camera.main.transform.right,
                                        touch1.deltaPosition.y * 17.0f * Time.deltaTime);
                                }
                            }
                            else
                            {
                                touch2 = Input.GetTouch(1);

                                //Debug.Log ("2Finger distance: "+ Mathf.Abs(Vector2.Distance(touch1.position,touch2.position) ));

                                if (touch1.phase == TouchPhase.Moved)
                                {
                                    t1PrevPos = touch1.position - touch1.deltaPosition;
                                    t2PrevPos = touch2.position - touch2.deltaPosition;

                                    prevMagnitude = (t1PrevPos - t2PrevPos).magnitude;
                                    cMagnitude = (touch1.position - touch2.position).magnitude;
                                    diffMagnitude = (prevMagnitude - cMagnitude);
                                        //print (diffMagnitude);
                                    
                                    if (Mathf.Abs (diffMagnitude) >= minPitcgDis) {
                                        //Debug.Log ("Scale : "+ (diffMagnitude * 0.00009f));
                                        //rayDis += diffMagnitude * 0.00009f;
                                            //sObject.transform.localScale *= diffMagnitude * 0.00005f;
                                            //Pitch finger
                                    }
                                    else {

                                        prevDir = t2PrevPos - t1PrevPos;
                                        currentDir = touch2.position - touch1.position;

                                        sObject.transform.RotateAround(sObject.transform.position, Camera.main.transform.forward, orienCont.ZTwistGesture(prevDir, currentDir));
                                        //sObject.transform.Rotate (Camera.main.transform.forward, orienCont.ZTwistGesture (prevDir, currentDir)*-1);
                                    }
                                    
                                }


                            }
                            break;
                        }

                    default:
                        break;
                }
            }

        }

        if (sObject!=null && currState == AppState.EDIT)
        {
            DeviceMovemnt();
        }

    }

    private void DeviceMovemnt()
    {
        Ray tmpray = Camera.main.ScreenPointToRay(crossHairObj.transform.position);
        sObject.transform.position = tmpray.GetPoint(rayDis);
    }

    private Ray ray;

    public void PickupObject()
    {
        GetSelectedObject();
    }

    private bool GetSelectedObject()
    {
        ray = Camera.main.ScreenPointToRay(crossHairObj.transform.position);
        foreach (RaycastHit hit in Physics.RaycastAll(ray))
        {
            if (hit.collider.tag.Equals("3DModel") || hit.collider.tag.Equals("Annotation"))
            {
                Debug.Log("HIT !!!! :  " + hit.collider.tag);

                SelectedObject(hit.collider.gameObject);
                rayDis = Vector3.Distance(Camera.main.ScreenToWorldPoint(crossHairObj.transform.position), hit.collider.gameObject.transform.position);

                return true;
            }
        }
        return false;
    }

    public void ObjectPointToGround(bool t)
    {
        isObjectVerticalToG = t;
    }
    private void SetInitialOrientation()
    {

        if (!isObjectVerticalToG)
        {
            sObject.transform.Rotate(Vector3.right, 90f);
        }
    }

    private void SelectedObject(GameObject selected)
    {
        objCenterIn2D.SetActive(true);
        sObject = selected;
        UIFObj.SetObjectToFollow(selected);

        if (currState != AppState.AUTORING) {
            if (AnnotationIsBeingSelected != null)
            {
                AnnotationIsBeingSelected(true);
            }
        }
        ChangeState(3);
    }
    public void DeSelectObject()
    {
        objCenterIn2D.SetActive(false);
        sObject = null;

        if (AnnotationIsBeingSelected != null)
        {
            AnnotationIsBeingSelected(false);
        }
        ChangeState(0);
    }

}
