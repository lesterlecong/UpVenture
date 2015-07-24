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

namespace AssemblyCSharp
{
	public class UserAccountFacebookDataHandler: IUserAccountDataHandler
	{
		private SocialMediaHandler socialMediaAccount;
		private CouchbaseDatabase couchDatabase;
		private FBUserAccount fbUserAccount = new FBUserAccount();

		public UserAccountFacebookDataHandler ()
		{

		}

		public void ChangeData(){
			Debug.Log ("At UserAccountFacebookDataHandler: ChangeData()");
			if (socialMediaAccount != null &&  couchDatabase != null) {
				string UUID = GetUUIDofTemporaryUserAccount();
				if(!string.IsNullOrEmpty(UUID)){
					fbUserAccount.UserID = socialMediaAccount.GetAccountID();
					fbUserAccount.UserEmail = socialMediaAccount.GetAccountEmail();
					fbUserAccount.Update(UUID);
				}
			}
		}

		public void SocialMediaObject(GameObject socialMediaObject){
			if (socialMediaObject != null) {
				socialMediaAccount = (SocialMediaHandler) socialMediaObject.GetComponent(typeof(SocialMediaHandler));
			}
		}

		public void SetDatabaseObject(GameObject databaseObject){
			couchDatabase = (CouchbaseDatabase)databaseObject.GetComponent (typeof(CouchbaseDatabase));
			fbUserAccount.SetDatabase(couchDatabase);
		}

		string GetUUIDofTemporaryUserAccount(){
			fbUserAccount.UserID = UserAccountDefineKeys.TemporaryID;
			fbUserAccount.UserEmail = UserAccountDefineKeys.TemporaryEmail;
			return fbUserAccount.GetUUID ();
		}
	}
}

