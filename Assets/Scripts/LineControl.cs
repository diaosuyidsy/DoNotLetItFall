using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineControl : MonoBehaviour
{

	public GameObject leftLine;
	public GameObject rightLine;
	public GameObject dottedLine;
	public float maxLineDistance = 38f;
	public bool CanClose = false;
	[Range (1, 20)]
	public float CloseSpeed = 1f;

	private Color thisColor;
	private bool initLock = true;
	private float LineDistance;
	private float CamHeight;
	private float CamWidth;
	private int NUMb;
	private Text Score;
	private float l_distanceTraveled = 0f;
	private float r_distanceTraveled = 0f;
	private float l_maxDist;
	private float r_maxDist;
	private bool l_isClosing = true;
	private bool r_isClosing = true;
	private float t = 0f;
	//	private Text BestScore;

	void Start ()
	{
		thisColor = dottedLine.GetComponent<SpriteRenderer> ().color;

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
		l_maxDist = ((-1f * CamWidth) / 2f) - leftLine.transform.position.x;
		r_maxDist = rightLine.transform.position.x - CamWidth / 2f;
		l_maxDist *= 0.6f;
		r_maxDist *= 0.6f;
		initLock = false;
	}

	void Update ()
	{
		if (dottedLine.GetComponent<SpriteRenderer> ().color != thisColor) {
			dottedLine.GetComponent<SpriteRenderer> ().color = Color.Lerp (Color.white, thisColor, t);

			if (t < 1) {
				t += (Time.deltaTime / 1.0f);
			}
		}


		if (CanClose && !initLock) {
			if (l_distanceTraveled <= l_maxDist && l_isClosing && leftLine != null) {
				Vector3 l_oldPos = leftLine.transform.position;
				leftLine.transform.Translate (CloseSpeed * Time.deltaTime, 0, 0);
				l_distanceTraveled += Vector3.Distance (l_oldPos, leftLine.transform.position);
			} else if (l_distanceTraveled >= l_maxDist) {
				l_isClosing = false;
			}

			if (l_distanceTraveled >= 0 && !l_isClosing && leftLine != null) {
				Vector3 l_oldPos = leftLine.transform.position;
				leftLine.transform.Translate (-CloseSpeed / 4f * Time.deltaTime, 0, 0);
				l_distanceTraveled -= Vector3.Distance (l_oldPos, leftLine.transform.position);
			} else if (l_distanceTraveled <= 0) {
				l_isClosing = true;
			}
			//Deal with right
			if (r_distanceTraveled < r_maxDist && r_isClosing && rightLine != null) {
				Vector3 r_oldPos = rightLine.transform.position;
				rightLine.transform.Translate (-CloseSpeed * Time.deltaTime, 0, 0);
				r_distanceTraveled += Vector3.Distance (r_oldPos, rightLine.transform.position);
			} else if (r_distanceTraveled >= r_maxDist) {
				r_isClosing = false;
			}

			if (r_distanceTraveled >= 0 && !r_isClosing && rightLine != null) {
				Vector3 r_oldPos = rightLine.transform.position;
				rightLine.transform.Translate (CloseSpeed / 4f * Time.deltaTime, 0, 0);
				r_distanceTraveled -= Vector3.Distance (r_oldPos, rightLine.transform.position);
			} else if (r_distanceTraveled <= 0) {
				r_isClosing = true;
			}
		}
	}


	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.tag == "Square") {
			if (other.attachedRigidbody.velocity.y >= 0f && !other.gameObject.GetComponent<SquareControl> ().Numbers.Contains (NUMb)) {
				// Score
				int score;
				int.TryParse (Score.text, out score);
				Score.text = (score + 1).ToString ();
//				// Add time
//				GameManager.GM.Tiime += 1f;
				other.gameObject.GetComponent<SquareControl> ().Numbers.Add (NUMb);

				dottedLine.GetComponent<SpriteRenderer> ().color = Color.white;
				t = 0f;

			}
		}
	}

}
