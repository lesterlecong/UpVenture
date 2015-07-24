using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using AssemblyCSharp;

public class FacebookButtonHandler : MonoBehaviour {

	public SocialMediaType socialMediaType = SocialMediaType.FACEBOOK;
	private GameObject socialMediaHandlerObject;
	private SocialMediaHandler socialMediaHandler;

	void Awake(){
		SetupSocialMediaHandler ();
		SetButtonListener ();
	}

	void SetupSocialMediaHandler(){
		socialMediaHandlerObject = GameObject.Find ("SocialMediaHandlerObject");
		if (socialMediaHandlerObject != null) {
			socialMediaHandler = (SocialMediaHandler) socialMediaHandlerObject.GetComponent(typeof(SocialMediaHandler));
			if(socialMediaHandler != null){
				socialMediaHandler.socialMediaType = this.socialMediaType;
			}
		}
	}

	void SetButtonListener(){
		Button facebookButton = transform.GetComponent<Button> ();
		facebookButton.onClick.AddListener (() => {
			LoginFacebook (); 
		});
	}
	
	void LoginFacebook(){
		socialMediaHandler.Login ();
	}
}
