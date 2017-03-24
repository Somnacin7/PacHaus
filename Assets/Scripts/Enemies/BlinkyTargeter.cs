﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkyTargeter : GhostTargeter {

    public Vector2 scatterTarget = new Vector2(25, 32);

    public override Vector2 GetTarget() {
        if (currentMode == GhostTargetMode.SCATTER) {
            return scatterTarget;
        } else if (currentMode == GhostTargetMode.CHASE) {
            var larr = GameObject.FindGameObjectWithTag("Player");
            var larrPos = larr.transform.position;
            return new Vector2(Mathf.Abs(larrPos.x), Mathf.Abs(larrPos.y));
        } else {
            throw new Exception("this probably shouldn't happen.");
        }
    }
}
