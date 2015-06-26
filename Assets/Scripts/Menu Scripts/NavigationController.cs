using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using PreviewLabs;

public class NavigationController : MonoBehaviour {


	public void LoadScene(string sceneName){
		UserDefineKeys defineKeys;
		NextSceneNameHolder.nextScene = sceneName;
		Application.LoadLevel (defineKeys.UpdateSceneName);
	}

	public void LoadScene(int SceneNumber){
		Application.LoadLevel (SceneNumber);
	}


}
