using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public Transform target;
	public float xOffset;

	void Update () {
		transform.position = new Vector3 (target.position.x + xOffset, transform.position.y, transform.position.z);
	}
}
