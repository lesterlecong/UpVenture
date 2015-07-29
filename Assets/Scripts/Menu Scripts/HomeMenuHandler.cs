using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HomeMenuHandler : MonoBehaviour {

	public Button socialMediaButton;
	private GameObject socialMediaHandlerObject;
	private SocialMediaHandler socialMediaHandler;

	void Awake () {
		socialMediaHandlerObject = GameObject.Find ("SocialMediaHandlerObject");
		socialMediaHandler = (SocialMediaHandler)socialMediaHandlerObject.GetComponent (typeof(SocialMediaHandler));
		socialMediaHandler.socialMediaButton = this.socialMediaButton;
		socialMediaHandler.SetupSocialMediaAccount ();
	}

}
