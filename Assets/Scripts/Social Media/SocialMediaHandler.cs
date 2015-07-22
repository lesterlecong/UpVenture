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
		socialMediaAccount = SocialMediaAccountFactory.Instance ().GetSocialMediaAccount (socialMediaType);
		if (socialMediaAccount != null) {
			socialMediaAccount.SocialMediaButton(socialMediaButton);
			socialMediaAccount.Initialized();
		}
	}

}
