using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using AssemblyCSharp;

using Couchbase.Lite;
using Couchbase.Lite.Unity;
using Newtonsoft;

public class DataUpdater : MonoBehaviour {
	public GameObject couchbaseDatabaseObject;
	public Text logText;

	private CouchbaseDatabase couchbaseDatabase;

	void Start () {

		couchbaseDatabase = (CouchbaseDatabase)couchbaseDatabaseObject.GetComponent (typeof(CouchbaseDatabase));
	
		Invoke ("StartReplicate", 0.1f);
	}

	void StartReplicate(){

		if (couchbaseDatabase.IsPullReplicationOnline () && couchbaseDatabase.IsPushReplicationOnline()) {
			logText.text += "Sync Gateway is Online\n";
			couchbaseDatabase.PullDataChanges();
			couchbaseDatabase.PullDataChanges();
		}
	
			NextScene();
		
	}



	void NextScene(){
		UserDefineKeys defineKeys;
		Application.LoadLevel(PlayerPrefs.GetString(defineKeys.NextScene));
	}

}
