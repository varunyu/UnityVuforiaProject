using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslationAndIntial : MonoBehaviour {

	public GameObject parentObject;
	private Transform emptyTran;
    public GameObject[] preFabList;
    private int preFabIndex;

	void Awake(){
		//ARKitHitScript = (ARKitHitCheck)gameObject.GetComponent(typeof(ARKitHitCheck));
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

    public GameObject ObjectInstantiate(Touch t,GameObject prefab){
		//emptyTran = ARKitHitScript.HitLoc (t);
		return (Instantiate (prefab, GetPosFrom2DTouch(t), parentObject.transform.rotation, parentObject.transform));
	}

    public GameObject ObjectInstantiate(Touch t)
    {
        return (Instantiate(preFabList[preFabIndex], GetPosFrom2DTouch(t), parentObject.transform.rotation, parentObject.transform));
    }


    public Vector3 GetRealWorldPos(Vector2 t){
        ray = Camera.main.ScreenPointToRay(t);
        return ray.GetPoint(1.3f);//ARKitHitScript.HitLoc (t).position;
    }

	public Vector3 GetRealWorldPos(Touch t){
		return GetPosFrom2DTouch(t);//ARKitHitScript.HitLoc (t).position;
	}
	private Ray ray;
	public Vector3 GetPosFrom2DTouch(Touch t){
        ray = Camera.main.ScreenPointToRay(t.position);
		return ray.GetPoint(2.0f);
	}


}
