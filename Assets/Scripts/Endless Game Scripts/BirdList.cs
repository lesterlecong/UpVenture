using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BirdList : MonoBehaviour {
	private static BirdList _instance;
	private static List<GameObject> birdGroupList;

	static public BirdList instance{
		get{
			if(_instance == null){
				_instance = Object.FindObjectOfType(typeof(BirdList)) as BirdList;

				if(_instance == null){
					GameObject birdListObject = new GameObject("_birdlist");
					DontDestroyOnLoad(birdListObject);
					_instance = birdListObject.AddComponent<BirdList>();
				}
				birdGroupList = new List<GameObject> ();
			}


			return _instance;
		}
	}
	
	public void addBirdTarget(GameObject birdGroup){

		birdGroupList.Add (birdGroup);

	}

	public void removeBirdTarget(GameObject birdGroup){
		birdGroupList.Remove (birdGroup);
	}

	public List<GameObject> birdTargetList(){
		return birdGroupList;
	}

	void onDestroy(){
		Debug.Log ("BirdList destroy");
	}

}
