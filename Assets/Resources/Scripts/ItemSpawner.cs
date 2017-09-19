using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour {

	public List<Item> AvailableItems;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void spawnItem(Vector2 position) {
		Item o = Instantiate( AvailableItems[ Random.Range(0, AvailableItems.Count) ] );
		o.transform.position = new Vector3( position.x, position.y, -0.5f);
	}
}
