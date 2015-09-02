using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;
using ChartboostSDK;

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
	public AudioClip scoreSound;
	#endregion
	#region Protected Member Variables
	protected bool isGameOver = false;
	protected bool isPaused = false;
	protected Sprite gameOverSprite;
	protected GameObject gameAdsHandlerObject;
	protected AudioSource gameObjectAudioSource;
	protected GameObject facebookHandlerObject = null;
	protected FacebookHandler facebookHandler = null;
	protected GameAdMobHandler gameAdsHandler;
	protected const int adShowScaler = 5; 
	#endregion
	#region Private Member Variables
	private const string adShowCountKey = "AdShowCount";

	#endregion

	#region Public Method
	public void RestartScene(){
		Application.LoadLevel(Application.loadedLevel);

	}

	public void PlayerDied(){
		birdSpawner.StopSpawn ();
		isGameOver = true;
		
		int adShowCount = PlayerPrefs.HasKey (adShowCountKey) ? PlayerPrefs.GetInt (adShowCountKey) : 0;

		if (Chartboost.hasInterstitial (CBLocation.GameOver)) {
			Chartboost.showInterstitial (CBLocation.GameOver);
		} else {
			GameAdMobHandler.instance.ShowAds ();
		}

		//PlayerPrefs.SetInt(adShowCountKey, adShowCount + 1);

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
		GameAdMobHandler.instance.HideAds ();
	}

	public void ShowAdsOnPause(){
		GameAdMobHandler.instance.ShowAds ();
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

			SetupFacebookHandler();
		} else if (current != this) {
			Destroy(gameObject);
		}
		
		gameObjectAudioSource = gameObject.GetComponent<AudioSource> ();
		GameAdMobHandler.instance.HideAds ();
		Chartboost.setAutoCacheAds (true);
		gameOverObject.SetActive (false);
		pauseButton.gameObject.SetActive (true);
		playButton.gameObject.SetActive (false);
	}

	protected void PlayScoreSoundFX(){
		if (PlayerPrefs.GetInt (GameSystemDefineKeys.SoundFXState) == 1) {
			gameObjectAudioSource.clip = scoreSound;
			gameObjectAudioSource.Play ();
		}
	}
	#endregion

	#region Private Method

	private void SetupFacebookHandler(){
		facebookHandlerObject = GameObject.Find ("FacebookHandler");
		if (facebookHandlerObject != null) {
			facebookHandler = (FacebookHandler) facebookHandlerObject.GetComponent(typeof(FacebookHandler));
		}
	}
	#endregion
}
