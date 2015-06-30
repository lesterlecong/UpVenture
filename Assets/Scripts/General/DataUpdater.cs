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
	private Replication pushReplication;
	private Replication pullReplication;



	void Start () {

		couchbaseDatabase = (CouchbaseDatabase)couchbaseDatabaseObject.GetComponent (typeof(CouchbaseDatabase));
		pullReplication = couchbaseDatabase.GetPullReplication ();
		pushReplication = couchbaseDatabase.GetPushReplication ();
		
		Invoke ("StartReplicate", 0.1f);
	}

	void StartReplicate(){
		pullReplication.Start ();

		bool isOffline = (pullReplication.Status == ReplicationStatus.Offline);

		if (!isOffline) {
			logText.text += "Sync Gateway is Online\n";
			pushReplication.Start ();
			StartCoroutine (UpdateProgress ());
		} else {
			logText.text += "Sync Gateway is Offline\n";
			NextScene();
		}
	}

	IEnumerator UpdateProgress(){
		logText.text += "Update Progress";	
		while ((pullReplication.Status == ReplicationStatus.Active) || (pushReplication.Status == ReplicationStatus.Active)){
			logText.text += ".";	
			yield return new WaitForSeconds(1.0f);
		}
		logText.text += "\n";	
		NextScene ();
	}

	void NextScene(){
		UserDefineKeys defineKeys;
		Application.LoadLevel(PlayerPrefs.GetString(defineKeys.NextScene));
	}

}
