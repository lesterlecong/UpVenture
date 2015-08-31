using UnityEngine;
using System.Collections;

public class EndlessGameController : GameController {
	private int score = 0;
	private int previousGameLevel = 5;
	private GameScoreHandler scoreHandler;
	private string scoreField = "score";



	void Awake(){
		Initialized ();
	}
	
	void Start(){
		scoreHandler = (GameScoreHandler)gameScoreHandler.GetComponent (typeof(GameScoreHandler));
		scoreHandler.ScoreFieldName = scoreField;
		scoreHandler.InitGameScoreHandlerDocument ();
	}



	public override void PlayerScored(){
		if (isGameOver) {
			return;
		}
		PlayScoreSoundFX ();
		score++;
		
		if (score >= (previousGameLevel + 2)) {
			birdSpawner.increaseGameLevel ();
			previousGameLevel = score;
		}
		
		
		scoreText.text = "Score: " + score.ToString ();
	}
	
	public override void ShareScore(){
		if (facebookHandler != null) {
			if(!facebookHandler.IsLoggedIn()){
				facebookHandler.LoginFacebook();
			}
			facebookHandler.ShareMessage("Whooah I passed " + score.ToString() + " obstacle" + ((score > 1)? "s!":"!") + "\n Can you beat my score? #upventure");
		}
	}
	
	protected override void SetupGameOverObject(){
		currentScoreText.text = score.ToString ();
		highScoreText.text = scoreHandler.HighScore.ToString();
	}
	
	protected override void SaveToDatabase(){
		scoreHandler.HighScore = score;
	}
}
