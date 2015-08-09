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

	private CouchbaseDatabase couchbaseDatabase;
	private GameObject socialMediaHandlerObject;

	private Replication pullReplication;
	private Replication pushReplication;

	private bool isUpdatingDone = false;
	private CallDelegate delegateToCall;

	void Awake(){
		Debug.Log ("DataUpdater::Awake()");

		SetupDatabase ();
		SetupReplicator ();
	}

	void Start () {
		
		AddChannel ();
		
		NextScene();
	}

	void SetupDatabase(){
		Debug.Log ("DataUpdater::SetupDatabase()");
		couchbaseDatabase = (CouchbaseDatabase)couchbaseDatabaseObject.GetComponent (typeof(CouchbaseDatabase));
	}

	void SetupReplicator(){
		pullReplication = couchbaseDatabase.GetPullReplication ();
		pushReplication = couchbaseDatabase.GetPushReplication ();
	}


	public void AddChannel(){
		if (couchbaseDatabase != null && socialMediaHandler != null) {
			if (socialMediaHandler.IsLoggedIn ()) {
				Debug.Log ("At DataUpdater::Start(): Adding Channel to Replication");
				logText.text += "Account ID:" + socialMediaHandler.GetAccountID () + "\n";

				List<string> channels = new List<string> ();
				channels.Add (socialMediaHandler.GetAccountID ());
				pullReplication.Channels = channels;
			}
		} 
	}

	public void StartReplicate(){

		pullReplication.Start ();
		bool isOffline = (pullReplication.Status == ReplicationStatus.Offline);
		Debug.Log ("Replication is " + (isOffline ? "Offline" : "Online"));

		if (!isOffline) {
			logText.text += "Sync Gateway is Online\n";
			StartPushData();
		} else {
			logText.text += "Sync Gateway is Offline\n";
			NextScene();
		}
		
	}

	public void StartPullData(){
		logText.text += "Start to Pull Data\n";
		pullReplication.Start ();
		StartCoroutine(UpdateProgress());
	}

	public void StartPushData(){
		logText.text += "Start to Push Data\n";
		pushReplication.Start();
		StartCoroutine(UpdateProgress());
	}

	public bool IsUpdatingDone(){
		return isUpdatingDone;
	}

	public void AddDelegateToCallAfterReplication(CallDelegate delegateToCall){
		this.delegateToCall = delegateToCall;
	}

	IEnumerator UpdateProgress(){
		logText.text += "Update Progress";
		while ((pullReplication.Status == ReplicationStatus.Active) || (pushReplication.Status == ReplicationStatus.Active)) {
			logText.text += ".";
			isUpdatingDone = false;
			yield return new WaitForSeconds(1.0f);
		}
		isUpdatingDone = true;
		logText.text += "\nDone \n";
		if (delegateToCall != null) {
			delegateToCall ();
		}
		NextScene ();
	}



	void NextScene(){
		if (allowProceedToNextScene) {
			UserDefineKeys defineKeys;
			Application.LoadLevel (PlayerPrefs.GetString (defineKeys.NextScene));
		}
	}

}
