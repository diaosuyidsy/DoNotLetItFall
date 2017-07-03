using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineControl : MonoBehaviour
{

	public GameObject leftLine;
	public GameObject rightLine;
	public float maxLineDistance = 38f;

	private float LineDistance;
	private float CamHeight;
	private float CamWidth;
	private int NUMb;
	private Text Score;
	//	private Text BestScore;

	void Start ()
	{
		Score = GameObject.FindGameObjectWithTag ("Score").GetComponent<Text> ();
//		BestScore = GameObject.FindGameObjectWithTag ("BestScore").GetComponent<Text> ();
	}

	public void Init (int NUM)
	{
		NUMb = NUM;
		LineDistance = maxLineDistance;
		CamHeight = 2f * Camera.main.orthographicSize;
		CamWidth = CamHeight * Camera.main.aspect;
		if (NUM >= maxLineDistance) {
			NUM = Mathf.RoundToInt (maxLineDistance);
		}
		LineDistance -= (1f * NUM / 2f);
		float lrpos = LineDistance / 2f;
		leftLine.transform.position = new Vector3 (-lrpos, leftLine.transform.position.y);
		rightLine.transform.position = new Vector3 (lrpos, rightLine.transform.position.y);

		//Set transform randomly
		float ranSpace = (CamWidth - (LineDistance - leftLine.GetComponent<BoxCollider2D> ().size.x)) / 2f;
		transform.position = new Vector3 (Random.Range (-ranSpace, ranSpace), transform.position.y);
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
