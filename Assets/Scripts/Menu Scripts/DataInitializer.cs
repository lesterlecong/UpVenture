using UnityEngine;
using System.Collections;
using AssemblyCSharp;

using Couchbase.Lite;
using Couchbase.Lite.Unity;

public class DataInitializer : MonoBehaviour {

	public GameObject couchbaseDatabaseObject;
	public string homeMenuSceneName ="home menu";

	private UserDefineKeys userDefineKey;
	private CouchbaseDatabase couchbaseDatabase;


	void Awake(){
		couchbaseDatabase = (CouchbaseDatabase)couchbaseDatabaseObject.GetComponent (typeof(CouchbaseDatabase));
		couchbaseDatabase.StartCouchbase ();

		if (!PlayerPrefs.HasKey (userDefineKey.FBLoginStatus)) {
			PlayerPrefs.SetString (userDefineKey.FBLoginStatus, userDefineKey.FBIsLogout);
			InitializeData();
		}

		Invoke ("InvokeHomeMenu", 2.0f);
	}

	void InitializeData(){

		UUIDGenerator uuidGenerator = new UUIDGenerator (couchbaseDatabase);
		uuidGenerator.UserEmail = userDefineKey.TemporaryEmail;
		uuidGenerator.UserID = userDefineKey.TemporaryID;
		uuidGenerator.UserName = userDefineKey.TemporaryUser;
		uuidGenerator.UserToken = userDefineKey.TemporaryToken;

		string UUID = uuidGenerator.GetUUID ();

		PlayerPrefs.SetString (userDefineKey.UUID, UUID);
	}

	void InvokeHomeMenu(){
		Application.LoadLevel (homeMenuSceneName);
	}

}
