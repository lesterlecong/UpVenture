using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using AssemblyCSharp;

using Couchbase.Lite;
using Couchbase.Lite.Unity;
using Newtonsoft;

public class DataUpdater : MonoBehaviour {
	public GameObject couchbaseDatabaseObject;
	public GameObject socialMediaHandlerObject;
	public Text logText;

	private CouchbaseDatabase couchbaseDatabase;
	private SocialMediaHandler socialMediaHandler;
	void Start () {

		couchbaseDatabase = (CouchbaseDatabase)couchbaseDatabaseObject.GetComponent (typeof(CouchbaseDatabase));
		socialMediaHandler = (SocialMediaHandler)socialMediaHandlerObject.GetComponent (typeof(SocialMediaHandler));

		if (couchbaseDatabase != null && socialMediaHandler != null) {
			if(socialMediaHandler.IsLoggedIn()){
				couchbaseDatabase.AddChannel(socialMediaHandler.GetAccountID());
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
