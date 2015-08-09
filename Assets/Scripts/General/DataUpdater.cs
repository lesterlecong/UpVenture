using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;

using Couchbase.Lite;
using Couchbase.Lite.Unity;
using Newtonsoft;

public delegate void CallDelegate();

public class DataUpdater : MonoBehaviour {
	public GameObject couchbaseDatabaseObject;
	public Text logText;
	public bool allowProceedToNextScene = true;
	private bool isUpdatingDone = false;


	void Awake(){
		Debug.Log ("DataUpdater::Awake()");
	}

	void Start () {
		NextScene();
	}

	void NextScene(){
		if (allowProceedToNextScene) {
			UserDefineKeys defineKeys;
			Application.LoadLevel (PlayerPrefs.GetString (defineKeys.NextScene));
		}
	}

}
