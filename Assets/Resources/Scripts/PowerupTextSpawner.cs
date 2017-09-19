using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupTextSpawner : MonoBehaviour {

	public PowerupText PowerupTextPrefab;

	// Update is called once per frame
	public void SetText (string text) {
		PowerupText put = Instantiate(PowerupTextPrefab, this.transform);
		put.Init(text);
	}
}
