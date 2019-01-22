using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserStudyUI : MonoBehaviour
{
    [SerializeField]
    private GameObject pos_Indicator_UI;
    [SerializeField]
    private GameObject Rot_Indicator_UI;

    private GameObject currentTarget;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
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
}
