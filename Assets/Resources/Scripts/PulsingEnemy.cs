using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PulsingEnemy : Enemy {
	
	float Radius;

	PulsingDamager Damager;
	MeshRenderer mr;

	// Use this for initialization
	void Start () {
		
	}

	public override void Init(Transform target) {
		base.Init(target);

		Damager = GetComponentInChildren<PulsingDamager>();

		mr = GetComponent<MeshRenderer>();
		mr.material = new Material(mr.material);

		Color c;
		if (Zone <= 1) {
			Damager.Reflects = false;
			c = new Color(0, 0.5f, 1);
		}
		else {
			Damager.Reflects = true;
			c = new Color(0.5f, 0f, 1f);
		}
		mr.material.SetColor("_Color", c);
		Damager.BaseColor = c;

		float distance = Vector2.Distance( transform.position, Vector2.zero);

		HP = (int)(distance / 50f);

		Radius = Random.Range(5f,10f);
		Damager.Duration = Random.Range(3f,7f);

		Damager.Damage = distance / 10f;
		Damager.DamageCooldown = Mathf.Min( 0.1f, Random.Range(1f,2f) * 5f / Mathf.Sqrt(distance) );

		Damager.Cooldown = 5f;

		Damager.transform.localScale = new Vector3( Radius / transform.localScale.x, 0.5f, Radius / transform.localScale.z );

		Damager.Init(target, Radius / transform.localScale.x);
	}
}
