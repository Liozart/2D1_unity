using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI_Inventory : MonoBehaviour {

	Inventory inv;
	Text invlist;

	// Use this for initialization
	void Start () {
		inv = GameObject.FindWithTag("Player").GetComponent<Inventory>();
		Text[] ui = gameObject.GetComponents<Text>();
		invlist = ui[0];
	}

	// Update is called once per frame
	void Update () {
		invlist.text = "Inventory " + inv.InventoryToString();
	}
}
