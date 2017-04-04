using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinkyTargeter : GhostTargeter {

    public Vector2 scatterTarget = new Vector2(3, 34);

    public override Vector2 GetTarget() {
        if (currentMode == GhostTargetMode.SCATTER) {
            return scatterTarget;
        } else if (currentMode == GhostTargetMode.CHASE) {
            var larr = GameObject.FindGameObjectWithTag("Player");
            var larrPos = (Vector2) larr.transform.position;
            larrPos = new Vector2(Mathf.Abs(larrPos.x), Mathf.Abs(larrPos.y));
            var larrDir = larr.GetComponent<PacmanMove>().currentDirection;

            Vector2 offset;
            if (larrDir == Vector2.up) {
                offset = 4 * (Vector2.up + Vector2.left);
            } else if (larrDir == Vector2.left) {
                offset = 4 * Vector2.left;
            } else if (larrDir == Vector2.down) {
                offset = 4 * Vector2.down;
            } else { // Right
                offset = 4 * Vector2.right;
            }

            return larrPos + offset;
        } else {
            throw new Exception("this probably shouldn't happen.");
        }
    }
}
