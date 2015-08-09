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
namespace AssemblyCSharp
{
	public class SocialMediaAccountFactory
	{
		static private SocialMediaAccountFactory instance = null;

		static public SocialMediaAccountFactory Instance(){
			if (instance == null) {
				instance = new SocialMediaAccountFactory();
			}

			return instance;
		}

		public SocialMediaAccount GetSocialMediaAccount(SocialMediaType socialMediaType){

			switch (socialMediaType) {
			case SocialMediaType.FACEBOOK:
				return new FacebookAccount();
			default:
				return null;
			}
		}

		private SocialMediaAccountFactory ()
		{
		}
	}
}
