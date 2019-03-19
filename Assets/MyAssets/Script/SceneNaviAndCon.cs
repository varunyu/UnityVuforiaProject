using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vuforia;

public class SceneNaviAndCon : MonoBehaviour
{
    [SerializeField]
    private GameObject loadingPannel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeScenes(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void RestartTracking()
    {
        TrackerManager.Instance.GetTracker<ObjectTracker>().Stop();
        loadingPannel.SetActive(true);
        TrackerManager.Instance.GetTracker<ObjectTracker>().Start();
        Invoke("DeactiveLoadingPannel",1.5f);

    }

    private void DeactiveLoadingPannel()
    {
        loadingPannel.SetActive(false);
    }
}
