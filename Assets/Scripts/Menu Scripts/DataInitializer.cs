using UnityEngine;
using System.Collections;
using AssemblyCSharp;

using Couchbase.Lite;
using Couchbase.Lite.Unity;

public class DataInitializer : MonoBehaviour {

	public string version;
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
		InitializeSettings ();
		Invoke ("InvokeHomeMenu", 2.0f);
	}

	void InitializeData(){
		FBUserAccount userAccount = new FBUserAccount (couchbaseDatabase);
		userAccount.UserEmail = UserAccountDefineKeys.TemporaryEmail;
		userAccount.UserID = UserAccountDefineKeys.TemporaryID;
		userAccount.UserName = UserAccountDefineKeys.TemporaryUser;
		userAccount.UserToken = UserAccountDefineKeys.TemporaryToken;
		userAccount.Version = version;
		string UUID = userAccount.Create ();
		PlayerPrefs.SetString (UserAccountDefineKeys.UUID, UUID);
	}

	void InitializeSettings(){
		if (!PlayerPrefs.HasKey (GameSystemDefineKeys.SoundFXState)) {
			PlayerPrefs.SetInt(GameSystemDefineKeys.SoundFXState, 0);
		}
	}

	void InvokeHomeMenu(){
		Application.LoadLevel (homeMenuSceneName);
	}

}
