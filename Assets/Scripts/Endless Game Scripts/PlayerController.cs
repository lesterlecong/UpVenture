using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class PlayerController : MonoBehaviour {

	public float upForce;
	public float forwardSpeed;
	public bool isDead = false;
	public GameObject smoke;
	public Camera cam;
	public GameType gameType = GameType.Endless;
	public Canvas startGameCanvas;

	private float maxHeight;
	private GameController controller;
	private bool isStarted = false;
	private bool flap = false;


	void Start(){


		controller = GameControllerFactory.Instance ().controller (gameType);
		controller.Pause ();

		smoke.SetActive (false);

		GetComponent<Rigidbody2D> ().velocity = new Vector2 (forwardSpeed, 0);
		setupMaxHeight ();

	}

	void setupMaxHeight(){
		if (cam == null) {
			cam = Camera.main;
		}
		
		Vector3 upperCorner = new Vector3 (Screen.width, Screen.height, 0.0f);
		Vector3 targetWidth = cam.ScreenToWorldPoint (upperCorner);
		maxHeight = targetWidth.y;
	}

	void Update(){
		if (Input.anyKeyDown && !isStarted) {
			startGameCanvas.enabled = false;
			isStarted = true;
			controller.Play();
		}

		if (isDead)
			return;

		if (Input.anyKeyDown && GetComponent<Rigidbody2D> ().transform.position.y < maxHeight) {
			flap = true;

		}

	}

	void FixedUpdate(){
		if (controller.IsPaused ()) {
			GetComponent<Rigidbody2D> ().isKinematic = true;
		}else if(!GameController.current.IsPaused () ) {
			GetComponent<Rigidbody2D>().isKinematic = false;

			if(flap){
				MoveUp();
			}
		}
	}

	void MoveUp(){
		flap = false;
		GetComponent<Rigidbody2D> ().velocity = new Vector2 (GetComponent<Rigidbody2D> ().velocity.x, 0);
		GetComponent<Rigidbody2D> ().velocity = new Vector2 (forwardSpeed, 0);
		GetComponent<Rigidbody2D> ().AddForce (new Vector2 (0, upForce));
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Bird" || other.tag == "BirdGroup") {
			isDead = true;
			smoke.SetActive(true);
			smoke.GetComponent<Animator>().SetTrigger("smoke");
			controller.PlayerDied();
		}

		if (other.tag == "TopBorder") {
			flap = false;
		}
	}

}
