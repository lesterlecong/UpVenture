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

	void Start () {
		Invoke("NextScene", 1.0f);
	}

	void NextScene(){
		if (allowProceedToNextScene) {
			UserDefineKeys defineKeys;
			Application.LoadLevel (PlayerPrefs.GetString (defineKeys.NextScene));
		}
	}

}
