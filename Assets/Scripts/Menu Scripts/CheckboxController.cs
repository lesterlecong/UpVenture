using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using AssemblyCSharp;

public class CheckboxController : MonoBehaviour {

	public Toggle soundeffectsToggle;

	private static bool soundeffectsStatus = true;

	void Start(){
		soundeffectsToggle.isOn = (PlayerPrefs.GetInt (GameSystemDefineKeys.SoundFXState) == 1);
	}

	public void SoundEffectsSettings(){
		PlayerPrefs.SetInt (GameSystemDefineKeys.SoundFXState, (soundeffectsToggle.isOn)? 1:0);
	}
}
