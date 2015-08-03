using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;

using Couchbase.Lite;
using Couchbase.Lite.Unity;
using Newtonsoft;

public class DataUpdater : MonoBehaviour {
	public GameObject couchbaseDatabaseObject;
	public Text logText;
	public SocialMediaType socialMediaType = SocialMediaType.FACEBOOK;
	public bool allowProceedToNextScene = true;

	private CouchbaseDatabase couchbaseDatabase;
	private GameObject socialMediaHandlerObject;
	private SocialMediaHandler socialMediaHandler;

	private Replication pullReplication;
	private Replication pushReplication;

	void Awake(){
		Debug.Log ("DataUpdater::Awake()");

		SetupDatabase ();
		SetupReplicator ();
		SetupSocialMediaHandler ();
	}

	void Start () {
		
		AddChannel ();

		if (socialMediaHandler.IsLoggedIn ()) {
			Invoke ("StartReplicate", 0.1f);
		} else {
			NextScene();
		}
	}

	void SetupDatabase(){
		Debug.Log ("DataUpdater::SetupDatabase()");
		couchbaseDatabase = (CouchbaseDatabase)couchbaseDatabaseObject.GetComponent (typeof(CouchbaseDatabase));
	}

	void SetupReplicator(){
		pullReplication = couchbaseDatabase.GetPullReplication ();
		pushReplication = couchbaseDatabase.GetPushReplication ();
	}

	void SetupSocialMediaHandler(){
		socialMediaHandlerObject = GameObject.Find ("SocialMediaHandlerObject");
		if(socialMediaHandlerObject != null){
			socialMediaHandler = (SocialMediaHandler) socialMediaHandlerObject.GetComponent(typeof(SocialMediaHandler));
			socialMediaHandler.socialMediaType = this.socialMediaType;
		}
	}

	void AddChannel(){
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
			pushReplication.Start();
			StartCoroutine(UpdateProgress());
		} else {
			logText.text += "Sync Gateway is Offline\n";
			NextScene();
		}
		
	}

	IEnumerator UpdateProgress(){
		logText.text += "Update Progress";
		while ((pullReplication.Status == ReplicationStatus.Active) || (pushReplication.Status == ReplicationStatus.Active)) {
			logText.text += ".";
			yield return new WaitForSeconds(1.0f);
		}
		logText.text += "\n";
		NextScene ();
	}



	void NextScene(){
		if (allowProceedToNextScene) {
			UserDefineKeys defineKeys;
			Application.LoadLevel (PlayerPrefs.GetString (defineKeys.NextScene));
		}
	}

}
