using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacdotPickup : MonoBehaviour {

    public delegate void DotPickup();
    public event DotPickup OnDotPickup;

	void OnColliderEnter2D(Collider2D col) {
        if (col.tag == "dot") {
            Pickup();
            Destroy(col.gameObject);
        }
    }

    public void Pickup() {
        if (OnDotPickup != null) {
            OnDotPickup();
        }
    }
}
