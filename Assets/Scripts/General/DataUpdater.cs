using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections;
using AssemblyCSharp;

using Couchbase.Lite;
using Couchbase.Lite.Unity;
using Newtonsoft;

public class DataUpdater : MonoBehaviour {
	public GameObject couchbaseDatabaseObject;

	private CouchbaseDatabase couchbaseDatabase;
	private Replication pushReplication;
	private Replication pullReplication;
	private UserDefineKeys defineKeys;

	void Start () {
		couchbaseDatabase = (CouchbaseDatabase)couchbaseDatabaseObject.GetComponent (typeof(CouchbaseDatabase));
		pullReplication = couchbaseDatabase.GetPullReplication ();
		pushReplication = couchbaseDatabase.GetPushReplication ();
		StartCoroutine (UpdateProgress ());
	}
	
	IEnumerator UpdateProgress(){
		pullReplication.Start ();
		pushReplication.Start ();

		while ((pullReplication.Status == ReplicationStatus.Active) || (pushReplication.Status == ReplicationStatus.Active)){

			yield return new WaitForSeconds(1.0f);
		}

		Application.LoadLevel (PlayerPrefs.GetString (defineKeys.NextScene));

	}
}
