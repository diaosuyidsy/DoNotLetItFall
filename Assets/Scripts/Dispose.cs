using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dispose : MonoBehaviour
{

	public GameObject ExplosionParticle;

	void OnCollisionEnter2D (Collision2D coll)
	{
		if (coll.gameObject.tag == "Square") {
			GameManager.GM.SquareNum--;
			GameManager.GM.GameOver ();
			Instantiate (ExplosionParticle, coll.gameObject.transform.position, Quaternion.identity);
		}
		if (coll.gameObject.tag != "ReclaimZone")
			Destroy (coll.gameObject);
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.tag != "ReclaimZone")
			Destroy (other.gameObject);
	}
}
