using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShootingEnemy : Enemy {

	public float TimeBetweenFires;
	public float BeamDamage;

	public List<Weapon> Weapons;
	public List<string> canHit;

	public float SpinSpeed;

	AudioSource shootingAudio;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		float dist = Vector2.Distance(transform.position, Target.position);
		if(dist < 20) {
			transform.localEulerAngles += new Vector3(0, 0, -Time.deltaTime * SpinSpeed);

			foreach (Weapon w in Weapons) {
				float pctToNextFire = (Time.time - w.LastFired) / w.FireCooldown;
				w.meshRenderer.material.SetColor("_EmissionColor", Color.Lerp( Color.black, Color.red, pctToNextFire) );
				if(w.TryFire(canHit)) {
					shootingAudio.volume = 1 - (dist/20f);
					shootingAudio.Play();
				}
			}
		}
	}

	public override void Init(Transform target) {
		base.Init(target);

		float distance = Vector2.Distance(transform.position, Vector2.zero);
		TimeBetweenFires = Mathf.Max( 0.5f, 10f / Mathf.Sqrt(distance));
		BeamDamage = 20f + (distance / 5f);
		SpinSpeed = Random.Range(45, 135);

		shootingAudio = GetComponent<AudioSource>();

		int activeBeams = Random.Range(2,5);

		canHit = new List<string>() { "Player" };

		Weapons = GetComponentsInChildren<Weapon>().ToList();
		Bullet bulletPrefab = Resources.Load<Bullet>("Prefabs/Bullet");
		for(int i = 0; i < Weapons.Count; i++) {
			Weapon w = Weapons[i];
			if ( i >= activeBeams ) {
				Weapons.RemoveAt(i);
				i--;
				Destroy(w.gameObject);
				continue;
			}
		
			w.BulletPrefab = bulletPrefab;
			w.Damage = (int)BeamDamage;
			w.FireCooldown = TimeBetweenFires;
		}
	}
}
