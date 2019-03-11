using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslationAndIntial : MonoBehaviour {

	public GameObject parentObject;
    public GameObject tmpWorldCoordinate;
    private GameObject worldCoordinate;

    private Transform emptyTran;
    public GameObject[] preFabList;
    private int preFabIndex =0;

	void Awake(){
        //ARKitHitScript = (ARKitHitCheck)gameObject.GetComponent(typeof(ARKitHitCheck));
        var stage = PlayerPrefs.GetInt("WorldCoor");
        //Debug.Log("WorldCoordi "+ stage);

        if (stage == 1)
        {
            worldCoordinate = tmpWorldCoordinate;
        }
        else
        {
            worldCoordinate = parentObject;
        }
    }
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

    public void SetPrefabIndex(int i)
    {
        preFabIndex = i;
    }

    public GameObject ObjectInstantiateDubug()
    {
        return (Instantiate(preFabList[preFabIndex], new Vector3(0, 0, 0), worldCoordinate.transform.rotation, parentObject.transform));
    }

    public GameObject ObjectInstantiate(Touch t,GameObject prefab){
		//emptyTran = ARKitHitScript.HitLoc (t);
		return (Instantiate (prefab, GetPosFrom2DTouch(t), worldCoordinate.transform.rotation, parentObject.transform));
	}

    public GameObject ObjectInstantiate(Touch t)
    {
        return (Instantiate(preFabList[preFabIndex], GetPosFrom2DTouch(t), worldCoordinate.transform.rotation, parentObject.transform));
    }

    [SerializeField]
    private float range = 80f;
    public Vector3 GetRealWorldPos(Vector2 t){
        ray = Camera.main.ScreenPointToRay(t);
        return ray.GetPoint(range);//ARKitHitScript.HitLoc (t).position;
    }
    public Vector3 GetRealWorldPos(Vector2 t, float R)
    {
        ray = Camera.main.ScreenPointToRay(t);
        return ray.GetPoint(R);//ARKitHitScript.HitLoc (t).position;
    }


    public Vector3 GetRealWorldPos(Touch t){
		return GetPosFrom2DTouch(t);//ARKitHitScript.HitLoc (t).position;
	}
	private Ray ray;
	public Vector3 GetPosFrom2DTouch(Touch t){
        ray = Camera.main.ScreenPointToRay(t.position);
		return ray.GetPoint(range);
	}

    public Vector3 GetPosFrom2DTouch(Touch t,float R)
    {
        ray = Camera.main.ScreenPointToRay(t.position);
        return ray.GetPoint(R);
    }

    public void DebugObjectCreation()
    {
        Instantiate(preFabList[0],new Vector3(0,0,0), parentObject.transform.rotation, parentObject.transform);
    }
}
