using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HomeMenuInvoker : MonoBehaviour {

	public string homeMenuSceneName ="home menu";

	void Start(){
		Invoke ("InvokeHomeMenu", 2.0f);
	}


	void InvokeHomeMenu(){
		Application.LoadLevel (homeMenuSceneName);
	}
}
