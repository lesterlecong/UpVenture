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
		private static string fbEmail = "";
		private static string fbID = "";
		private static string fbName = "";

		public FacebookAccount ()
		{
		}

		public override void Initialized(){
			if (!FB.IsInitialized) {
				FB.Init (SetInit, OnHideUnity);
			}
		}
		public override void Login(){
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
			return fbEmail;
		}

		public override string GetAccountID(){
			return fbID;
		}

		public override string GetAccountName(){
			return fbName;
		}

		public string GetAcessToken(){
			string accessToken = "";

			if (FB.IsLoggedIn) {
				accessToken = FB.AccessToken;
			}

			return accessToken;
		}

		protected override void OnLoggedIn(){
			Debug.Log ("Your Facebook is already logged in");

			GetFacebookInfo ();

			if (socialMediaButton != null) {
				socialMediaButton.interactable = false;
			} else {
				Debug.Log ("Social Media Button is null");
			}
		}

		void SetInit(){
			if (FB.IsLoggedIn) {
				OnLoggedIn();
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
				Debug.Log("Failed Logged in");
			}
		}

		void GetFacebookInfo(){
			FB.API ("/me", Facebook.HttpMethod.GET, GetFacebookCallback);
		}

		void GetFacebookCallback(FBResult result){
			Debug.Log ("Result:" + result.Text);

			IDictionary fbdata = Facebook.MiniJSON.Json.Deserialize (result.Text) as IDictionary;

			if (fbdata != null) {
				Debug.Log ("Email:" + fbdata ["email"].ToString ());
				Debug.Log ("ID:" + fbdata ["id"].ToString ());
				Debug.Log ("Name:" + fbdata ["name"].ToString());

				fbEmail = fbdata ["email"].ToString ();
				fbID = fbdata ["id"].ToString ();
				fbName = fbdata ["name"].ToString ();

				ApplyLoginCallback();
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

