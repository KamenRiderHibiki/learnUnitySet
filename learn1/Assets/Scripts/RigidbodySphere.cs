using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodySphere : MonoBehaviour
{
    [SerializeField, Range(0f, 100f)]
    float maxSpeed = 1f;
    [SerializeField, Range(0f, 10f)]
    float maxAccleration = 1f, maxAirAccleration = 1f;
    [SerializeField, Range(0f, 5f)]
    float jumpHeight = 2f;
    [SerializeField, Range(0, 5)]
    int maxAirJumps = 0;
    int jumpPhase;
    public Vector3 direction;
    float directionTimer;
    Vector3 lastPosition;
    public Vector3 velocity, desiredVelocity;
    bool desiredJump;
    bool onGround;
    Rigidbody body;
    void Awake()
    {
        body = GetComponent<Rigidbody>();
        velocity = desiredVelocity = new Vector3(0, 0);
        direction = Vector3.zero;
        lastPosition = transform.localPosition;
        jumpPhase = 0;
    }
    void Update()
    {
        
        Vector2 playerInput;
        playerInput.x = Input.GetAxis("Horizontal");
        playerInput.y = Input.GetAxis("Vertical");
        desiredJump |= Input.GetButtonDown("Jump");
        if (!playerInput.Equals(Vector2.zero))
        {
            playerInput = Vector2.ClampMagnitude(playerInput, 1f);
            desiredVelocity = new Vector3(playerInput.x, 0.0f, playerInput.y) * maxSpeed;
        }
        else
        {
            desiredVelocity = Vector3.zero;
        }
    }
    void FixedUpdate()
    {
        directionTimer += Time.deltaTime;
        if (directionTimer > 0.1f)
        {
            Vector3 mouse = Input.mousePosition;
            mouse.z = mouse.y;
            mouse.y = 0f;
            direction = mouse - lastPosition;
            lastPosition = mouse;
            directionTimer = 0f;
        }

        UpdateJumpState();
        if (desiredJump)
        {
            desiredJump = false;
            Jump();
        }

        float acc = onGround ? maxAccleration : maxAirAccleration;
        float speedChange = acc * Time.deltaTime;
        velocity = body.velocity;
        if(desiredVelocity != Vector3.zero)
        {
            velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, speedChange);
            velocity.z = Mathf.MoveTowards(velocity.z, desiredVelocity.z, speedChange);
        }
        body.velocity = velocity;

        onGround = false;
    }

    void UpdateJumpState()
    {
        if (onGround)
        {
            jumpPhase = 0;
        }
    }
    void Jump()
    {
        if (onGround || jumpPhase < maxAirJumps)
        {
            jumpPhase += 1;
            velocity = body.velocity;
            float jumpSpeed = Mathf.Sqrt(-2.0f * Physics.gravity.y * jumpHeight);
            velocity.y = Mathf.Max(jumpSpeed, velocity.y);
            body.velocity = velocity;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        onGround = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        EvaluateCollision(collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        EvaluateCollision(collision);
    }

    private void EvaluateCollision(Collision collision)
    {
        for(int i = 0; i < collision.contactCount; i++)
        {
            Vector3 normal = collision.GetContact(i).normal;
            onGround |= normal.y >= 0.9f;
        }
    }
}
