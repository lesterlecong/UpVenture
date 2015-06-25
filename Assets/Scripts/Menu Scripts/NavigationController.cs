using UnityEngine;
using System.Collections;



public class NavigationController : MonoBehaviour {
	

	public void LoadScene(string SceneName){
		Application.LoadLevel (SceneName);
	}

	public void LoadScene(int SceneNumber){
		Application.LoadLevel (SceneNumber);
	}


}
