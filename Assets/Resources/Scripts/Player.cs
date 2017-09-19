using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour {

	private BoxCollider box;
	private Rigidbody rigid;

	private float horizontal;
	private float vertical;

	public float moveSpeed = 0.05f;

	public bool Turning = false;
	public float Direction = 0f;

	public CameraController FollowingCamera { get; set; }

	public float MaxPower { get; set; }
	public float Power { get; set; }
	public float PowerPct {
		get {
			return Power / MaxPower;
		}
	}

	public Light SpotLight;

	public bool Charging { get; set; }

	private WeaponList weapons;

	public PowerupTextSpawner powerupText;

	Material material;

	AudioSource shotAudio;
	AudioSource hitAudio;

	// Use this for initialization
	public void Start () {
		AudioSource[] sources = GetComponents<AudioSource>();
		shotAudio = sources.First( a => !a.mute );
		hitAudio = sources.First( a => a.mute );
		hitAudio.mute = false;

		box = GetComponent<BoxCollider>();
		rigid = GetComponent<Rigidbody>();

		weapons = new WeaponList(GetComponentsInChildren<Weapon>());

		Charging = true;
		MaxPower = 150;
		Power = 150;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(!Turning) {
			rigid.velocity = Vector3.zero;
			transform.position += new Vector3(horizontal * moveSpeed, vertical * moveSpeed, 0);
		}

		if(Charging) {
			SetPower( Power + Time.fixedDeltaTime * (MaxPower / 5f) );
		}
		else {
			SetPower(Power - Time.fixedDeltaTime * 1.5f);
		}
	}

	public void SetPowerupText(string text) {
		powerupText.SetText(text);
	}

	public void ChangeWeaponType(ItemWeaponType.WeaponType weaponType) {
		string weaponString = ItemWeaponType.GetWeaponTypeString(weaponType);

		if (weaponType == weapons.weaponType) {
			SetPowerupText(weaponString + " Improved");
			weapons.LevelUpType();
			return;
		}

		weapons.DeleteAll();
		Weapon w = Resources.Load<Weapon>("Prefabs/Weapon");
		Weapon cw;

		float weaponDistance = w.transform.localPosition.x;
		weapons.SetWeaponType(weaponType);
		SetPowerupText(weaponString + " Equipped");

		switch (weaponType) {
			default:
			case ItemWeaponType.WeaponType.SINGLE:
				cw = Instantiate(w, this.transform);
				cw.FireCooldown = 0.4f;
				cw.PowerUsage = 5f;
				weapons.Add(cw);
				break;
			case ItemWeaponType.WeaponType.FORK:
				Vector2 vf = Vector2.one * .707f * weaponDistance; //normalized vector * weaponDistance
				cw = Instantiate(w, this.transform);
				cw.transform.localPosition = new Vector3(vf.y, cw.transform.localPosition.y, vf.x);
				cw.FireCooldown = 0.5f;
				cw.PowerUsage = 4f;
				weapons.Add(cw);

				vf.Scale( new Vector2(-1,1));
				cw = Instantiate(w, this.transform);
				cw.transform.localPosition = new Vector3(vf.y, cw.transform.localPosition.y, vf.x);
				cw.FireCooldown = 0.5f;
				cw.PowerUsage = 4f;
				weapons.Add(cw);
				break;
			case ItemWeaponType.WeaponType.FRONTBACK:
				cw = Instantiate(w, this.transform);
				cw.FireCooldown = 0.6f;
				cw.PowerUsage = 4f;
				weapons.Add(cw);

				cw = Instantiate(w, this.transform);
				cw.transform.localPosition = new Vector3( -weaponDistance, w.transform.localPosition.y, w.transform.localPosition.z );
				cw.FireCooldown = 0.6f;
				cw.PowerUsage = 4f;
				weapons.Add(cw);
				break;
			case ItemWeaponType.WeaponType.RING:
				for (int i = 0; i < 360; i+=60) {
					cw = Instantiate(w, this.transform);
					Vector2 v = AngleToVector(i).normalized * weaponDistance;
					cw.transform.localPosition = new Vector3( v.x, cw.transform.localPosition.y, v.y );
					cw.BulletDuration = 0.35f;
					cw.PowerUsage = 2f;
					cw.FireCooldown = 1f;
					weapons.Add(cw);
				}
				break;
		}
	}

	public void Fire ( bool fireDown ) {
		if(fireDown) {
			foreach(Weapon weapon in weapons) {
				if( weapon.TryFire(new List<string>(){"Enemy" } ) ) {
					SetPower ( Power - weapon.PowerUsage );

					//play audio
					shotAudio.Play();
				}
			}
		}
	}

	public void SetPower ( float newpower, bool fromHit = false ) {
		if(newpower <= 0) {
			GameManager.Instance.SetGameOver();
		}

		float maxLight = 1f;
		Power = Mathf.Max(0, Mathf.Min( MaxPower, newpower ));

		SpotLight.spotAngle = Mathf.Lerp( 10, 150, PowerPct );
		SpotLight.intensity = Mathf.Lerp( 0.25f, maxLight, PowerPct );

		if(fromHit) {
			hitAudio.Play();
		}
	}

	public void OnTriggerEnter( Collider c ) {
		if(c.tag == "Charger") {
			Charging = true;
			c.GetComponent<ChargingPad>().StartCharging();
		}
		if(c.tag == "Bullet") {
			Bullet b = c.GetComponent<Bullet>();
			if( b.CanHitTheseTags.Contains ( this.gameObject.tag )) {
				Destroy(c.gameObject);
				SetPower( Power - b.Damage, true );
			}
		}
	}

	public void OnTriggerExit ( Collider c) {
		if (c.tag == "Charger") {
			Charging = false;
			c.GetComponent<ChargingPad>().StopCharging();
		}
	}

	#region movement
	public float xyToAngle(float x, float y) {
		return Mathf.Atan2(y, x) * Mathf.Rad2Deg;
	}

	public float VectorToAngle(Vector2 vector) {
		return xyToAngle(vector.x, vector.y);
	}

	public void SetMoveDirections(float h, float v) {
		horizontal = h;
		vertical = v;

		if( Mathf.Abs(h) < 0.01f && Mathf.Abs(v) < 0.01f)
			return;
		float angle = xyToAngle(h,v);
		Direction = angle;
		transform.localRotation = Quaternion.Euler( transform.localEulerAngles.x, angle, transform.localEulerAngles.z);
	}

	private Vector3 AngleToVector(float angle) {
		return new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle));
	}
	#endregion movement
}
