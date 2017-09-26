using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : PhysicsController {
    private float movementSpeed = 8f;
    private float jumpSpeed = 1f;
    private SpriteRenderer spriteRenderer;
    // Use this for initialization
    void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update () {

        targetVelocity = Vector2.zero;
        if (Input.GetKey(KeyCode.A))
        {
            targetVelocity = Vector2.left * movementSpeed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            targetVelocity += Vector2.right * movementSpeed;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            targetVelocity += Vector2.up * jumpSpeed;
        }
        if ((targetVelocity.x < 0 && !spriteRenderer.flipX) || (targetVelocity.x > 0 && spriteRenderer.flipX))
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }
    }
}
