using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;


public class GameController : MonoBehaviour {

	#region Public Member Variables
	public GameObject gameScoreHandler;
	public static GameController current;
	public BirdSpawner birdSpawner;
	public GameObject gameOverObject;
	public Text scoreText;
	public Text currentScoreText;
	public Text highScoreText;
	public Button playButton;
	public Button pauseButton;
	#endregion
	#region Protected Member Variables
	protected bool isGameOver = false;
	protected bool isPaused = false;
	protected Sprite gameOverSprite;
	protected GameObject gameAdsHandlerObject;
	protected GameAdsHandler gameAdsHandler;

	#endregion

	#region Public Method
	public void RestartScene(){
		Application.LoadLevel(Application.loadedLevel);
	}

	public void PlayerDied(){
		birdSpawner.StopSpawn ();
		isGameOver = true;

		if (gameAdsHandlerObject != null && gameAdsHandler != null) {
			gameAdsHandlerObject.SetActive(true);
			gameAdsHandler.ShowAds();
		}

		gameOverObject.SetActive (true);

		SaveToDatabase ();
		SetupGameOverObject ();
	}

	public void Pause(){
		isPaused = true;
		pauseButton.gameObject.SetActive (false);
		playButton.gameObject.SetActive (true);

		if (gameAdsHandlerObject != null && gameAdsHandler != null) {
			gameAdsHandlerObject.SetActive(true);
			gameAdsHandler.ShowAds();
		}
	}

	public void Play(){
		isPaused = false;
		pauseButton.gameObject.SetActive (true);
		playButton.gameObject.SetActive (false);

		if (gameAdsHandlerObject != null && gameAdsHandler != null) {
			gameAdsHandlerObject.SetActive(false);
			gameAdsHandler.HideAds();
		}
	}
	
	public bool IsPaused(){
		return isPaused;
	}

	#endregion

	#region Public Virtual Method

	public virtual void ShareScore(){
		
	}

	public virtual void PlayerScored(){
	}

	#endregion

	#region Protected Virtual Method

	
	protected virtual void SetupGameOverObject(){ 
		
	}
	
	protected virtual void SaveToDatabase(){
		
	}

	#endregion

	#region Protected Method
	protected void Initialized(){
		if (current == null) {
			current = this;
			SetupGameAds();
		} else if (current != this) {
			Destroy(gameObject);
		}
		
		gameOverObject.SetActive (false);
		pauseButton.gameObject.SetActive (true);
		playButton.gameObject.SetActive (false);
	}
	#endregion

	#region Private Method
	private void SetupGameAds(){
		gameAdsHandlerObject = GameObject.Find ("GameAdsHandler");
		if (gameAdsHandlerObject != null) {
			gameAdsHandler = (GameAdsHandler) gameAdsHandlerObject.GetComponent(typeof(GameAdsHandler));
			gameAdsHandlerObject.SetActive(false);
			
		}
	}
	#endregion
}
