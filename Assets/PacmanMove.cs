using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacmanMove : MonoBehaviour {
    public float speed = 0.4f;
    Vector2 dest = Vector2.zero;

    private Vector2 curInput = Vector2.zero;
    private Vector2 curDir = Vector2.zero;


    // Use this for initialization
    void Start() {
        dest = transform.position;
    }

    void Update() {
        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");


        // Check if we are getting input
        if (!(x == 0 && y == 0)) {
            curInput = new Vector2(Mathf.Sign(x), Mathf.Sign(y));
            // Mathf.Sign returns 1 for + and 0, we need to know which it is
            curInput.x = (x == 0) ? 0 : curInput.x;
            curInput.y = (y == 0) ? 0 : curInput.y;
        }
    }

    void FixedUpdate() {
        // Move closer to Destination
        Vector2 p = Vector2.MoveTowards(transform.position, dest, speed);
        GetComponent<Rigidbody2D>().MovePosition(p);

        if ((Vector2) transform.position == dest) {
            if (curInput.y == 1 && Valid(Vector2.up))
                curDir = curInput;
            if (curInput.x == 1 && Valid(Vector2.right))
                curDir = curInput;
            if (curInput.y == -1 && Valid(-Vector2.up))
                curDir = curInput;
            if (curInput.x == -1 && Valid(-Vector2.right))
                curDir = curInput;
        }

        if ((Vector2) transform.position == dest) {
            if (curDir.y == 1 && Valid(Vector2.up))
                dest = (Vector2) transform.position + Vector2.up;
            if (curDir.x == 1 && Valid(Vector2.right))
                dest = (Vector2) transform.position + Vector2.right;
            if (curDir.y == -1 && Valid(-Vector2.up))
                dest = (Vector2) transform.position - Vector2.up;
            if (curDir.x == -1 && Valid(-Vector2.right))
                dest = (Vector2) transform.position - Vector2.right;
        }

        Vector2 dir = dest - (Vector2) transform.position;
        GetComponent<Animator>().SetFloat("x", dir.x);
        GetComponent<Animator>().SetFloat("y", dir.y);
    }

    bool Valid(Vector2 dir) {
        // Cast Line from 'next to Pac-Man' to 'Pac-Man'
        Vector2 pos = transform.position;
        RaycastHit2D hit = Physics2D.Linecast(pos + dir, pos);
        Debug.DrawLine(pos + dir, pos, Color.green);
        return (hit.collider == GetComponent<Collider2D>() || hit.collider.gameObject.GetComponent<Pacdot>() != null);
    }
}
