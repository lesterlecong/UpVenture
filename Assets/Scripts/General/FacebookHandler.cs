using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using AssemblyCSharp;


public class FacebookHandler : MonoBehaviour {
	

	private string email;
	private string userFullName;
	
	
	void Awake(){
		
		
		FB.Init (SetInit, OnHideUnity);
	}
	
	void SetInit(){
		if (FB.IsLoggedIn) {
			OnLoggedIn();
		}
	}
	
	void OnHideUnity(bool isGameShown){
		Time.timeScale = (!isGameShown)? 0:1;
	}
	
	public bool IsLoggedIn(){
		return FB.IsLoggedIn;
	}
	
	void OnLoggedIn(){
		Debug.Log ("Your Facebook is already logged in");
		if (PlayerPrefs.GetString (UserAccountDefineKeys.FBLoginStatus) == UserAccountDefineKeys.FBIsLogout) {
			GetFBData ();
			SavePreferences (UserAccountDefineKeys.FBID, FB.UserId);
			SavePreferences (UserAccountDefineKeys.FBToken, FB.AccessToken);
		}
	}
	
	
	public void LoginFacebook(){
		FB.Login ("public_profile,email,user_friends", AuthCallback);
	}
	
	void AuthCallback(FBResult result){
		if (FB.IsLoggedIn){
			OnLoggedIn();
			SavePreferences(UserAccountDefineKeys.FBLoginStatus, UserAccountDefineKeys.FBIsLogin);
		}else{
			Debug.Log("Failed Logged in");
		}
	}

	public void ShareMessage(string message){
		FB.Feed (
			linkName: message,
			picture: "https://farm1.staticflickr.com/355/19332490282_5e3201c652_n.jpg",
			linkCaption: "Play this exciting Game!!!",
			link: "www.facebook.com/upventuregame"
			);
		
	}

	void GetFBData(){
		
		FB.API ("/me", Facebook.HttpMethod.GET, FBDataGetCallback);
		
	}
	
	void FBDataGetCallback(FBResult result){
		
		
		Debug.Log ("Result:" + result.Text);
		IDictionary fbdata = Facebook.MiniJSON.Json.Deserialize (result.Text) as IDictionary;
		Debug.Log ("Email:" + fbdata ["email"].ToString());
		email = fbdata ["email"].ToString ();
		userFullName = fbdata ["name"].ToString ();
		Debug.Log ("userFullName:" + fbdata ["name"].ToString());
		
		SavePreferences (UserAccountDefineKeys.FBEmail, email);
		SavePreferences (UserAccountDefineKeys.FBUsername, userFullName);
		
	}


	void SavePreferences(string key, string value){
		PlayerPrefs.SetString (key, value);
	}
}