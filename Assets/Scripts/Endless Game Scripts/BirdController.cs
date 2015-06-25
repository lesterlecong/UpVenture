using UnityEngine;
using System.Collections;


public class BirdController : MonoBehaviour {
	public float birdSpeed = -5.0f;

	void Start(){
		GetComponent<Rigidbody2D> ().velocity = new Vector2 (birdSpeed, 0);
	}
	void FixedUpdate () {
		if (!GameController.current.IsPaused ()) {
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (birdSpeed, 0);
		} else {
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 0);
		}
	}



	
}
