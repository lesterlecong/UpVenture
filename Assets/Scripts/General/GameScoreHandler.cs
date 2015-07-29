using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using AssemblyCSharp;
using Couchbase.Lite;
using Couchbase.Lite.Unity;
using Newtonsoft;

public class GameScoreHandler : MonoBehaviour {

	public GameType gameType;
	public GameObject couchbaseDatabaseObject;

	private string scoreFieldName;
	private int level = 0;
	private string documentIDWithLevel;
	private CouchbaseDatabase couchbaseDatabase;
	private string userUUID;
	private UserDefineKeys userDefineKey;
	private GameObject socialMediaHandlerObject;
	private SocialMediaHandler socialMediaHandler;
	public int Level{
		set{
			level = value;
		}
		get{
			return level;
		}
	}

	public string ScoreFieldName{
		set{
			scoreFieldName = value;
		}
		get{
			return scoreFieldName;
		}
	}

	public int HighScore{
		set{
			if (value > GetPreviousScore ()) {
				Debug.Log("At GameScoreHandler::Highscore Field: " + GetFieldNameForHighScore());
				couchbaseDatabase.SaveData(GetFieldNameForHighScore(), value.ToString());
			}
		}
		get{
			Debug.Log ("At GameScoreHandler::Highscore Score: " + GetPreviousScore().ToString());
			return GetPreviousScore();
		}
	}

	public int AccumulatedScore{
		set{
			if(value > GetPreviousScore()){
				int accumulatedScore = GetAccumulatedScore() + (value - GetPreviousScore());
				couchbaseDatabase.SaveData(userDefineKey.Total, accumulatedScore.ToString());
				Debug.Log("Save accumulated score:" + accumulatedScore.ToString());
			}
		}

		get{
			return GetAccumulatedScore();
		}
	}

	public int ScoreRequired{
		set{
			couchbaseDatabase.SaveData(userDefineKey.Level + Level.ToString () + userDefineKey.RequiredScore, value.ToString()); 
		}
		get{
			return ConvertDataStoreReadingToInt(couchbaseDatabase.ReadDataAsString(userDefineKey.Level + Level.ToString () + userDefineKey.RequiredScore));
		}

	}

	public void InitGameScoreHandlerDocument(){
		couchbaseDatabase.CreateDocumentWithID(GetAdventureType(gameType) + userUUID);
		couchbaseDatabase.SelectDocumentWithID(GetAdventureType(gameType) + userUUID);
		if (socialMediaHandler != null) {
			if(socialMediaHandler.IsLoggedIn()){
				couchbaseDatabase.SaveData(UserAccountDefineKeys.Channels, socialMediaHandler.GetAccountID ());
			}
		}
	}

	int GetPreviousScore(){
		//Debug.Log ("At GameScoreHandler::GetPreviousScore Field: " + GetFieldNameForHighScore ());
		return ConvertDataStoreReadingToInt(couchbaseDatabase.ReadDataAsString (GetFieldNameForHighScore()));
	}
	

	int GetAccumulatedScore(){
		//Debug.Log ("At GameScoreHandler accumulated score = " + couchbaseDatabase.ReadDataAsString (userDefineKey.Total));
		return ConvertDataStoreReadingToInt(couchbaseDatabase.ReadDataAsString (userDefineKey.Total));
	}
	
	int ConvertDataStoreReadingToInt(string reading){
		int readingCount = 0;
		
		if (!string.IsNullOrEmpty (reading)) {
			readingCount = int.Parse(reading);
		}
		
		return readingCount;

	}

	
	void Start(){
		SetupDatabase ();
		SetupSocialMediaHandler ();

		userUUID = GetUserUUID();
		
		Debug.Log ("At GameScoreHandler::Start() UUID:" + userUUID);

	}

	void SetupDatabase(){
		if (couchbaseDatabaseObject != null) {
			couchbaseDatabase = (CouchbaseDatabase)couchbaseDatabaseObject.GetComponent (typeof(CouchbaseDatabase));
		}
	}

	void SetupSocialMediaHandler(){
		socialMediaHandlerObject = GameObject.Find ("SocialMediaHandlerObject");
		if (socialMediaHandlerObject != null) {
			socialMediaHandler = (SocialMediaHandler)socialMediaHandlerObject.GetComponent (typeof(SocialMediaHandler));
			socialMediaHandler.SetupSocialMediaAccount();
		}
	}

	string GetUserUUID(){

		FBUserAccount userAccount = new FBUserAccount (couchbaseDatabase);
		string GUUID = "";

		Debug.Log ("Social Media Handler is " + ((socialMediaHandler != null) ? "instatiated" : "null"));

		if (socialMediaHandler != null) {
			bool socialMediaCondition = socialMediaHandler.IsLoggedIn ();

			Debug.Log ("Social Media is " + ((socialMediaCondition) ? "logged in" : "not logged in"));

			userAccount.UserEmail = (socialMediaCondition) ? socialMediaHandler.GetAccountEmail () : UserAccountDefineKeys.TemporaryEmail;
			userAccount.UserID = (socialMediaCondition) ? socialMediaHandler.GetAccountID () : UserAccountDefineKeys.TemporaryID;

			if (string.IsNullOrEmpty (userAccount.GetUUID ())) {
				userAccount.Create ();
			}

			Debug.Log ("User Email: " + userAccount.UserEmail);
			Debug.Log ("User ID: " + userAccount.UserID);

			GUUID = userAccount.GetUUID ();
		}

		return GUUID;
	}

	
	string GetAdventureType(GameType type){
		string advetureTypeName = "";

		switch (type) {
			case GameType.MountainAdventure:
				advetureTypeName = "MA_";
				break;
			case GameType.CityAdventure:
				advetureTypeName = "CA_";
				break;
			case GameType.BeachAdventure:
				advetureTypeName = "BA_";
				break;
			case GameType.Endless:
				advetureTypeName = "EL_";
				break;
			default:
				break;
		}

		return advetureTypeName;
	}

	string GetFieldNameForHighScore(){
		return (gameType == GameType.Endless)? scoreFieldName:(userDefineKey.Level + Level.ToString () + ScoreFieldName);
	}
	
}
