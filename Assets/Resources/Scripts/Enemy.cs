using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public int HP;
	public Transform Target;
	public int Zone;
	public float DropChance;

	public static bool FirstEnemy = true;

	public virtual void Init( Transform target ) {
		Target = target;

		int comparison = (int)Mathf.Max( Mathf.Abs(transform.position.x), Mathf.Abs(transform.position.y));
		if( comparison < 25) {
			Zone = 0;
		}
		else if (comparison < 75) {
			Zone = 1;
		}
		else if (comparison < 150) {
			Zone = 2;
		}
		else if (comparison < 300) {
			Zone = 3;
		}
		else {
			Zone = 4;
		}

		DropChance = 0.125f * (1f + (float)Zone/1.5f);
	}

	public virtual void OnTriggerEnter(Collider c) {
		if (c.tag == "Bullet") {
			Bullet b = c.GetComponent<Bullet>();

			if (b.CanHitTheseTags.Contains(this.gameObject.tag)) {
				Destroy(c.gameObject);

				HP -= b.Damage;
				if (HP <= 0) {
					Destroy(this.gameObject);

					if(FirstEnemy) {
						//100% drop chance
						GameManager.Instance.SpawnItem(this.transform.position);
						FirstEnemy = false;
					}
					else {
						if( Random.value < DropChance ) {
							GameManager.Instance.SpawnItem(this.transform.position);
						}
					}
				}
			}
		}
	}
}
