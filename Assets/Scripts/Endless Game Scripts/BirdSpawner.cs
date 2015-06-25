using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class BirdFormation
{
	public GameObject[] formation;
}

public class BirdSpawner : MonoBehaviour {

	public BirdFormation[] birdFormations;

	public GameObject RightBorder;
	public float startWait;
	public float nextBirdWait;

	private bool stopSpawning;
	private int gameLevel = 5;

	private GameObject[] instatiatedGameObjects = null;
	private int totalSize = 0;

	void setInstatiatedGameObjectsTotalSize(){
		for (int index = 0; index < birdFormations.Length; index++) {
			totalSize += birdFormations[index].formation.Length;
		}
		instatiatedGameObjects = new GameObject[totalSize];
	}

	void Start () {
		int counter = 0;


		setInstatiatedGameObjectsTotalSize ();

		for (int index = 0; index < birdFormations.Length; index++) {
			foreach(GameObject formation in birdFormations[index].formation){
				instatiatedGameObjects[counter] = Instantiate(formation) as GameObject;
				instatiatedGameObjects[counter].transform.parent = gameObject.transform;
				instatiatedGameObjects[counter].SetActive(false);
				counter++;
			}
		}
		stopSpawning = false;

		StartCoroutine (SpawnBirds ());
	}

	void Update () {
	
	}

	public void StopSpawn(){
		stopSpawning = true;
	}

	public void increaseGameLevel(){
		if(gameLevel < instatiatedGameObjects.Length)
			this.gameLevel++;
	}

	IEnumerator SpawnBirds(){
		yield return new WaitForSeconds(startWait);
		
		while (true) {
				
			if(!GameController.current.IsPaused()){

				int selectedIndex = Random.Range(0, gameLevel);
				GameObject selectedGameObject = instatiatedGameObjects[selectedIndex];
				if(selectedGameObject.activeSelf == false){
					selectedGameObject.transform.position = RightBorder.transform.position;
					selectedGameObject.SetActive(true);	
				}else{
					continue;
				}
			}
			
			yield return new WaitForSeconds(nextBirdWait);

			if(stopSpawning){
				break;
			}
		}
	}
}
