using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDie {

    void Die();
}

public class DeathEvents : MonoBehaviour {

    public delegate void Die();
    public event Die OnDeath;

    public void TriggerDie() {
        if (OnDeath != null) {
            OnDeath();
        }
    }

}
