﻿using System;
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
using PreviewLabs;

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
	//private IAuthenticator facebookAuthenticator;
	//private string fbTokenKey = "fbtoken";
	private UserDefineKeys userDefineKey;
	private string UserUUID;
	private string syncGateWayURI;
	#endregion

	#region Private Methods
	void Awake(){
		CreateDatabase ();
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

	
	string GenerateNewUUID(){
		Document newDocument = database.CreateDocument ();
		string docID = "";
		newDocument.Update ((UnsavedRevision newRevision) => {
			Dictionary<string, object> properties = (Dictionary<string, object>)newRevision.Properties;
			properties [userDefineKey.FBUsername] = GetPreference (userDefineKey.FBUsername);
			properties [userDefineKey.FBUserID] = GetPreference (userDefineKey.FBUserID);
			properties [userDefineKey.FBEmail] = GetPreference (userDefineKey.FBEmail);
			properties [userDefineKey.FBToken] = GetPreference (userDefineKey.FBToken);
			docID = properties["_id"].ToString();
			Debug.Log ("_id: " + properties["_id"]);
			return true;
		});
		
		DocumentID = userDefineKey.FBUserID + "::" + GetPreference(userDefineKey.FBUserID);
		CreateDocument ();
		saveData (userDefineKey.UUID, docID);

		if(String.IsNullOrEmpty(GetPreference(userDefineKey.FBEmail))){
			DocumentID = userDefineKey.FBEmail + "::" + GetPreference(userDefineKey.FBUserID) + "@noemail.com";
		}else{
			DocumentID = userDefineKey.FBEmail + "::" + GetPreference(userDefineKey.FBEmail);//change this one
		}
		CreateDocument ();
		saveData (userDefineKey.UUID, docID);


		return docID;
	}

	string GetPreference(string key){
		return PreviewLabs.PlayerPrefs.GetString (key);
	}

	#endregion

	#region Properties
	static public Manager CouchbaseLiteManager{
		get{
			if(couchbaseLiteManager == null){
				couchbaseLiteManager = new Manager();
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

	#endregion

	#region Public Method
	public void StartCouchbase(){
		/****DO IT LATER******************************************
		string facebookToken = PlayerPrefs.GetString (fbTokenKey);

		Debug.Log ("Facebook Token: " + facebookToken);
		if (String.IsNullOrEmpty (facebookToken)) {
			Debug.LogError("Please provide facebook token");
			return;
		}
		
		
		facebookAuthenticator = AuthenticatorFactory.CreateFacebookAuthenticator (facebookToken);
		Debug.Log ("User Info: " + facebookAuthenticator.UserInfo);
		if (facebookAuthenticator == null) {
			Debug.Log ("facebookAuthenticator is null");
			return;
		}
		**********************************************************/
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

	public string GetUUID(){
		if (database == null) {
			CreateDatabase();
		}
		string UUID = "";
		

		doc = database.GetExistingDocument (userDefineKey.FBUserID + "::" + GetPreference (userDefineKey.FBUserID));
		if (doc != null) {
			UUID = readDataAsString(userDefineKey.UUID);
		}
		
		if(string.IsNullOrEmpty(UUID)){
			doc = database.GetExistingDocument (userDefineKey.FBEmail + "::" + GetPreference(userDefineKey.FBEmail));
			if (doc != null) {
				UUID = readDataAsString(userDefineKey.UUID);
			}
		}
		
		if (string.IsNullOrEmpty (UUID)) {
			UUID = GenerateNewUUID();
		}

		Debug.Log ("Returned UUID: " + UUID);
		return UUID;
	}

	public void CreateDocument(){
		if (database == null) {
			CreateDatabase();
		}

		doc = database.GetExistingDocument (DocumentID);
		if (doc == null) {
			doc = database.GetDocument(DocumentID);
		}


	}

	public void saveData(Dictionary<string, object> data){
		Dictionary<string, object>.KeyCollection keys = data.Keys;
		foreach (string key in keys) {
			saveData(key, data[key]);
		}
	}
	
	public void saveData(string key, object value){
		doc.Update ((UnsavedRevision newRevision) => {
			Dictionary<string, object> properties = (Dictionary<string, object>)newRevision.Properties;
			properties [key] = value;
			return true;
		});
	}


	
	public object readDataAsObject(string key){
		Debug.Log ("At CouchbaseDatabase doc id = " + doc.Id);
		object objectData = doc.GetProperty (key);
		return objectData;
	}
	
	public string readDataAsString(string key){
		Debug.Log ("At CouchbaseDatabase key: " + key);
		object objectData = readDataAsObject (key);
		string dataAsString = "";
		
		if (objectData != null) {
			dataAsString = objectData.ToString ();
		} else {
			Debug.Log("At CouchbaseDatabase: object data is null");
		}
		
		return dataAsString;
	}
	
	#endregion
	
}
