using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingEnemy : Enemy {

	public float AggroRange;
	public float Damage;

	public float LeashRange;

	public bool Aggroed = false;

	public bool CanMoveNormally = true;
	public float Speed;
	public Vector3? waypoint;

	public Vector3? temporaryWaypoint;
	public float temporarySpeed;
	public Vector3 aggroStartWaypoint;

	public Vector3 startWaypoint;
	public float waypointDistance;

	public Vector3 Direction;

	MeshRenderer mr;

	AudioSource buzz;

	// Use this for initialization
	void Start () {
		
	}

	public override void Init(Transform target) {
		base.Init(target);

		transform.position = new Vector3( transform.position.x, transform.position.y, -0.8f );

		StartCoroutine(GenerateNewWaypoint(0f));

		mr = GetComponent<MeshRenderer>();
		mr.material = new Material(mr.material);
		//mr.material.SetColor("_AggroMultiplier", new Color(1, 0.5f, 0.5f, 1));

		buzz = GetComponent<AudioSource>();

		if ( Zone == 0 ) {
			AggroRange = 0f;		
			HP = 1;
			Damage = 10f;
			Speed = 3f;
		}
		else {
			float dist = Vector2.Distance(transform.position, Vector2.zero);
			AggroRange = Random.value > 0.9f ? 0 : 5;			
			HP = (int)(dist/50f);
			Damage = 10f + (dist/10f);
			Speed = Random.Range(2.5f, 3.5f);
		}
		
		Color color;
		if(AggroRange > 0) {
			color = new Color(1f, 0, 0);
			LeashRange = 10f;
		}
		else {
			color = new Color(1f, 0.5f, 0);
		}

		mr.material.SetColor("_Color", color);
		var main = GetComponentInChildren<ParticleSystem>().main;
		main.startColor = color;
	}

	// Update is called once per frame
	void Update () {
		float playerDist = Vector2.Distance(Target.position, this.transform.position);
		if (temporaryWaypoint != null) {
			//go to temporary waypoint
			transform.position += -Direction * temporarySpeed * Time.deltaTime;

			float dist = Vector3.Distance(transform.position, startWaypoint);
			temporarySpeed = Mathf.Lerp( 10, 0.5f, dist / 3f );
			if(dist > 3) {
				//waypoint distance is 3
				temporaryWaypoint = null;
			}
		}
		else if( Aggroed ) {
			if(CanMoveNormally) {
				//move towards player
				float dist = Vector2.Distance(aggroStartWaypoint, this.transform.position);
				if(dist < LeashRange) {
					Direction = (Vector2)((Target.position - transform.position)).normalized;
					transform.position += Direction * Speed * Time.deltaTime;
				}
				else {
					SetAggroed(false);
				}
			}
		}
		else if( waypoint != null) {
			if(CanMoveNormally) {
				//if player within aggro range
				if(playerDist < AggroRange) {
					SetAggroed(true);
				}
				else if(playerDist < 20) {
					//move towards waypoint
					transform.position += Direction * Speed * Time.deltaTime;
					if ((transform.position - startWaypoint).sqrMagnitude >= waypointDistance) {
						waypoint = null;
						StartCoroutine(GenerateNewWaypoint(1f));
					}

					if(AggroRange > 0) {
						//mr.material.SetFloat("_AggroIndicator", AggroRange / dist);
					}
				}
			}
		}

		buzz.volume = 1 - (playerDist / 20f);
		transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles + new Vector3(0, 0, 135 * Time.deltaTime));
	}

	private void SetAggroed(bool val) {
		Aggroed = val;
		if(Aggroed) {
			//mr.material.SetFloat("_AggroIndicator", 1.1f);
			aggroStartWaypoint = transform.position;
		}
		else {
			//mr.material.SetFloat("_AggroIndicator", 0f);
		}
	}

	private IEnumerator GenerateNewWaypoint(float waitTime) {
		yield return new WaitForSeconds(waitTime);

		Vector3 offset = new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), 0);
		waypoint = transform.position + offset;
		startWaypoint = transform.position;
		waypointDistance = offset.sqrMagnitude;	
		Direction = offset.normalized;					
	}

	private IEnumerator DisableMovementForTime(float waitTime) {
		CanMoveNormally = false;
		yield return new WaitForSeconds(waitTime);
		CanMoveNormally = true;
	}

	public void OnCollisionEnter(Collision c) {
		if(c.transform.tag == "Player") {
			Player p = c.gameObject.GetComponent<Player>();
			//deal damage
			p.SetPower( p.Power - Damage, true );

			SetAggroed(false);
			StartCoroutine(DisableMovementForTime(1f));
		}

		if(!Aggroed) {
			//backward towards waypoint
			Direction = Vector3.Scale( 
							c.contacts[0].point - transform.position, 
							new Vector3( Random.Range(0f, 5f), Random.Range(0f, 5f), 0 )
						).normalized;
			startWaypoint = transform.position;
			temporaryWaypoint = -Direction * 3;
			temporarySpeed = 10f;
		}
	}
}
