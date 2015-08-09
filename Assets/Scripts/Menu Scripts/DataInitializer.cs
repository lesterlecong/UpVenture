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

		if (!PlayerPrefs.HasKey (UserAccountDefineKeys.FBLoginStatus)) {
			PlayerPrefs.SetString (UserAccountDefineKeys.FBLoginStatus, UserAccountDefineKeys.FBIsLogout);
			InitializeData();
		}

		Invoke ("InvokeHomeMenu", 2.0f);
	}

	void InitializeData(){
		FBUserAccount userAccount = new FBUserAccount (couchbaseDatabase);
		userAccount.UserEmail = UserAccountDefineKeys.TemporaryEmail;
		userAccount.UserID = UserAccountDefineKeys.TemporaryID;
		userAccount.UserName = UserAccountDefineKeys.TemporaryUser;
		userAccount.UserToken = UserAccountDefineKeys.TemporaryToken;
		string UUID = userAccount.Create ();
		PlayerPrefs.SetString (UserAccountDefineKeys.UUID, UUID);
	}

	void InvokeHomeMenu(){
		Application.LoadLevel (homeMenuSceneName);
	}

}
