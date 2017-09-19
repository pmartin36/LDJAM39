using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class UIBattery : MonoBehaviour {

	public Player player;

	private Image Battery;
	private TMP_Text PctText;

	public bool Blinking = false;
	
	void Start() {
		Image[] imgs = GetComponentsInChildren<Image>();
		Battery = imgs.First( i => i.tag == "BatteryBar" );


		PctText = GetComponentInChildren<TMP_Text>();
	}

	// Update is called once per frame
	void Update () {
		float pct = player.PowerPct;
		Color c = Color.Lerp( Color.green, Color.red, 1-pct);

		Battery.material.SetFloat("_Pct", pct);

		PctText.text = (int)(pct*100) + "%";

		if(pct < 0.15f && (int)Time.time % 2 == 0) {
			//blink
			Battery.color = Color.clear;
		}
		else {
			Battery.color = c;			
		}
		PctText.color = c;
	}
}
