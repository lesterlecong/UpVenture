using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using AssemblyCSharp;
using Couchbase.Lite;
using Couchbase.Lite.Unity;
using Newtonsoft;

public class GameScoreHandler : MonoBehaviour {

	public AdventureType adventureType;
	public GameObject couchbaseDatabaseObject;
	
	private string scoreFieldName;
	private int level;
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
				couchbaseDatabase.saveData(GetFieldNameForHighScore(), value.ToString());
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
				couchbaseDatabase.saveData(userDefineKey.Total, accumulatedScore.ToString());
				Debug.Log("Save accumulated score:" + accumulatedScore.ToString());
			}
		}

		get{
			return GetAccumulatedScore();
		}
	}

	public int ScoreRequired{
		set{
			couchbaseDatabase.saveData(userDefineKey.Level + Level.ToString () + userDefineKey.RequiredScore, value.ToString()); 
		}
		get{
			return ConvertDataStoreReadingToInt(couchbaseDatabase.readDataAsString(userDefineKey.Level + Level.ToString () + userDefineKey.RequiredScore));
		}

	}
	
	public void initGameScoreHandlerDocument(){
		couchbaseDatabase.DocumentID = GetAdventureType(adventureType) + userUUID;
		couchbaseDatabase.CreateDocument ();
	}

	int GetPreviousScore(){
		Debug.Log ("At GameScoreHandler::GetPreviousScore Field: " + GetFieldNameForHighScore ());
		return ConvertDataStoreReadingToInt(couchbaseDatabase.readDataAsString (GetFieldNameForHighScore()));
	}
	

	int GetAccumulatedScore(){
		Debug.Log ("At GameScoreHandler accumulated score = " + couchbaseDatabase.readDataAsString (userDefineKey.Total));
		return ConvertDataStoreReadingToInt(couchbaseDatabase.readDataAsString (userDefineKey.Total));
	}
	
	int ConvertDataStoreReadingToInt(string reading){
		int readingCount = 0;
		
		if (!string.IsNullOrEmpty (reading)) {
			readingCount = int.Parse(reading);
		}
		
		return readingCount;

	}

	
	void Start(){
		level = 0;
		couchbaseDatabase = (CouchbaseDatabase)couchbaseDatabaseObject.GetComponent (typeof(CouchbaseDatabase));
		couchbaseDatabase.StartCouchbase ();
		userUUID = couchbaseDatabase.GetUUID ();
		Debug.Log ("At GameScoreHandler: " + userUUID);

	}


	
	string GetAdventureType(AdventureType type){
		string advetureTypeName = "";

		switch (type) {
			case AdventureType.MountainAdventure:
				advetureTypeName = "MA_";
				break;
			case AdventureType.CityAdventure:
				advetureTypeName = "CA_";
				break;
			case AdventureType.BeachAdventure:
				advetureTypeName = "BA_";
				break;
			case AdventureType.Endless:
				advetureTypeName = "EL_";
				break;
			default:
				break;
		}

		return advetureTypeName;
	}

	string GetFieldNameForHighScore(){
		return (adventureType == AdventureType.Endless)? scoreFieldName:(userDefineKey.Level + Level.ToString () + ScoreFieldName);
	}
	
}
