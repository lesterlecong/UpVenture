using UnityEngine;
using System.Collections;

public class PlayerSoundFXHandler : MonoBehaviour {

	public AudioClip upSound;
	public AudioClip gameOverSound;
	private AudioSource audioSource;

	private static bool gameOverSoundFxAlreadyPlay = false;

	void Awake(){
		gameOverSoundFxAlreadyPlay = false;
	}

	void Start () {
		audioSource = gameObject.GetComponent<AudioSource> ();
	}
	
	void Update () {
		if (Input.anyKeyDown) {
			PlayClip (upSound);
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Bird" || other.tag == "BirdGroup") {
			PlayClip (gameOverSound);
			gameOverSoundFxAlreadyPlay = true;
		} 
		if (other.tag == "BottomBorder" && !gameOverSoundFxAlreadyPlay) {
			PlayClip (gameOverSound);
		}
	}
	
	void PlayClip(AudioClip clip){
		audioSource.clip = clip;
		audioSource.Play();
	}

}
