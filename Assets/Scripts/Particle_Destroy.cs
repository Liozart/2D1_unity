using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle_Destroy : MonoBehaviour {

	public int time = 5;

	// Use this for initialization
	void Start () {
		StartCoroutine (Destroy());
	}
	
	// Update is called once per frame
	void Update () {
	}

	IEnumerator Destroy()
	{
		yield return new WaitForSeconds(time);
		GameObject.Destroy(this.gameObject);
	}
}
