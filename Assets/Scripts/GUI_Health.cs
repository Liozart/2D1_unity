using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI_Health : MonoBehaviour {

	Player player;
	Text healthText;

	// Use this for initialization
	void Start () {
		player = GameObject.FindWithTag("Player").GetComponent<Player>();
		Text[] ui = gameObject.GetComponents<Text>();
		healthText = ui[0];
	}
	
	// Update is called once per frame
	void Update () {
		if (player.health > 0)
			healthText.text = "Health " + player.health;
		else
			healthText.text = "";
	}
}
