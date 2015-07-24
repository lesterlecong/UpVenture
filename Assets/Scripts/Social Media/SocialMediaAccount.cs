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
using UnityEngine.UI;
using System.Collections.Generic;

public delegate void LoginCallback();

namespace AssemblyCSharp
{
	public abstract class SocialMediaAccount
	{
		protected Button socialMediaButton;
		protected List<LoginCallBack> loginCallbackList;

		public abstract void Initialized();
		public abstract void Login();
		public abstract bool IsLoggedIn();
		public abstract void ShareMessage(string message);
		public abstract string GetAccountEmail();
		public abstract string GetAccountID();
		public abstract string GetAccountName();

		public void SocialMediaButton(Button socialMediaButton){
			this.socialMediaButton = socialMediaButton;
		}

		public void AddLoginCallback(LoginCallback callback){
			loginCallbackList.Add (callback);
		}
		protected abstract void OnLoggedIn();
	}
}

