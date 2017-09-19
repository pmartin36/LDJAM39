using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponList : List<Weapon> {

	public ItemWeaponType.WeaponType weaponType;
	public int Level;

	public WeaponList( IEnumerable<Weapon> weapons ) {
		this.AddRange(weapons);
		weaponType = ItemWeaponType.WeaponType.SINGLE;
		Level = 1;
	}

	public void LevelUpType() {
		Level = Mathf.Min(5, Level+1);
		foreach(Weapon w in this) {
			w.LevelWeapon(Level);
		}
	}

	public void SetWeaponType(ItemWeaponType.WeaponType wt) {
		weaponType = wt;
		Level = 1;
	}

	public void DeleteAll() {
		for(int i = 0; i < this.Count; i++) {
			this[i].DestroyWeapon();
		}
		this.Clear();
	}
}
