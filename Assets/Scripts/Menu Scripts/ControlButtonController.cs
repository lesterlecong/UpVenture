using UnityEngine;
using System.Collections;

public class ControlButtonController : MonoBehaviour {
	

	public void Back(){
		Application.LoadLevel ("adventure options");
	}

	public void GotoMenu(){
		Application.LoadLevel ("menu");
	}
}
