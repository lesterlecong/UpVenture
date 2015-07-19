using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;

public class StarCountActivator : MonoBehaviour {

	public Image[] starImages;
	public GameObject gameScoreHandler;
	public int level;
	public Text scoreUpdate;
	public int scoreRequired;
	public Image lockSystem;

	private UserDefineKeys userDefineKey;
	private GameScoreHandler scoreHandler;
	private bool isLock = true;

	void Start(){

		for (int index = 0; index < starImages.Length; index++) {
			starImages[index].enabled = false;
		}

		scoreHandler = (GameScoreHandler)gameScoreHandler.GetComponent (typeof(GameScoreHandler));
		scoreHandler.ScoreFieldName = userDefineKey.Score;;
		scoreHandler.Level = level;
		scoreHandler.ScoreRequired = scoreRequired;
		scoreHandler.InitGameScoreHandlerDocument ();

		UnlockLevel ();
		if (!isLock) {
			StarCount (scoreHandler.HighScore);
		}

	}


	void StarCount(int count){
		for (int index = 0; index < count; index++) {
			starImages[index].enabled = true;
		}
	}

	void UnlockLevel(){

		int accumulatedScore = scoreHandler.AccumulatedScore;
		scoreUpdate.text = accumulatedScore.ToString () + "/" + scoreRequired.ToString ();
		if (accumulatedScore >= scoreRequired) {
			lockSystem.gameObject.SetActive (false);
			GetComponent<Button> ().interactable = true;
			isLock = false;
		}
	}
}
