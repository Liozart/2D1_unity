using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum AttackType {
	Normal, Strong
}

public class Entity : MonoBehaviour {

	public int health;
	public int strengh;
	public float pushStrengh;
	float maxWalkSpeed;
	float maxRunningAccel;
	float runningAccel;
	float jumpForce;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
