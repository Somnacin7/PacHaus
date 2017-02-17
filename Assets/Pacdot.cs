using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pacdot : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D col) {
        if (col.name == "Pacman") {
            Destroy(gameObject);
        }
    }
}
