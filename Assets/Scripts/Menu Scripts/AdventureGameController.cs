﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine.UI;
using AssemblyCSharp;



public class AdventureGameController : GameController {
	
	public Image[] scoreStar;
	public Image[] afterScoreStar;
	public int level;
	public Button forwardButton;
	public Image gameOverBackground;
	public AudioClip perfectScoreSound;

	private Sprite perfectPanelSprite;
	private int starCount = 0;
	private UserDefineKeys userDefineKey;
	private GameScoreHandler scoreHandler;

	void Awake(){
		Initialized ();
	}

	void Start () {

		pauseButton.gameObject.SetActive (true);
		playButton.gameObject.SetActive (false);

		for (int count = 0; count < scoreStar.Length; count++) {
			scoreStar[count].enabled = false;
			afterScoreStar[count].enabled = false;
		}

		scoreHandler = (GameScoreHandler)gameScoreHandler.GetComponent (typeof(GameScoreHandler));
		scoreHandler.ScoreFieldName = GameScoreDefineKeys.ScoreName;
		scoreHandler.Level = level;
		scoreHandler.InitGameScoreHandlerDocument ();

		Debug.Log ("Accumulated Score:" + scoreHandler.AccumulatedScore.ToString ());

		SetupGameOverPanel ();
	}

	void SetupGameOverPanel(){
		perfectPanelSprite = Resources.Load<Sprite> ("perfect score");
		gameOverSprite = Resources.Load<Sprite> ("game over");
		
		gameOverBackground.sprite = gameOverSprite;
	}

	void PlayPerfectScoreSoundFX(){
		if (PlayerPrefs.GetInt (GameSystemDefineKeys.SoundFXState) == 1) {
			gameObjectAudioSource.clip = perfectScoreSound;
			gameObjectAudioSource.Play ();
		}
	}

	public override void PlayerScored(){
		if (isGameOver) {
			return;
		}
		PlayScoreSoundFX ();
		scoreStar [starCount].enabled = true;
		starCount++;

		if (starCount >=  (scoreStar.Length)) {
			PlayPerfectScoreSoundFX();
			gameOverBackground.sprite = perfectPanelSprite;
			this.PlayerDied();
		}

	}


	protected override void SetupGameOverObject(){

		scoreHandler.Level = level + 1;
		bool isInteractable = (scoreHandler.AccumulatedScore >= scoreHandler.ScoreRequired);
		forwardButton.interactable = isInteractable;
		Debug.Log ("Score Required for Next Level: " + scoreHandler.ScoreRequired);
		scoreHandler.Level = level;


		for (int score = 0; score<starCount; score++) {
			afterScoreStar[score].enabled = true;
		}

	}

	protected override void SaveToDatabase(){
		scoreHandler.AccumulatedScore = starCount;
		scoreHandler.HighScore = starCount;
	}
}
