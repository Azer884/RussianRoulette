using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;

public class PlayerMovementAdvanced : NetworkBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;

    public float groundDrag;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;
    

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    ShootingSys shootingSys;
    [SerializeField]private GameObject objectToShowForOwner;
    [SerializeField]private GameObject objectToHideForOwner;

    public MovementState state;
    public enum MovementState
    {
        walking,
        sprinting,
        crouching,
        air
    }
    public Animator JumpAnim;
    public Transform target;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        readyToJump = true;

        startYScale = transform.localScale.y;

        shootingSys = GetComponent<ShootingSys>();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();


        if(!IsOwner)
        {
            shootingSys.enabled = false;
            enabled = false;
            objectToHideForOwner.SetActive(false);
            ChangeLayerRecursively(objectToShowForOwner, 0);
        }
        else
        {
            transform.position = new Vector3(0, 2, 1);
            objectToHideForOwner.SetActive(true);
        }
    }

    private void Update()
    {
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();
        SpeedControl();
        StateHandler();

        if(state == MovementState.air)
        {
            JumpAnim.SetBool("IsGrounded" , false); 
        }
        else
        {
            JumpAnim.SetBool("IsGrounded" , true); 
        }
        if(state == MovementState.crouching)
        {
            JumpAnim.SetBool("IsCrouching" , true); 
        }
        else
        {
            JumpAnim.SetBool("IsCrouching" , false); 
        }

        if (IsOwner)
        {
            target.localPosition = new Vector3(0f, 5f, objectToHideForOwner.transform.localRotation.x * 10);
        }

        // handle drag
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // when to jump
        if(Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }

        // start crouch
        if (Input.GetKeyDown(crouchKey))
        {
            readyToJump = false;
            if (IsOwner)
            {
                objectToHideForOwner.transform.localPosition = Vector3.up * .15f;
            }


            GetComponentInChildren<CapsuleCollider>().height = 1f;
            GetComponentInChildren<CapsuleCollider>().center = Vector3.up * -0.5f;
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
            playerHeight /= 2;
        }

        // stop crouch
        if (Input.GetKeyUp(crouchKey))
        {
            readyToJump = true;
            if (IsOwner)
            {
                objectToHideForOwner.transform.localPosition = Vector3.up * .537f;
            }


            GetComponentInChildren<CapsuleCollider>().height = 2f;
            GetComponentInChildren<CapsuleCollider>().center = Vector3.zero;
            playerHeight *= 2;
        }
    }

    private void StateHandler()
    {
        // Mode - Crouching
        if (Input.GetKey(crouchKey))
        {
            state = MovementState.crouching;
            moveSpeed = crouchSpeed;
        }

        // Mode - Sprinting
        else if(grounded && Input.GetKey(sprintKey))
        {
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
        }

        // Mode - Walking
        else if (grounded)
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
        }

        // Mode - Air
        else
        {
            state = MovementState.air;
        }
    }

    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on slope
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);

            if (rb.velocity.y > 0)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }

        // on ground
        else if(grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        // in air
        else if(!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

        // turn gravity off while on slope
        rb.useGravity = !OnSlope();
    }

    private void SpeedControl()
    {
        // limiting speed on slope
        if (OnSlope() && !exitingSlope)
        {
            if (rb.velocity.magnitude > moveSpeed)
                rb.velocity = rb.velocity.normalized * moveSpeed;
        }

        // limiting speed on ground or in air
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            // limit velocity if needed
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    private void Jump()
    {
        exitingSlope = true;

        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;

        exitingSlope = false;
    }

    private bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }



    void ChangeLayerRecursively(GameObject currentGameObject, int newLayer)
    {
        currentGameObject.layer = newLayer;

        foreach (Transform child in currentGameObject.transform)
        {
            ChangeLayerRecursively(child.gameObject, newLayer);
        }
    }
}