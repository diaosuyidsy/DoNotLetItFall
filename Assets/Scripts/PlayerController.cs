using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private Rigidbody2D rb;
	public float ExplosionRadius = 5.0f;
	public float ExplosionForce = 10.0f;
	public float Upforce = 1.0f;
	public bool GameStart = false;

	public Transform FirstExplosionPosition;

	public void StartGame ()
	{
		Detonate (FirstExplosionPosition.position, 30f, 0, ExplosionForce);
		GameStart = true;
	}

	void Update ()
	{
		if (GameStart) {
//			#if UNITY_EDITOR_OSX
//			if (Input.GetMouseButtonDown (0)) {
//				Detonate (Camera.main.ScreenToWorldPoint (Input.mousePosition), ExplosionRadius, 0f, ExplosionForce);
//			}
//			#endif

			#if UNITY_IOS
			foreach (Touch touch in Input.touches) {
				if (touch.phase == TouchPhase.Began) {
					Detonate (Camera.main.ScreenToWorldPoint (Input.mousePosition), ExplosionRadius, 0f, ExplosionForce);
				}
			}
			#endif
		}


	}

	void Detonate (Vector3 explosionPosition, float ER, float UF, float EF)
	{
		Collider2D[] colliders = Physics2D.OverlapCircleAll (explosionPosition, ER);
		foreach (Collider2D hit in colliders) {
			Rigidbody2D rb = hit.GetComponent<Rigidbody2D> ();
			if (rb != null && rb.gameObject.name != "Ground") {
				rb.AddExplosionForce (EF, explosionPosition, ER, UF, ForceMode2D.Force);
			}

		}
	}
}
