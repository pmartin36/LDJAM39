using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ChargingPad : MonoBehaviour {

	public AudioSource click;
	public AudioSource charging;

	Coroutine startCharging;

	// Use this for initialization
	void Start () {
		AudioSource[] sources = GetComponents<AudioSource>();
		charging = sources.First( a => a.loop );
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void StartCharging() {
		charging.Play();
	}

	public void StopCharging() {
		charging.Stop();
	}
}
