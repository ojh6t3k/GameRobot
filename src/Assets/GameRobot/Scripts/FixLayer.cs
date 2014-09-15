using UnityEngine;
using System.Collections;

public class FixLayer : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
		FixLayerUI();
	}

	void FixLayerUI()
	{
		gameObject.layer = 5;
		Invoke("FixLayerUI", 1f);
	}


//	// Update is called once per frame
//	void Update () {
//	
//	}
}
