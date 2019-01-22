using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UserStudyMainScene : MonoBehaviour
{

    /// 
    /// 0 = slidAR+
    /// 1 = Hybrid
    ///
    [SerializeField]
    private int ChosenMethod;


    /// 
    /// 0 = Pos_Easy
    /// 1 = Pos_Hard
    /// 2 = Target_Aligned_World_G
    /// 3 = Target_Not_Aligned_World_G
    /// 4 = Target_Aligned_World_Not
    /// 5 = Target_Not_Aligned_World_Not
    /// 
    [SerializeField]
    private int ChosenStage; 


    public void SetChosenMethod(int i)
    {
        ChosenMethod = i;
        PlayerPrefs.SetInt("ChosenSystem",ChosenMethod);
    }

    public void SetChosenStage(int i)
    {
        ChosenStage = i;
        PlayerPrefs.SetInt("ChosenStage",ChosenStage);
    }

    public void LoadUserStudyScenes()
    {
        SceneManager.LoadScene("UserStudy");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    
    }
}
