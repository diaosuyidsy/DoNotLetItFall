using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dispose : MonoBehaviour
{

	void OnCollisionEnter2D (Collision2D coll)
	{
		if (coll.gameObject.tag == "Square") {
			GameManager.GM.SquareNum--;
			GameManager.GM.GameOver ();
		}
		Destroy (coll.gameObject);
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		Destroy (other.gameObject);
	}
}
