using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BackgroundChanger : MonoBehaviour {

	public Sprite[] backgroundSprite;
	public SpriteRenderer firstSceneBackground;
	public SpriteRenderer secondSceneBackground;

	private const string COUNT = "COUNT";

	void Start () {
		int backgroundCount = 0;
		do {
			backgroundCount = Random.Range (0, backgroundSprite.Length);
		} while(PlayerPrefs.GetInt(COUNT) == backgroundCount);

		Sprite spritePicked = backgroundSprite [backgroundCount];
		firstSceneBackground.sprite = spritePicked;
		secondSceneBackground.sprite = spritePicked;
		PlayerPrefs.SetInt (COUNT, backgroundCount);
	}
	

	void Update () {
	
	}
}
