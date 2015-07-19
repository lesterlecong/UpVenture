using System;
using AssemblyCSharp;
using Couchbase.Lite;
using Couchbase.Lite.Unity;
using Couchbase.Lite.Util;
using Couchbase.Lite.Auth;

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using System.Threading.Tasks;

public class CouchbaseDatabase : MonoBehaviour {

	#region Public Member Variables
	public string hostName = "localhost";
	public int portNumber = 4984;
	public string databaseName = "adventuredb";
	#endregion

	#region Member Variables
	private Document doc;
	private Database database;
	private Replication pullReplication;
	private Replication pushReplication;
	private string documentID;
	private UserDefineKeys userDefineKey;
	private string syncGateWayURI;
	#endregion

	#region Private Methods
	void Awake(){
		CreateDatabase ();
		SetupReplication ();
	}

	void SetupReplication(){
		syncGateWayURI = "http://" + hostName + ":" + portNumber.ToString() + "/" + databaseName;
		pullReplication = database.CreatePullReplication (new Uri (syncGateWayURI));
		pushReplication = database.CreatePushReplication (new Uri (syncGateWayURI));
	}

	IEnumerator DoReplication(Replication replication){
		replication.Start ();
		
		while (replication != null && replication.Status == ReplicationStatus.Active) {
			yield return new WaitForSeconds(0.5f);
		}
	}

	void CreateDatabase(){
		if (CouchbaseLiteManager == null) {
			Debug.LogError("Manager is NULL");
			return;
		}
		
		if (String.IsNullOrEmpty(databaseName)) {
			Debug.LogError("Databasename is Empty");
			return ;
		}

		database = CouchbaseLiteManager.GetDatabase(databaseName);

		if (database == null) {
			Debug.LogError("Cannot Create Database!!");
			return;
		}

	}

	#endregion

	#region Properties
	static public Manager CouchbaseLiteManager{
		get{
			if(couchbaseLiteManager == null){
				ManagerOptions options = Manager.DefaultOptions;
				couchbaseLiteManager = new Manager(new DirectoryInfo (Application.persistentDataPath), options);
			}
			
			return couchbaseLiteManager;
		}
	}
	static private Manager couchbaseLiteManager;
	

	public string DocumentID{
		set{
			documentID = value;
		}
		get{
			return documentID;
		}
	}

	public Document CouchbaseDocument{
		set{
			doc = value;
		}
		get{
			return doc;
		}
	}

	#endregion

	#region Public Method
	public void StartCouchbase(){

	}

	public Database GetCouchbaseDatabase(){
		if (database == null) {
			CreateDatabase();
		}

		return database;
	}

	public void PullRemoteChanges(){
		DoReplication (pullReplication);
	}

	public void PushRemoteChanges(){
		DoReplication (pushReplication);
	}

	public Replication GetPullReplication(){
		return pullReplication;
	}

	public Replication GetPushReplication(){
		return pushReplication;
	}



	public void CreateDocument(){
		if (database == null) {
			CreateDatabase();
		}

		CouchbaseDocument = database.GetExistingDocument (DocumentID);
		if (CouchbaseDocument == null) {
			CouchbaseDocument = database.GetDocument(DocumentID);
		}
	}

	public void SaveData(Dictionary<string, object> data){
		Dictionary<string, object>.KeyCollection keys = data.Keys;
		foreach (string key in keys) {
			SaveData(key, data[key]);
		}
	}
	
	public void SaveData(string key, object value){
		CouchbaseDocument.Update ((UnsavedRevision newRevision) => {
			Dictionary<string, object> properties = (Dictionary<string, object>)newRevision.Properties;
			properties [key] = value;
			return true;
		});
	}

	public object ReadDataAsObject(string key){
		object objectData = CouchbaseDocument.GetProperty (key);
		return objectData;
	}
	
	public string ReadDataAsString(string key){
		object objectData = ReadDataAsObject (key);
		string dataAsString = "";
		
		if (objectData != null) {
			dataAsString = objectData.ToString ();
		} 
		
		return dataAsString;
	}


	public bool IsDatabaseNull(){
		return (database == null);
	}

	public bool IsDocumentNull(){
		return (CouchbaseDocument == null);
	}

	public bool ReplicationsNull(){
		return ((pullReplication == null) || (pullReplication == null));
	}

	#endregion
	
}
