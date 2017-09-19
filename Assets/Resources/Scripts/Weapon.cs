using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

	public float LastFired;
	public float BulletDuration;

	public float FireCooldown;
	public int Damage;

	public float PowerUsage;

	public Bullet BulletPrefab;
	public Color PowerColor;

	public MeshRenderer meshRenderer;

	public void Awake() {
		meshRenderer = GetComponent<MeshRenderer>();
		meshRenderer.material = new Material(meshRenderer.material);
		PowerColor = Color.red;
		Damage = 1;
		BulletDuration = 1f;
		PowerUsage = 1f;
	}

	public bool TryFire(List<string> canhit) {
		Vector2 direction = (transform.position - transform.parent.position).normalized;
		float d = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		return TryFire(d, canhit);
	}

	public void LevelWeapon(int level) {
		Damage = level;
		switch(level) {
			default:
			case 1:
				PowerColor = Color.red;
				break;
			case 2:
				PowerColor = Color.green;
				break;
			case 3:
				PowerColor = Color.cyan;
				break;
			case 4:
				PowerColor = Color.yellow;
				break;
			case 5:
				PowerColor = new Color(0.3f, 0.188f, 0.52f);
				break;
		}
		meshRenderer.material.SetColor("_Color", PowerColor);
	}

	public void DestroyWeapon() {
		Destroy(this.gameObject);
	}

	public bool TryFire(float direction, List<string> canhit) {
		if ((Time.time - LastFired) > FireCooldown) {
			Fire(direction, canhit);
			LastFired = Time.time;
			return true;
		}
		return false;
	}

	public void Fire(float direction, List<string> canhit) {
		Bullet b = Instantiate<Bullet>(BulletPrefab, this.transform.position, Quaternion.identity);
		b.Init(direction, 10f, Damage, PowerColor, BulletDuration, canhit);
	}
}
