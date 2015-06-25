using UnityEngine;
using System.Collections;

public class BirdInstanceDestroyer : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Bird") {
			GameObject childObject = other.gameObject;
			GameObject parentObject = childObject.transform.parent.gameObject;
			if(parentObject.activeInHierarchy == true){
				parentObject.SetActive(false);
			}
		
		}
		
	}
}
