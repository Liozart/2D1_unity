using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI_Death : MonoBehaviour {

	public bool isPlayerDead = false;

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<Text>().canvasRenderer.SetAlpha(0.01f);

	}
	
	// Update is called once per frame
	void Update () {
		if (isPlayerDead) {
			gameObject.GetComponent<Text>().CrossFadeAlpha(1.0f, 3.0f, false );
			isPlayerDead = false;
		}
	}
}
