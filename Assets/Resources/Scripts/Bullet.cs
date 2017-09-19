using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	public Vector3 Direction;
	public float Speed;

	public float Lifetime;
	public float ExpirationTime;

	public int Damage;

	public ParticleSystem ps;

	public List<string> CanHitTheseTags;

	public void Init ( float dir, float speed, int damage, Color color, float lifetime, List<string> hittags ) {
		Direction = AngleToVector(dir);
		Speed = speed;
		Damage = damage;
		Lifetime = lifetime;

		this.transform.position = new Vector3(transform.position.x + 0.5f * Direction.x , transform.position.y + 0.5f * Direction.y, -0.5f);

		ps = GetComponentInChildren<ParticleSystem>();
		var main = ps.main;
		main.startRotation = Mathf.Deg2Rad * (180 -  dir);
		main.startColor = color;

		var l = ps.lights;
		l.useParticleColor = true;

		ExpirationTime = Time.time + Lifetime;

		CanHitTheseTags = hittags;
	}

	private Vector3 AngleToVector(float angle) {
		return new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle));
	}

	// Update is called once per frame
	void FixedUpdate () {
		if(Time.time > ExpirationTime) {
			DestroyImmediate(this.gameObject);
		}
		else {
			transform.position += Direction * Speed * Time.fixedDeltaTime;
		}
	}	
}
