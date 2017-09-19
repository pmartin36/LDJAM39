using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel;

public class ItemWeaponType : Item {

	public enum WeaponType {
		[Description("Single Shot")]
		SINGLE,
		[Description("Fork Shot")]
		FORK,
		[Description("Rear Shot")]
		FRONTBACK,
		[Description("Ring Shot")]
		RING
	}

	public static List<Sprite> WeaponSprites;

	public WeaponType weaponType;

	public override void Awake() {
		base.Awake();
		weaponType = GenerateRandomWeaponType();

		if(WeaponSprites == null) {
			WeaponSprites = new List<Sprite>();
			WeaponSprites.AddRange( Resources.LoadAll<Sprite>("Sprites/WeaponSprites") );
		}
		GetComponent<SpriteRenderer>().sprite = WeaponSprites[(int)weaponType];
	}

	public override void Start() {
		base.Start();
	}

	public override void Update() {
		base.Update();
	}

	public static WeaponType GenerateRandomWeaponType() {
		return (WeaponType) Random.Range(0,4);
	}

	public static string GetWeaponTypeString(WeaponType w) {
		switch (w) {
			default:
			case WeaponType.SINGLE:
				return "Single Shot";
			case WeaponType.FORK:
				return "Fork Shot";
			case WeaponType.FRONTBACK:
				return "Rear Shot";
			case WeaponType.RING:
				return "Ring Shot";
		}
	}

	public override void PickupItem(GameObject picker) {
		base.PickupItem(picker);

		Player player = picker.GetComponent<Player>();
		if(player != null) {
			player.ChangeWeaponType( weaponType );
		}
	}
}
