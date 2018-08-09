using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour {

	int strengh = 3;
	Rigidbody2D rigidbod;

	// Use this for initialization
	void Start () {
		rigidbod = this.gameObject.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D othercol) {
		if (othercol.gameObject.tag == "Enemy") {
			othercol.gameObject.GetComponent<Enemy_Slime>().Damage(strengh);
			rigidbod.velocity = Vector2.zero;
			StartCoroutine(Destroy());
		}
	}

	IEnumerator Destroy() {
		GetComponent<ParticleSystem>().Stop();
		yield return new WaitForSeconds(2);
		Destroy(this.gameObject);
	}
}
