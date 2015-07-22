using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using AssemblyCSharp;

using Couchbase.Lite;
using Couchbase.Lite.Unity;
using Newtonsoft;

public class DataUpdater : MonoBehaviour {
	public GameObject couchbaseDatabaseObject;
	public GameObject facebookHandlerObject;
	public Text logText;

	private CouchbaseDatabase couchbaseDatabase;
	private FacebookHandler facebookHandler;
	void Start () {

		couchbaseDatabase = (CouchbaseDatabase)couchbaseDatabaseObject.GetComponent (typeof(CouchbaseDatabase));
		facebookHandler = (FacebookHandler)facebookHandlerObject.GetComponent (typeof(FacebookHandler));

		if (couchbaseDatabase != null && facebookHandler != null) {
			if(facebookHandler.IsLoggedIn()){
				couchbaseDatabase.AddChannel(facebookHandler.GetUserID());
				Invoke ("StartReplicate", 0.1f);
			}else{
				NextScene();
			}
		}


	}

	void StartReplicate(){

		couchbaseDatabase.PullDataChanges();
		if (!couchbaseDatabase.IsPullReplicationOffline()) {
			logText.text += "Sync Gateway is Online\n";
			couchbaseDatabase.PushDataChanges ();
		} else {
			logText.text += "Sync Gateway is Offline\n";
		}
	
			NextScene();
		
	}



	void NextScene(){
		UserDefineKeys defineKeys;
		Application.LoadLevel(PlayerPrefs.GetString(defineKeys.NextScene));
	}

}
