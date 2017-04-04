using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelPerfectCamera : MonoBehaviour {

    private new Camera camera;

	void Start () {
        camera = GetComponent<Camera>();
        camera.orthographicSize = Screen.height / (4 * 8);
    }

}
