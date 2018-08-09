using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour {

	GameObject player;
	Vector3 offset;

	public float shakeDuration;
	public float shakeAmount = 0.01f;
	public float decreaseFactor = 1.0f;
	Vector3 originalPos;

	public bool isPlayerDead = false;

	// Use this for initialization
	void Start () {
		player = GameObject.FindWithTag("Player");
		offset = transform.position - player.transform.position;
		originalPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (!isPlayerDead) {
			transform.position = player.transform.position + offset;
			originalPos = transform.position;
			if (shakeDuration > 0) {
				transform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
				shakeDuration -= Time.deltaTime * decreaseFactor;
			} else {
				shakeDuration = 0f;
				transform.localPosition = originalPos;
			}
		}
	}

	public void Shake() {
		shakeDuration = 0.1f;
	}
}
