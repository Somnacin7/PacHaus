using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GhostTargetMode {
    SCATTER = 0,
    CHASE = 1
}
public abstract class GhostTargeter : MonoBehaviour {

    public GhostTargetMode currentMode = GhostTargetMode.SCATTER;

    public abstract Vector2 GetTarget();
    protected void SwitchMode(GhostTargetMode mode) {
        currentMode = mode;
    }
}
