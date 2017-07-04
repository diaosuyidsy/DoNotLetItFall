using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpinLineControl : MonoBehaviour
{
	public GameObject dottedLine;
	public GameObject SpinBlock;
	public float SpinSpeed = 250f;
	public bool CanSpin = true;

	private Text Score;
	private int NUMb;
	private Color thisColor;
	private float t = 0f;


	// Use this for initialization
	void Start ()
	{
		thisColor = dottedLine.GetComponent<SpriteRenderer> ().color;

		Score = GameObject.FindGameObjectWithTag ("Score").GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (dottedLine.GetComponent<SpriteRenderer> ().color != thisColor) {
			dottedLine.GetComponent<SpriteRenderer> ().color = Color.Lerp (Color.white, thisColor, t);

			if (t < 1) {
				t += (Time.deltaTime / 1.0f);
			}
		}

		if (CanSpin && SpinBlock != null)
			SpinBlock.transform.Rotate (Vector3.forward * Time.deltaTime * SpinSpeed);

	}

	public void Init (int NUM)
	{
		NUMb = NUM;
		float CamHeight = 2f * Camera.main.orthographicSize;
		float CamWidth = CamHeight * Camera.main.aspect;
		Vector3 ls = SpinBlock.transform.localScale;
		if (ls.x <= 0.85)
			SpinBlock.transform.localScale = new Vector3 (ls.x + 0.01f * NUM, ls.y, ls.z);
		if (SpinSpeed <= 300f)
			SpinSpeed += NUM * 2;
		float selfWidth = SpinBlock.GetComponent<BoxCollider2D> ().size.x * SpinBlock.transform.localScale.x;
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

				dottedLine.GetComponent<SpriteRenderer> ().color = Color.white;
				t = 0f;
			}
		}
	}
}
