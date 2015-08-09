using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using AssemblyCSharp;

public class SocialMediaHandler : MonoBehaviour {


	public SocialMediaType socialMediaType = SocialMediaType.FACEBOOK;
	public Button socialMediaButton;

	private SocialMediaAccount socialMediaAccount;
	private static SocialMediaHandler instance;


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

	public void AddLoginCallback(LoginCallback callback){
		socialMediaAccount.AddLoginCallback (callback);
	}

	public void SetupSocialMediaAccount(){
		
		socialMediaAccount = SocialMediaAccountFactory.Instance ().GetSocialMediaAccount (socialMediaType);
		
		if (socialMediaAccount != null && socialMediaButton != null) {
			socialMediaAccount.SocialMediaButton (socialMediaButton);
			socialMediaButton.interactable = !socialMediaAccount.IsLoggedIn ();
		}
		
		if (socialMediaAccount != null) {
			if (!socialMediaAccount.IsLoggedIn ()) {
				socialMediaAccount.Initialized ();
			}
		} 
	}

	void Awake () {
		CheckInstance ();
		DontDestroyOnLoad (transform.gameObject);
	}


	void CheckInstance(){
		if (!instance) {
			instance = this;
		} else {
			Destroy(this.gameObject);
		}
	}



}
