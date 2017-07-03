using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpinLineControl : MonoBehaviour
{
	
	public GameObject SpinBlock;
	public float SpinSpeed = 50f;
	public bool CanSpin = true;
	private Text Score;
	private int NUMb;


	// Use this for initialization
	void Start ()
	{
		Score = GameObject.FindGameObjectWithTag ("Score").GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (CanSpin && SpinBlock != null)
			SpinBlock.transform.Rotate (Vector3.forward * Time.deltaTime * SpinSpeed);
	}

	public void Init (int NUM)
	{
		NUMb = NUM;
		float CamHeight = 2f * Camera.main.orthographicSize;
		float CamWidth = CamHeight * Camera.main.aspect;
		float selfWidth = SpinBlock.GetComponent<BoxCollider2D> ().size.x;
		float randomSpace = (CamWidth / 2) - (selfWidth / 2);
		transform.position = new Vector3 (Random.Range (-randomSpace, randomSpace), transform.position.y);
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.tag == "Square") {
			if (other.attachedRigidbody.velocity.y >= 0f && !other.gameObject.GetComponent<SquareControl> ().Numbers.Contains (NUMb)) {
				// Score
				int score;
				int.TryParse (Score.text, out score);
				Score.text = (score + 1).ToString ();
				// Add time
				GameManager.GM.Tiime += 1f;
				other.gameObject.GetComponent<SquareControl> ().Numbers.Add (NUMb);
			}
		}
	}
}
