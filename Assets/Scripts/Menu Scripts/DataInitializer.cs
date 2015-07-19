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

		if (!PlayerPrefs.HasKey (userDefineKey.FBLoginStatus)) {
			PlayerPrefs.SetString (userDefineKey.FBLoginStatus, userDefineKey.FBIsLogout);
			InitializeData();
		}

		Invoke ("InvokeHomeMenu", 2.0f);
	}

	void InitializeData(){

		UserAccount userAccount = new UserAccount (couchbaseDatabase);
		userAccount.UserEmail = userDefineKey.TemporaryEmail;
		userAccount.UserID = userDefineKey.TemporaryID;
		userAccount.UserName = userDefineKey.TemporaryUser;
		userAccount.UserToken = userDefineKey.TemporaryToken;

		string UUID = userAccount.Create ();

		PlayerPrefs.SetString (userDefineKey.UUID, UUID);
	}

	void InvokeHomeMenu(){
		Application.LoadLevel (homeMenuSceneName);
	}

}
