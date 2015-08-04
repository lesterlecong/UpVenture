//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public class FacebookAccount: SocialMediaAccount
	{
		private bool isSocialMediaButtonPressed = false;
		private GameObject debugText;

		public FacebookAccount ()
		{
			if (PlayerPrefs.GetString (UserAccountDefineKeys.FBLoginStatus) == UserAccountDefineKeys.FBIsLogout) {
				PlayerPrefs.SetString (UserAccountDefineKeys.FBID, String.Empty);
				PlayerPrefs.SetString (UserAccountDefineKeys.FBEmail, String.Empty);
				PlayerPrefs.SetString (UserAccountDefineKeys.FBUsername, String.Empty);
			}

			OnScreenLog.write ("Facebook Initialize");
		}

		public override void Initialized(){
			if (!FB.IsInitialized) {
				FB.Init (SetInit, OnHideUnity);
			}
		}
		public override void Login(){
			Debug.Log ("Social Media button is pressed");
			isSocialMediaButtonPressed = true;
			FB.Login ("public_profile,email,user_friends", LoginCallback);
		}

		public override bool IsLoggedIn(){
			return FB.IsLoggedIn; 
		}

		public override void ShareMessage(string message){
			if (FB.IsLoggedIn) {
				FB.Feed (
					linkName: message,
					picture: "https://farm1.staticflickr.com/355/19332490282_5e3201c652_n.jpg",
					linkCaption: "Play this exciting Game!!!",
					link: "www.facebook.com/upventuregame"
					);
			}
		}

		public override string GetAccountEmail(){
			return PlayerPrefs.GetString(UserAccountDefineKeys.FBEmail);
		}

		public override string GetAccountID(){
			return PlayerPrefs.GetString(UserAccountDefineKeys.FBID);
		}

		public override string GetAccountName(){
			return PlayerPrefs.GetString(UserAccountDefineKeys.FBUsername);
		}

		public string GetAcessToken(){
			string accessToken = "";

			if (FB.IsLoggedIn) {
				accessToken = FB.AccessToken;
			}

			return accessToken;
		}

		protected override void OnLoggedIn(){
			OnScreenLog.write ("FB is logged in");

			if (socialMediaButton != null) {
				socialMediaButton.interactable = false;
			}

			GetFacebookInfo ();


		}

		void SetInit(){
			if (FB.IsLoggedIn) {
				OnLoggedIn();
			}

			if ((PlayerPrefs.GetString (UserAccountDefineKeys.FBLoginStatus) == UserAccountDefineKeys.FBIsLogin) && socialMediaButton != null) {
				OnScreenLog.write("FB is already logged in before");
				socialMediaButton.interactable = false;
			}
		}

		void OnHideUnity(bool isGameShown){
			Time.timeScale = (!isGameShown)? 0:1;
		}

		void LoginCallback(FBResult result){
			Debug.Log ("LoginCallback:" + result.Text);
			if (FB.IsLoggedIn){
				OnLoggedIn();
				PlayerPrefs.SetString(UserAccountDefineKeys.FBLoginStatus, UserAccountDefineKeys.FBIsLogin);
			}else{
				OnScreenLog.write ("Error in logging in FB");
			}
		}

		void GetFacebookInfo(){
			FB.API ("/me", Facebook.HttpMethod.GET, GetFacebookInfoCallback);
		}

		void GetFacebookInfoCallback(FBResult result){
			Debug.Log ("Result:" + result.Text);
			OnScreenLog.write ("Login Result: " + result.Text);

			IDictionary fbdata = Facebook.MiniJSON.Json.Deserialize (result.Text) as IDictionary;

			if (fbdata != null) {
				OnScreenLog.write ("ID:" + fbdata ["id"].ToString ());
				//OnScreenLog.write ("Email:" + fbdata ["email"].ToString ());
				OnScreenLog.write ("Name:" + fbdata ["name"].ToString ());

				PlayerPrefs.SetString (UserAccountDefineKeys.FBID, fbdata ["id"].ToString ());
				//PlayerPrefs.SetString (UserAccountDefineKeys.FBEmail, fbdata ["email"].ToString ());
				PlayerPrefs.SetString (UserAccountDefineKeys.FBUsername, fbdata ["name"].ToString ());

				if (isSocialMediaButtonPressed) {
					ApplyLoginCallback ();
				}
			} else {
				OnScreenLog.write("FB Data is Null");
			}
		}

	
		void ApplyLoginCallback(){
			if (loginCallbackList.Count > 0) {
				foreach(LoginCallback callback in loginCallbackList){
					callback();
				}
			}
		}
	
	}
}

