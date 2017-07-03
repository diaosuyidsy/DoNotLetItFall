using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareControl : MonoBehaviour
{
	
	public HashSet<int> Numbers;

	// Use this for initialization
	void Start ()
	{
		Numbers = new HashSet<int> ();
	}
}
