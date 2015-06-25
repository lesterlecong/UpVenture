using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BackgroundChanger : MonoBehaviour {

	public Sprite[] backgroundSprite;
	public SpriteRenderer firstSceneBackground;
	public SpriteRenderer secondSceneBackground;


	void Start () {
		Sprite spritePicked = backgroundSprite [Random.Range (0, backgroundSprite.Length)];
		firstSceneBackground.sprite = spritePicked;
		secondSceneBackground.sprite = spritePicked;
	}
	

	void Update () {
	
	}
}
