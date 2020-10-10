using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteScroller : MonoBehaviour {

    public float scrollSpeed = 5f;
    Vector2 startPos;

    private float currentPos;

    void Start() {
        startPos = transform.localPosition;
    }

    void FixedUpdate() {
        currentPos += 1 * scrollSpeed;
        // TODO: get screen size and set as max
        if (currentPos > 1920) {
            currentPos = 0;
        }
        transform.localPosition = startPos + (Vector2.right * currentPos);
    }
}
