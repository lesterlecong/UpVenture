using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;

public class AccumulatedScoreViewer : MonoBehaviour {

	public GameObject gameScoreHandler;
	public GameObject previousgameScoreHandler;
	public Text scoreText;
	public Text scoreToUnlockText;
	public GameObject lockSystem;
	public int requiredScore;

	private UserDefineKeys userDefineKey;
	private GameScoreHandler scoreHandler;
	private GameScoreHandler previousScoreHandler;
	private int previousScore;
	private int score;

	void Awake(){

	}

	void Start(){
		scoreHandler = (GameScoreHandler)gameScoreHandler.GetComponent (typeof(GameScoreHandler));
		scoreHandler.ScoreFieldName = userDefineKey.Score;
		scoreHandler.initGameScoreHandlerDocument ();
		score = scoreHandler.AccumulatedScore;
		
		previousScoreHandler = (GameScoreHandler)previousgameScoreHandler.GetComponent (typeof(GameScoreHandler));
		previousScoreHandler.ScoreFieldName = userDefineKey.Score;
		previousScoreHandler.initGameScoreHandlerDocument ();
		previousScore = previousScoreHandler.AccumulatedScore;
		Debug.Log ("At AccumulatedScoreViewer total score = " + scoreHandler.AccumulatedScore.ToString ());

		if (requiredScore > 0) {
			if(isUnlock()){
				Unlock();
				DisplayAccumulatedScore();
			}else{
				DisplayScoreRequiredToUnlock();
			}
		} else {
			Unlock();
			DisplayAccumulatedScore();
		}



	}

	bool isUnlock(){
		return (previousScore >= requiredScore);
	}

	void Unlock(){
		scoreText.gameObject.SetActive(true);
		lockSystem.SetActive (false);
		GetComponent<Button> ().interactable = true;
	}

	void DisplayScoreRequiredToUnlock(){
		scoreToUnlockText.text = previousScore.ToString() + "/" + requiredScore.ToString();
	}

	void DisplayAccumulatedScore(){
		scoreText.text = score.ToString () + "/100";
	}

}
