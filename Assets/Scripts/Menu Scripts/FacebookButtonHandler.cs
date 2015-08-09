using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using AssemblyCSharp;

public class FacebookButtonHandler : MonoBehaviour {

	public SocialMediaType socialMediaType = SocialMediaType.FACEBOOK;
	public GameObject couchbaseDatabaseObject;
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
			SetFacebookCallback();
			LoginFacebook (); 
		});
	}
	
	void LoginFacebook(){
		socialMediaHandler.Login ();
	}

	void SetFacebookCallback(){
		IUserAccountDataHandler userAccountDataHandler = UserAccountDataHandlerFactory.Instance ().GetDataHandler (socialMediaType);
		userAccountDataHandler.SetDatabaseObject (couchbaseDatabaseObject);
		userAccountDataHandler.SocialMediaObject (socialMediaHandlerObject);
		socialMediaHandler.AddLoginCallback (userAccountDataHandler.ChangeData);
	
	}
}
