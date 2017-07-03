//this script will create the ripples when clicked

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIRipple : MonoBehaviour
{
	
	/// <summary> 
	/// the Sprite that will render
	/// </summary>
	public Sprite ShapeSprite;
	
	/// <summary> 
	/// the speed at which the ripple will grow
	/// </summary>
	[Range (0.25f, 10f)]
	public float Speed = 1f;

	/// <summary> 
	/// The Maximum Size of the Ripple
	/// </summary>
	public float MaxSize = 4f;
	
	/// <summary> 
	/// Start Color of Ripple
	/// </summary>
	public Color StartColor = new Color (1f, 1f, 1f, 1f);

	/// <summary> 
	/// End Color of Ripple
	/// </summary>
	public Color EndColor = new Color (1f, 1f, 1f, 1f);

	/// <summary> 
	/// If true Ripples will appear on the top of all other children in the UI Element 
	/// </summary>
	public bool RenderOnTop = false;


	// Update is called once per frame
	void Update ()
	{
		#if UNITY_EDITOR_OSX
		//if the mouse button is down
		if (Input.GetMouseButtonDown (0)) {
			//create the Ripple
			CreateRipple (Camera.main.ScreenToWorldPoint (Input.mousePosition));
		}
		#endif

		#if UNITY_IOS
		foreach (Touch touch in Input.touches) {
			if (touch.phase == TouchPhase.Began) {
				//create the Ripple
				CreateRipple (Camera.main.ScreenToWorldPoint (Input.mousePosition));
			}
		}
		#endif
	}

	//this will create the Ripple
	public void CreateRipple (Vector2 Position)
	{
		//create the GameObject and add components
		GameObject ThisRipple = new GameObject ();
		ThisRipple.AddComponent<Ripple> ();
		ThisRipple.AddComponent<SpriteRenderer> ();
		ThisRipple.GetComponent<SpriteRenderer> ().sprite = ShapeSprite;
		ThisRipple.name = "Ripple";

		//set the parent
		ThisRipple.transform.SetParent (gameObject.transform);

		//rearrange the children if needed 
		if (RenderOnTop) {
			ThisRipple.transform.SetAsFirstSibling ();
		}

		ThisRipple.transform.position = Position;

		//set the parameters in the Ripple
		ThisRipple.GetComponent<Ripple> ().Speed = Speed;
		ThisRipple.GetComponent<Ripple> ().MaxSize = MaxSize;
		ThisRipple.GetComponent<Ripple> ().StartColor = StartColor;
		ThisRipple.GetComponent<Ripple> ().EndColor = EndColor;
	}

}
