using UnityEngine;
using System.Collections;

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
		audioSource.clip = audioClip;
		audioSource.Play ();
	}

	void DeactivateSound(){
		audioSource.Stop ();
	}
	
}
