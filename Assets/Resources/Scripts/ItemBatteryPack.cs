using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBatteryPack : Item {

	public override void Start() {
		base.Start();
	}

	// Update is called once per frame
	public override void Update () {
		base.Update();
	}

	public override void PickupItem(GameObject picker) {
		base.PickupItem(picker);

		Player player = picker.GetComponent<Player>();
		if (player != null) {
			player.MaxPower += 50;
			player.SetPower(player.Power + 50);
			player.SetPowerupText("Battery Life Increased");
		}
	}
}
