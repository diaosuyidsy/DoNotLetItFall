using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public static GameManager GM;
	public bool GameStart = false;
	public GameObject AllGameObjects;
	public GameObject LinePrefab;
	public GameObject SpinLinePrefab;
	public float Tiime;
	public int SquareNum;


	private float LineGenRate;
	private float MovingRate;
	private int NUM;
	//	private Text Timer;
	private Text BestScore;
	private Text Score;
	private int MaxSquareNum;
	private int bestscore;

	void Awake ()
	{
		GM = this;
	}

	void Start ()
	{
		BestScore = GameObject.FindGameObjectWithTag ("BestScore").GetComponent<Text> ();
//		Timer = GameObject.FindGameObjectWithTag ("Time").GetComponent<Text> ();
		Score = GameObject.FindGameObjectWithTag ("Score").GetComponent<Text> ();

		bestscore = PlayerPrefs.GetInt ("BestScore", 0);
		BestScore.text = bestscore.ToString ();
		MaxSquareNum = GameObject.FindGameObjectsWithTag ("Square").Length;
		SquareNum = MaxSquareNum;
		Tiime = 10f;
//		Timer.text = Tiime.ToString ("F1");
		NUM = 1;
		GameStart = false;
		LineGenRate = 6f;
		MovingRate = 2f;
		Input.multiTouchEnabled = true;
	}

	public void StartGame ()
	{
		GameStart = true;
		GenerateNewObstacles ();
		StartCoroutine (ProgressHarderLevel ());
	}

	void GenerateNewObstacles ()
	{
		if (NUM <= 3) {
			StartCoroutine (GenerateNewLine (LineGenRate, false));
		} else if (NUM <= 6) {
			int r = Random.Range (1, 3);
			switch (r) {
			case 1:
				StartCoroutine (GenerateNewSpinLine (LineGenRate, false));
				break;
			case 2:
				StartCoroutine (GenerateNewLine (LineGenRate, false));
				break;
			}
		} else {
			int r = Random.Range (1, 3);
			switch (r) {
			case 1:
				StartCoroutine (GenerateNewSpinLine (LineGenRate, true));
				break;
			case 2:
				StartCoroutine (GenerateNewLine (LineGenRate, true));
				break;
			}
		}

	}

	IEnumerator ProgressHarderLevel ()
	{
		yield return new WaitForSeconds (3f);
		if (LineGenRate >= 3f) {
			LineGenRate -= 0.3f;
		}
		if (MovingRate <= 15f)
			MovingRate += 1f;
	}


	public void RestartGame ()
	{
		int score;
		int.TryParse (Score.text, out score);
		if (score > bestscore) {
			Debug.Log ("Change Best Score");
			PlayerPrefs.SetInt ("BestScore", score);
		}
		restart ();
	}

	void restart ()
	{
		SceneManager.LoadScene ("Main");
	}

	IEnumerator GenerateNewSpinLine (float time, bool canSpin)
	{
		yield return new WaitForSeconds (time);
		GameObject Spinline = Instantiate (SpinLinePrefab, new Vector3 (0, Camera.main.orthographicSize), Quaternion.identity, AllGameObjects.transform);
		Spinline.GetComponent<SpinLineControl> ().Init (NUM);
		SpinLinePrefab.GetComponent<SpinLineControl> ().CanSpin = canSpin;
		NUM++;
		GenerateNewObstacles ();
	}

	IEnumerator GenerateNewLine (float time, bool canClose)
	{
		yield return new WaitForSeconds (time);
		GameObject Line = Instantiate (LinePrefab, new Vector3 (0, Camera.main.orthographicSize), Quaternion.identity, AllGameObjects.transform);
		Line.GetComponent<LineControl> ().Init (NUM);
		Line.GetComponent<LineControl> ().CanClose = canClose;
		NUM++;
		GenerateNewObstacles ();
	}

	void Update ()
	{
		if (GameStart) {
//			Tiime -= Time.deltaTime;
//			Timer.text = Tiime.ToString ("F1");
//			if (Tiime <= 0f) {
//				GameOver ();
//			}
			foreach (Transform child in AllGameObjects.transform) {
				child.Translate (Vector3.down * Time.deltaTime * MovingRate);
			}
		}
	}

	public void GameOver ()
	{
		if (SquareNum <= 0) {
			int score;
			int.TryParse (Score.text, out score);
			if (score > bestscore) {
				Debug.Log ("Change Best Score");
				PlayerPrefs.SetInt ("BestScore", score);
			}
			restart ();
		}

	}
		
}


public static class Rigidbody2DExt
{

	public static void AddExplosionForce (this Rigidbody2D rb, float explosionForce, Vector2 explosionPosition, float explosionRadius, float upwardsModifier = 0.0F, ForceMode2D mode = ForceMode2D.Force)
	{
		var explosionDir = rb.position - explosionPosition;
		var explosionDistance = explosionDir.magnitude;

		// Normalize without computing magnitude again
		if (upwardsModifier == 0)
			explosionDir /= explosionDistance;
		else {
			// From Rigidbody.AddExplosionForce doc:
			// If you pass a non-zero value for the upwardsModifier parameter, the direction
			// will be modified by subtracting that value from the Y component of the centre point.
			explosionDir.y += upwardsModifier;
			explosionDir.Normalize ();
		}

//		rb.AddForce (Mathf.Lerp (0, explosionForce, (1 - explosionDistance)) * explosionDir, mode);
		rb.AddForce (explosionForce * explosionDir * (explosionRadius + 10 - explosionDistance), mode);
	}
}