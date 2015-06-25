using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class PlayerPass : MonoBehaviour {

	public GameType gameType = GameType.Endless;
	
	void OnTriggerEnter2D(Collider2D other)
	{

		if (other.tag == "Player") {
			GameControllerFactory.Instance().controller(gameType).PlayerScored ();
		}
	}
}
