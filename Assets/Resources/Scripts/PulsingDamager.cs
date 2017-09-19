using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PulsingDamager : MonoBehaviour {

	public float Cooldown;

	public float Duration;
	public float StartTime;

	public float Damage;
	public float DamageCooldown;
	public float LastDamageTime;

	public bool Reflects;

	public float TargetDistance;
	public Transform Target;

	public Color BaseColor;

	public bool Active;

	ParticleSystem OuterRing;
	ParticleSystem InnerRing;

	AudioSource charging;
	AudioSource zapping;

	MeshRenderer mr;

	// Use this for initialization
	void Start () {
		
	}

	public void Update() {
		if (Target != null) {
			TargetDistance = Vector2.Distance(transform.position, Target.position);
			SetAudioVolume(  1 - (TargetDistance / 20f) );
		}
	}

	public void SetAudioVolume (float val) {
		zapping.volume = val * 0.8f;
		charging.volume = val * 0.6f;
	}

	public void Init(Transform target, float radius) {
		Target = target;

		AudioSource[] audio = GetComponents<AudioSource>();
		charging = audio.First( a => !a.loop );
		zapping = audio.First(a => a.loop);

		mr = GetComponent<MeshRenderer>();
		mr.material = new Material(mr.material);

		Color c = BaseColor;
		c.a = 0.1f;
		mr.material.SetColor("_Color", c);

		ParticleSystem[] pss = GetComponentsInChildren<ParticleSystem>();
		OuterRing = pss.First(ps => ps.tag == "PulsingRing");
		InnerRing = pss.First(ps => ps.tag != "PulsingRing");

		//model was done with radius = 5
		var rawRadius = radius;
		radius /= 5;

		var main = OuterRing.main;
		main.startLifetimeMultiplier = radius;

		var emission = OuterRing.emission;
		emission.rateOverTime = emission.rateOverTime.constant * Mathf.PI * radius;

		var shape = OuterRing.shape;
		shape.radius *= radius;


		main = InnerRing.main;
		main.startLifetimeMultiplier = radius;
		main.startColor = new ParticleSystem.MinMaxGradient(BaseColor, Color.white);

		emission = InnerRing.emission;
		emission.rateOverTime =  emission.rateOverTime.constant * Mathf.Pow(radius,1.5f);

		var vol = InnerRing.velocityOverLifetime;
		float vx = Mathf.Abs(vol.x.constantMax) * Mathf.Pow(radius, 0.8f);
		vol.x = new ParticleSystem.MinMaxCurve(-vx, vx);
		vol.y = new ParticleSystem.MinMaxCurve(-vx, vx);

		var lvol = InnerRing.limitVelocityOverLifetime;
		lvol.limit = vx * 0.66f;

		InnerRing.gameObject.SetActive(false);

		StartCoroutine(Operate());
	}

	public IEnumerator Operate() {
		var outerMain = OuterRing.main;
		var innerEmission = InnerRing.emission;
		var innerMain = InnerRing.main;
		while (true) {
			float startTime = Time.time;
			charging.Play();
			while (Time.time - startTime < Cooldown) {
				if (TargetDistance < 20) {
					float jTime = (Time.time - startTime) / Cooldown;
					outerMain.startColor = new ParticleSystem.MinMaxGradient(
						new Color(BaseColor.r, BaseColor.g, BaseColor.b) * Mathf.Lerp(0,1,jTime),
						new Color(1,1,1) * Mathf.Lerp(0, 1, jTime)
					);
					yield return new WaitForEndOfFrame();
				}
				else {
					yield return new WaitUntil(() => TargetDistance < 20);
				}
			}

			Active = true;
			InnerRing.gameObject.SetActive(true);
			zapping.Play();
			yield return new WaitForSeconds(Duration-innerMain.startLifetimeMultiplier);
			zapping.Stop();

			float rot = innerEmission.rateOverTimeMultiplier;
			innerEmission.rateOverTimeMultiplier = 0f;

			outerMain.startColor = new ParticleSystem.MinMaxGradient(
					new Color(BaseColor.r, BaseColor.g, BaseColor.b, 0),
					new Color(1, 1, 1, 0)
				);
			Active = false;

			yield return new WaitForSeconds(innerMain.startLifetimeMultiplier);
			InnerRing.gameObject.SetActive(false);
			innerEmission.rateOverTimeMultiplier = rot;			
		}
	}

	public void OnTriggerEnter(Collider c) {
		if(c.tag == "Bullet") {
			if(Active) {
				if(Reflects) {
					Bullet b = c.GetComponent<Bullet>();

					b.ExpirationTime = Time.time + b.Lifetime;

					var main = b.ps.main;

					b.CanHitTheseTags.Add("Player");
					b.Damage *= 10;

					b.Direction = ((Vector2)c.transform.position - (Vector2)this.transform.position).normalized;
					main.startRotation = Mathf.Deg2Rad * (180 - Mathf.Atan(b.Direction.y / b.Direction.x) * Mathf.Rad2Deg);
				}
				else {
					Destroy(c.gameObject);
				}
			}
		}
	}

	public void OnTriggerStay(Collider c) {
		if (c.tag == "Player") {
			if( Active && Time.time - LastDamageTime > DamageCooldown ) {
				Player p = c.GetComponent<Player>();
				p.SetPower( p.Power - Damage );
				LastDamageTime = Time.time;
			}
		}
	}
}
