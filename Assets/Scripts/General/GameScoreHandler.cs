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
				couchbaseDatabase.SaveData(GameScoreDefineKeys.Total, accumulatedScore.ToString());
				Debug.Log("Save accumulated score:" + accumulatedScore.ToString());
			}
		}

		get{
			return GetAccumulatedScore();
		}
	}

	public int ScoreRequired{
		set{
			couchbaseDatabase.SaveData(GameScoreDefineKeys.Level + Level.ToString () + GameScoreDefineKeys.RequiredScore, value.ToString()); 
		}
		get{
			return ConvertDataStoreReadingToInt(couchbaseDatabase.ReadDataAsString(GameScoreDefineKeys.Level + Level.ToString () + GameScoreDefineKeys.RequiredScore));
		}

	}

	public void InitGameScoreHandlerDocument(){
		couchbaseDatabase.CreateDocumentWithID(GetAdventureType(gameType) + userUUID);
		couchbaseDatabase.SelectDocumentWithID(GetAdventureType(gameType) + userUUID);
	}

	int GetPreviousScore(){
		//Debug.Log ("At GameScoreHandler::GetPreviousScore Field: " + GetFieldNameForHighScore ());
		return ConvertDataStoreReadingToInt(couchbaseDatabase.ReadDataAsString (GetFieldNameForHighScore()));
	}
	

	int GetAccumulatedScore(){
		//Debug.Log ("At GameScoreHandler accumulated score = " + couchbaseDatabase.ReadDataAsString (userDefineKey.Total));
		return ConvertDataStoreReadingToInt(couchbaseDatabase.ReadDataAsString (GameScoreDefineKeys.Total));
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

		userUUID = GetUserUUID();
		
		Debug.Log ("At GameScoreHandler::Start() UUID:" + userUUID);

	}

	void SetupDatabase(){
		if (couchbaseDatabaseObject != null) {
			couchbaseDatabase = (CouchbaseDatabase)couchbaseDatabaseObject.GetComponent (typeof(CouchbaseDatabase));
		}
	}

	string GetUserUUID(){

		FBUserAccount userAccount = new FBUserAccount (couchbaseDatabase);
		string GUUID = "";
		
		userAccount.UserEmail = UserAccountDefineKeys.TemporaryEmail;
		userAccount.UserID =  UserAccountDefineKeys.TemporaryID;

		if (string.IsNullOrEmpty (userAccount.GetUUID ())) {
			userAccount.Create ();
		}

		Debug.Log ("User Email: " + userAccount.UserEmail);
		Debug.Log ("User ID: " + userAccount.UserID);

		GUUID = userAccount.GetUUID ();
		
		return GUUID;
	}

	
	string GetAdventureType(GameType type){
		string advetureTypeName = "";

		switch (type) {
			case GameType.MountainAdventure:
				advetureTypeName = GameScoreDefineKeys.MountainAdventure;
				break;
			case GameType.CityAdventure:
				advetureTypeName = GameScoreDefineKeys.CityAdventure;
				break;
			case GameType.BeachAdventure:
				advetureTypeName = GameScoreDefineKeys.BeachAdventure;
				break;
			case GameType.Endless:
				advetureTypeName = GameScoreDefineKeys.EndlessAdventure;
				break;
			default:
				break;
		}

		return advetureTypeName;
	}

	string GetFieldNameForHighScore(){
		return (gameType == GameType.Endless)? scoreFieldName:(GameScoreDefineKeys.Level + Level.ToString () + GameScoreDefineKeys.ScoreName);
	}
	
}
