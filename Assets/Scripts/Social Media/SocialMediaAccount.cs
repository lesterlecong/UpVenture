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

namespace AssemblyCSharp
{
	public abstract class SocialMediaAccount
	{
		protected Button socialMediaButton;

		public abstract void Initialized();
		public abstract void Login();
		public abstract bool IsLoggedIn();
		public abstract void ShareMessage(string message);
		public abstract string GetAccountEmail();
		public abstract string GetAccountID();
		
		public void SocialMediaButton(Button socialMediaButton){
			this.socialMediaButton = socialMediaButton;
		}

		protected abstract void OnLoggedIn();
	}
}

