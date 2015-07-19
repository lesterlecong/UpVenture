using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;


public class GameController : MonoBehaviour {

	public GameObject gameScoreHandler;
	public static GameController current;
	public BirdSpawner birdSpawner;
	public GameObject gameOverObject;
	public Text scoreText;
	public Text currentScoreText;
	public Text highScoreText;

	public Button playButton;
	public Button pauseButton;

	protected bool isGameOver = false;
	protected bool isPaused = false;


	protected Sprite gameOverSprite;

	private int score = 0;
	private int previousGameLevel = 5;
	private GameScoreHandler scoreHandler;
	private string scoreField = "score";

	private GameObject fbObject;
	private FBHolder fbHolder;

	void Awake(){
		if (current == null) {
			current = this;
		} else if (current != this) {
			Destroy(gameObject);
		}

		gameOverObject.SetActive (false);
	}

	void Update(){


	}

	public void RestartScene(){
		Application.LoadLevel(Application.loadedLevel);
	}

	void Start () {
		SetupFBObject ();
		pauseButton.gameObject.SetActive (true);
		playButton.gameObject.SetActive (false);

		scoreHandler = (GameScoreHandler)gameScoreHandler.GetComponent (typeof(GameScoreHandler));
		scoreHandler.ScoreFieldName = scoreField;
		scoreHandler.InitGameScoreHandlerDocument ();
	}

	void SetupFBObject(){
		fbObject = GameObject.Find ("fbObject");
		if (fbObject != null) {
			fbHolder = (FBHolder) fbObject.GetComponent(typeof(FBHolder));
		}
	}

	public virtual void PlayerScored(){
		if (isGameOver) {
			return;
		}

		score++;

		if (score >= (previousGameLevel + 2)) {
			birdSpawner.increaseGameLevel();
			previousGameLevel = score;
		}


		scoreText.text = "Score: " + score.ToString();
	}

	public void PlayerDied(){
		birdSpawner.StopSpawn ();
		isGameOver = true;
		gameOverObject.SetActive (true);
		SaveToDatabase ();
		SetupGameOverObject ();

	}

	public void Pause(){
		isPaused = true;
		pauseButton.gameObject.SetActive (false);
		playButton.gameObject.SetActive (true);
	}

	public void Play(){
		isPaused = false;
		pauseButton.gameObject.SetActive (true);
		playButton.gameObject.SetActive (false);
	}

	public bool IsPaused(){
		return isPaused;
	}

	public void ShareScore(){
		if (fbHolder != null) {
			fbHolder.ShareHighscore (score);
		} else {
			Debug.LogError("FB Holder is null");
		}
	}

	protected virtual void SetupGameOverObject(){
		currentScoreText.text = score.ToString ();
		highScoreText.text = scoreHandler.HighScore.ToString();
	}

	protected virtual void SaveToDatabase(){
		scoreHandler.HighScore = score;
	}


	

}
