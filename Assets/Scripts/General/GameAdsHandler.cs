using UnityEngine;
using System.Collections;

using GoogleMobileAds.Api;

public class GameAdsHandler : MonoBehaviour {

	public static GameAdsHandler current;
	public string adMobAndroidUnitID = "";
	public string adMobiOSUnitID = "";
	public bool isTesting = false;
	public AdPosition position = AdPosition.Bottom;

	private static BannerView bannerView = null;
	private static AdRequest request = null;

	// Use this for initialization
	void Start () {
		if (current == null) {
			current = this;
			SetupAds ();
		} else if (current != this) {
			Destroy(gameObject);
		}
	}

	void OnDestroy(){
		bannerView.Destroy ();
	}

	void SetupAds(){

		#if UNITY_ANDROID
		string adUnitId =   adMobAndroidUnitID;// "ca-app-pub-6867538652048315/8768168980";
		#elif UNITY_IPHONE
		string adUnitId = adMobiOSUnitID;
		#else
		string adUnitId = "unexpected_platform";
		#endif

		if (bannerView == null) {	
			bannerView = new BannerView (adUnitId, AdSize.Banner, position);
			if (isTesting) {
				request = new AdRequest.Builder ()
					.AddTestDevice (AdRequest.TestDeviceSimulator)
					.AddTestDevice ("36086015333251D9")
					.AddTestDevice ("571260770879168")
					.AddTestDevice("351811072354090")
					.AddKeyword("Upventure")
					.SetGender(Gender.Male)
					.AddExtra("color_bg", "9B30FF")
					.Build ();
			} else {
				request = new AdRequest.Builder ().Build ();
			}
			bannerView.LoadAd (request);
			HideAds();
		}
	}


	public void ShowAds(){
		if (bannerView == null) {
			SetupAds();
		}
			bannerView.Show ();
		
	}

	public void HideAds(){
		if (bannerView == null) {
			SetupAds();
		}
			bannerView.Hide ();

		
	}
}
