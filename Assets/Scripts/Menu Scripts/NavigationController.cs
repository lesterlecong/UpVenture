using UnityEngine;
using System.Collections;
using AssemblyCSharp;


public class NavigationController : MonoBehaviour {


	public void LoadScene(string sceneName){
		UserDefineKeys defineKeys;
		PlayerPrefs.SetString (defineKeys.NextScene, sceneName);
		Application.LoadLevel (defineKeys.UpdateSceneName);
	}

	public void LoadScene(int SceneNumber){
		Application.LoadLevel (SceneNumber);
	}

	public void OpenURL(string url){
		Application.OpenURL (url);
	}
}
