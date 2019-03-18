using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vuforia;

public class UserStudyUI : MonoBehaviour
{
    /*
    [SerializeField]
    private GameObject pos_Indicator_UI;
    [SerializeField]
    private GameObject Rot_Indicator_UI;
    */
    private GameObject currentTarget;

    [SerializeField]
    private GameObject finishText;

    [SerializeField]
    private GameObject time_Out_Object;
    // Start is called before the first frame update

    [SerializeField]
    private Camera mainCam;

    [SerializeField]
    private GameObject loadingPannel;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }/*
    public void RegisterNewListenser(GameObject go)
    {
        //go
        currentTarget = go;
        currentTarget.GetComponent<TargetWithEvents>().UpdatePOSIndicator += EnablePosIndicatorUI;
        currentTarget.GetComponent<TargetWithEvents>().UpdateROTIndicator += EnableRotIndicatorUI;
    }
    public void UnregisterListener()
    {
        currentTarget.GetComponent<TargetWithEvents>().UpdatePOSIndicator -= EnablePosIndicatorUI;
        currentTarget.GetComponent<TargetWithEvents>().UpdateROTIndicator -= EnableRotIndicatorUI;
        currentTarget = null;
        //
    }

    public void ClearAll()
    {
        UnregisterListener();
        EnablePosIndicatorUI(false);
        EnableRotIndicatorUI(false);
    }

    private void EnablePosIndicatorUI(bool t)
    {
        pos_Indicator_UI.SetActive(t);
    }

    private void EnableRotIndicatorUI(bool t)
    {
        Rot_Indicator_UI.SetActive(t);
    }
    */
    public void BackToMenu()
    {
        SceneManager.LoadScene("Main");
    }

    public void ShowFinishTesxt(bool b)
    {
        finishText.SetActive(true);
    }
    public void ShowTimeOutText(bool b)
    {
        time_Out_Object.SetActive(b);
    }

    public void RefreshCamera()
    {
        mainCam.gameObject.SetActive(false);
        loadingPannel.SetActive(true);
        mainCam.gameObject.SetActive(true);
        //loadingPannel.SetActive(false);
        Invoke("DeactiveLoadingPannel", 1.5f);
    }

    public void RestartTracking()
    {
        TrackerManager.Instance.GetTracker<ObjectTracker>().Stop();
        loadingPannel.SetActive(true);
        TrackerManager.Instance.GetTracker<ObjectTracker>().Start();
        //Invoke("DeactiveLoadingPannel",2);

    }

    private void DeactiveLoadingPannel()
    {
        loadingPannel.SetActive(false);
    }
}
