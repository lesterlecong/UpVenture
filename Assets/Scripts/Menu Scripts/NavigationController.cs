using UnityEngine;
using System.Collections;
using AssemblyCSharp;


public class NavigationController : MonoBehaviour {
	

	public void LoadScene(string SceneName){
		UserDefineKeys defineKeys;
		PlayerPrefs.SetString (defineKeys.NextScene, SceneName);
		Application.LoadLevel (defineKeys.UpdateSceneName);
	}

	public void LoadScene(int SceneNumber){
		Application.LoadLevel (SceneNumber);
	}


}
