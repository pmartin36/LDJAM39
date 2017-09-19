using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

	public float SpawnRate;
	public List<Enemy> EnemyPrefabs;
	private Transform EnemyContainer;

	public Transform EnemyTarget;

	// Use this for initialization
	void Start () {
		EnemyContainer = new GameObject("Enemies").transform;
	}

	void SpawnEnemy( Vector3 position ) {
		int spawnRandom = Random.Range(0, EnemyPrefabs.Count);
		Enemy p = EnemyPrefabs[spawnRandom];
		Enemy e = Instantiate<Enemy>(p , new Vector3( position.x, position.y, -0.35f), p.transform.rotation, EnemyContainer);
		e.Init(EnemyTarget);
	}

	public void c_newTilesSpawnedEvent(object sender, TileSpawnedEventArgs e) {
		foreach( Vector3 v in e.Positions.Where( p => Mathf.Abs(p.x) > 5 && Mathf.Abs(p.y) > 5) ) {
			if( Random.value < SpawnRate ) {
				SpawnEnemy(v);
			}
		}
	}
}
