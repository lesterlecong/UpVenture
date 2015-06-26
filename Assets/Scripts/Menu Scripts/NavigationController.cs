using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using PreviewLabs;

public class NavigationController : MonoBehaviour {
	

	public void LoadScene(string SceneName){
		UserDefineKeys defineKeys;
		PreviewLabs.PlayerPrefs.SetString (defineKeys.NextScene, SceneName);
		PreviewLabs.PlayerPrefs.Flush ();
		Application.LoadLevel (defineKeys.UpdateSceneName);
	}

	public void LoadScene(int SceneNumber){
		Application.LoadLevel (SceneNumber);
	}


}
