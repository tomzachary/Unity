using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsController : MonoBehaviour {
    protected Vector2 velocity;
    protected Vector2 targetVelocity;
    public float minGroundNormalY = .65f;
    public float gravityModifier = 1f;
    protected Rigidbody2D rb2d;
    protected const float minMoveDistance = 0.001f;
    protected bool isGrounded;
    protected Vector2 groundNormal;
    //filter of stuff object can collide with, maps to Unity's matrix
    protected ContactFilter2D contactFilter;
    //will hold array of items we are going to hit on a move.
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
    protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);
        //creates a buffer around distance move.
    protected const float shellRadius = 0.01f;
    private bool isJumping = false;
    private int jumpCount = 0;
    private int jumpCountMax = 10;
    private void OnEnable()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }
    // Use this for initialization
    void Start () {
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;
    }
	
	// Update is called once per frame
	void Update () {
        targetVelocity = Vector2.zero;
	}
    void FixedUpdate() //for physics
    { if(targetVelocity.x != 0 || targetVelocity.y != 0)
        {
            var x = 1;//to break on movement
        }

        velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
        if (jumpCount < jumpCountMax) // to control height of jump
        {
            if (!isJumping && isGrounded)
            {
                isJumping = true;
            }
            if (isJumping)
            {
                velocity.y += targetVelocity.y;
                jumpCount++;
            }
        }
        isGrounded = false;
        velocity.x = targetVelocity.x;
        Vector2 deltaPosition = velocity * Time.deltaTime;

        Vector2 movementAlongGround = new Vector2(groundNormal.y, -groundNormal.x);//creates perpendicular lines.  1 on ground, and 90* from it
        Vector2 move = movementAlongGround * deltaPosition.x;
        Movement(move, false);
        move = Vector2.up * deltaPosition.y;
        Movement(move, true);
        if (isJumping && isGrounded)// trigger reset of jump count
        {
            isJumping = false;
            jumpCount = 0;
        }
    }

    void Movement(Vector2 move, bool yMovement)
    {        
        //gets total distance expecting to move
        float distance = move.magnitude;
        //makes sure there is a significant move to go through code
        if(distance > minMoveDistance)
        {
            //checks to see if any object will be collided with on the new movement location
            int count = rb2d.Cast(move, contactFilter, hitBuffer, distance + shellRadius);

            //copies over collided objects to a new list, so we can loop through them.
            hitBufferList.Clear();
            for(int i = 0; i < count; i++)
            {
                hitBufferList.Add(hitBuffer[i]);
            }

            for(int i = 0; i < hitBufferList.Count; i ++)
            {

                //grabs the normal of the object and checks to see if the normal is higher than normal ground normal, for slopes.  If it is not, considered grounded.
                Vector2 currentNormal = hitBufferList[i].normal;
                if(currentNormal.y > minGroundNormalY) {
                    isGrounded = true;
                    if (yMovement)
                    {
                        groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }

                float projection = Vector2.Dot(velocity, currentNormal);
                if (projection < 0)
                {
                    velocity = velocity - projection * currentNormal;
                }
                float modifiedDistance = hitBufferList[i].distance - shellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }

        }
        rb2d.position = rb2d.position + move.normalized * distance;
        
    }
}
