using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using AssemblyCSharp;

public class SocialMediaHandler : MonoBehaviour {

	public Button socialMediaButton;
	public SocialMediaType socialMediaType = SocialMediaType.FACEBOOK;

	private SocialMediaAccount socialMediaAccount;

	public void Login(){
		socialMediaAccount.Login ();
	}

	public bool IsLoggedIn(){
		return socialMediaAccount.IsLoggedIn ();
	}

	public void ShareMessage(string message){
		socialMediaAccount.ShareMessage (message);
	}

	public string GetAccountEmail(){
		return socialMediaAccount.GetAccountEmail ();
	}

	public string GetAccountID(){
		return socialMediaAccount.GetAccountID ();
	}

	public string GetAccountName(){
		return socialMediaAccount.GetAccountName ();
	}

	void Awake () {
		DontDestroyOnLoad (transform.gameObject);
		socialMediaAccount = SocialMediaAccountFactory.Instance ().GetSocialMediaAccount (socialMediaType);
		if (socialMediaAccount != null && socialMediaButton != null) {
			Debug.Log ("SocialMediaHandler awake");
			if (!socialMediaAccount.IsLoggedIn ()) {
				Debug.Log ("SocialMediaHandler::Initialized");
				socialMediaAccount.SocialMediaButton (socialMediaButton);
				socialMediaAccount.Initialized ();
			}else {
				socialMediaButton.interactable = false;
			}
		} 
	}

}
