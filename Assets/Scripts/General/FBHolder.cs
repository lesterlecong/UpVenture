﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using AssemblyCSharp;

public class FBHolder : MonoBehaviour {
	public Button loginButton;
	
	private UserDefineKeys userDefineKey;

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

	void OnLoggedIn(){
		Debug.Log ("Your Facebook is already logged in");
		loginButton.interactable = false;
		GetFBData();

		SavePreferences (userDefineKey.FBToken, GetAcessToken ());
		SavePreferences(userDefineKey.FBUserID, GetUserID());
	
	}
	

	public void loginFacebook(){
		FB.Login ("public_profile,email,user_friends", AuthCallback);
	}

	void AuthCallback(FBResult result){
		if (FB.IsLoggedIn){
			OnLoggedIn();
			//StartCoroutine(RegisterToCouchbase());
		}else{
			Debug.Log("Failed Logged in");
		}
	}

	public string GetUserID(){
		string userID = "None";
		if (FB.IsLoggedIn) {
			userID = FB.UserId;
		}

		return userID;
	}


	public void ShareHighscore(int score){
		FB.Feed (
			linkName: "Whooah I passed " + score.ToString() + " obstacle" + ((score > 1)? "s!":"!" + "\n Can you beat my score?"),
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

		SavePreferences (userDefineKey.FBEmail, email);
		SavePreferences (userDefineKey.FBUsername, userFullName);

	}

	string GetAcessToken(){
		string accessToken = "";
		if (FB.IsLoggedIn) {
			accessToken = FB.AccessToken;
		}
		return accessToken;
	}


	IEnumerator RegisterToCouchbase(){
		string syncGatewayURL = "http://localhost:4984/adventuredb/_facebook";
		System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding ();
		Dictionary<string, string> header = new Dictionary<string, string> ();
		string data = "{ \"access_token\":\"" + GetAcessToken() + "\"" + "," +
			"\"email\":\"" + email + "\"" + "," +
				"\"remote_url\":\"" + "http://localhost:8091\"" + "}";
		header.Add ("Content-Type", "application/json");
		header.Add ("Content-Length", data.Length.ToString());
		Debug.Log ("Data: " + data);
		
		WWW request = new WWW (syncGatewayURL, encoding.GetBytes (data), header);
		yield return request;
		
		if (request.error != null) {
			Debug.Log ("Error:" + request.error);
		} else {
			Debug.Log("Request Success");
			Debug.Log ("Data Returned:" + request.bytes.ToString());
			
			
		}

		
	}

	void SavePreferences(string key, string value){
		PlayerPrefs.SetString (key, value);
	}

}
