using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpeedBoost : Item {

	public override void Start() {
		base.Start();
	}

	// Update is called once per frame
	public override void Update() {
		base.Update();
	}

	public override void PickupItem(GameObject picker) {
		base.PickupItem(picker);

		Player player = picker.GetComponent<Player>();
		if (player != null) {
			player.moveSpeed += 0.02f;
			player.SetPowerupText("Speed Increased");
		}
	}
}
