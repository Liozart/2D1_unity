using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Weapon : Item {

	int strengh;

	public Item_Weapon(string n, int v, int w, int str) : base(n, v, w) {
		strengh = str;
	}
}
