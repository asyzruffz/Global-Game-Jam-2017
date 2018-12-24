using UnityEngine;

public class MonsterSpawner : MonoBehaviour {

	public float ghostSpawnTime;
	public GameObject ghost;
	public Transform[] spawnPoints;

	float ghostSpawnTimer = 0;

	void Start () {
		SpawnGhost ();
	}

	void Update () {
		ghostSpawnTimer += Time.deltaTime;

		if (ghostSpawnTimer >= ghostSpawnTime) {
			SpawnGhost ();
		}
			
	}

	void SpawnGhost() {
		int chosenSpawner = Random.Range (0, spawnPoints.Length);

		Instantiate (ghost, spawnPoints[chosenSpawner].position, Quaternion.identity);
		ghostSpawnTimer = 0;
	}
}
