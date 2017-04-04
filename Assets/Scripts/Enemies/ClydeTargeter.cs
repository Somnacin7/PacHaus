using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClydeTargeter : GhostTargeter {

    public Vector2 scatterTarget = new Vector2(1, -2);

    public override Vector2 GetTarget() {
        if (currentMode == GhostTargetMode.SCATTER) {
            return scatterTarget;
        } else if (currentMode == GhostTargetMode.CHASE) {
            var larr = GameObject.FindGameObjectWithTag("Player");
            var larrPos = larr.transform.position;
            larrPos = new Vector2(Mathf.Abs(larrPos.x), Mathf.Abs(larrPos.y));

            var clydePos = (Vector2) transform.position;
            clydePos = new Vector2(Mathf.Abs(clydePos.x), Mathf.Abs(clydePos.y));

            var dist = Vector2.Distance(clydePos, larrPos);

            if (dist > 8) {
                return larrPos;
            } else {
                return scatterTarget;
            }
        } else {
            throw new Exception("this probably shouldn't happen.");
        }
    }
}
