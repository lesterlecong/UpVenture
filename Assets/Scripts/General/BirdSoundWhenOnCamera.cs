using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class BirdSoundWhenOnCamera : MonoBehaviour {
	
	public AudioClip birdSound;

	private AudioSource audioSource;

	void Start(){
		audioSource = gameObject.GetComponent<AudioSource> ();
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Bird") {
			ActivateSound(birdSound);

			//ActivateSound(birdFlap);

		}
	}

	void ActivateSound(AudioClip audioClip){
		if (PlayerPrefs.GetInt (GameSystemDefineKeys.SoundFXState) == 1) {
			audioSource.clip = audioClip;
			audioSource.Play ();
		}
	}

	void DeactivateSound(){
		audioSource.Stop ();
	}
	
}
