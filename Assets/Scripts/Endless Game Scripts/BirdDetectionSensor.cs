using UnityEngine;
using System.Collections;

public class BirdDetectionSensor : MonoBehaviour {

	private static string previousObjectName = "";

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Bird") {
			GameObject childObject = other.gameObject;
			GameObject parentObject = childObject.transform.parent.gameObject;
			if(previousObjectName != parentObject.name){
				BirdList.instance.addBirdTarget(parentObject);
				previousObjectName = parentObject.name;
			}
		}
		
	}
}
