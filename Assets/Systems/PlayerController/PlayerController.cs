using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;
using UnityEngine.TextCore.Text;
using UnityEngine.InputSystem;
using UnityEditor.Rendering.LookDev;

public class PlayerController : MonoBehaviour
{
    public enum MovementState
    {
        Idle,
        Walking,
        Sprinting,
        Crouching,
        Jumping,
        Falling
    }

    public MovementState currentMovementState;
    public float characterVelocity;
    
    #region Variables

    // Manager references
    private InputManager inputManager => GameManager.Instance.InputManager;

    private CharacterController characterController => GetComponent<CharacterController>();

    [SerializeField] private Transform cameraRoot;

    public Transform CameraRoot => cameraRoot;
    public Transform spawnPosition;


    [Header("Enable/Disable Controls & Features")]
    public bool moveEnabled = true;
    public bool lookEnabled = true;

    [SerializeField] private bool jumpEnabled = true;
    [SerializeField] private bool sprintEnabled = true;
    [SerializeField] private bool crouchEnabled = true;

    [Header("Input Values")]
    public Vector2 moveInput;
    public Vector2 lookInput;
    private bool sprintInput = false;
    [SerializeField] private bool crouchInput = false;
    private bool jumpInput = false;


    [Header("Movement Settings")]
    public float currentSpeed;
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float sprintSpeed = 10f;
    [SerializeField] private float crouchSpeed = 2f;

    private float speedTransitionDuration = 0.05f;
    [SerializeField] private float currentMoveSpeed;

    private Vector3 velocity;

    // bools to trigger sprint/crouch states
    private bool isSprinting = false;

    [Header("Look Settings")]
    public float horizontalLookSensitivity = 10f;
    public float verticalLookSensitivity = 30f;
    public float lowerLookLimit = -60f;
    public float upperLookLimit = 60f;

    public bool invertLookY { get;  private set; } = false;


    [Header("Jump & Gravity Settings")]
    [SerializeField] private bool isGrounded;
    [SerializeField] private float jumpHeight = 2.5f;
    [SerializeField] private float gravity = 30.0f;
    private float jumpCooldown = 0.1f;
    private float jumpCooldownTimer = 0f;
    private bool jumpRequested = false;

    // crouching variables
    private float standingHeight;
    private Vector3 standingCenter;
    private float standingCamY;
    private bool isObstructed = false;
    [Header("Crouch Settings")]
    [SerializeField] private float crouchTransitionDuration = 0.2f; // Time in seconds for crouch/stand transition
    [SerializeField] private float crouchingHeight = 1.0f;
    [SerializeField] private Vector3 crouchingCenter = new Vector3(0, 0.5f, 0);
    [SerializeField] private float crouchingCamY = 0.75f;
 
    private float targetHeight;
    private Vector3 targetCenter;
    private float targetCamY; // Target Y position for camera root during crouch transition

    private int playerLayerMask;

    #endregion

    private void Awake()
    {
        playerLayerMask = ~LayerMask.GetMask("Player");

        #region Initialize Default values
        currentMovementState = MovementState.Idle;


        // Initialize crouch variables
        standingHeight = characterController.height;
        standingCenter = characterController.center;
        standingCamY = cameraRoot.localPosition.y;

        targetHeight = standingHeight;
        targetCenter = standingCenter;
        targetCamY = cameraRoot.localPosition.y;

        // initialize current move speed to walk speed
        currentMoveSpeed = walkSpeed;

        crouchInput = false;
        sprintInput = false;

        #endregion
    }

    public void HandlePlayerMovement()
    {
        characterVelocity = characterController.velocity.magnitude;

        if (!moveEnabled)
        {
            Debug.LogWarning("Player movement is disabled!");
            return;
        }

        // Determine movement state
        DetermineMovementState();   

        // perform ground check
        GroundedCheck();

        // handle crouch transition
        HandleCrouchTransition();

        // apply movement
        ApplyMovement();
    }

    private void DetermineMovementState()
    {
        // determine what movement state we are in

        // if the player is not grounded, they are either jumping or falling
        if (!isGrounded)
        {
            // check if the player is moving upwards or downwards
            if (velocity.y > 0.1f) // jumping
            {
                currentMovementState = MovementState.Jumping;
            }
            else if (velocity.y < 0) // falling
            {
                currentMovementState = MovementState.Falling;
            }
            return; // exit early since we are airborne
        }

        else if (isGrounded)
        {
            // CROUCHING
            if (crouchInput == true || isObstructed == true)
            {
                currentMovementState = MovementState.Crouching;
            }

            // SPRINTING
            else if (sprintInput == true && currentMovementState != MovementState.Crouching)
            {
                currentMovementState = MovementState.Sprinting;
            }

            // WALKING
            else if (moveInput.magnitude > 0.1f && sprintInput == false && crouchInput == false)
            {
                currentMovementState = MovementState.Walking;
            }

            // IDLE
            else if( moveInput.magnitude <= 0.1f)
            {
                currentMovementState = MovementState.Idle;
            }
        }
    }

    private void ApplyMovement()
    {
        // get input direction
        Vector3 moveInputDirection = new Vector3(moveInput.x, 0, moveInput.y);
        Vector3 worldMoveDirection = transform.TransformDirection(moveInputDirection);

        // smoothly transition to target move speed based on movement state
        float targetMoveSpeed;

        switch (currentMovementState)
        {
            case MovementState.Idle: { targetMoveSpeed = 0f; break; }
            case MovementState.Walking: { targetMoveSpeed = walkSpeed; break; }
            case MovementState.Sprinting: { targetMoveSpeed = sprintSpeed; break; }
            case MovementState.Crouching: { targetMoveSpeed = crouchSpeed; break; }
            case MovementState.Jumping:
            case MovementState.Falling:
                targetMoveSpeed = currentMoveSpeed; // maintain current speed while airborne
                break;
            default: { targetMoveSpeed = walkSpeed; break; }
        }

        // Calculate lerp speed based on desired duration
        float lerpSpeed = 1f - Mathf.Pow(0.01f, Time.deltaTime / speedTransitionDuration);
        currentMoveSpeed = Mathf.Lerp(currentMoveSpeed, targetMoveSpeed, lerpSpeed);

        Vector3 horizontalMovement = worldMoveDirection * currentMoveSpeed;

        ApplyJumpAndGravity();

        Vector3 movement = horizontalMovement;
        movement.y = velocity.y;

        characterController.Move(movement * Time.deltaTime);
    }

    public void HandlePlayerLook()
    {
        if (!lookEnabled) return;

        float mouseX = lookInput.x * horizontalLookSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * verticalLookSensitivity * Time.deltaTime * (invertLookY ? 1 : -1);

        // rotate the player object horizontally
        transform.Rotate(Vector3.up * mouseX);

        // rotate the camera vertically
        Vector3 currentRotation = cameraRoot.localEulerAngles;
        float desiredX = currentRotation.x + mouseY;

        // clamp the vertical rotation to prevent flipping
        if (desiredX > 180) desiredX -= 360; // convert to -180 to 180 range
        desiredX = Mathf.Clamp(desiredX, lowerLookLimit, upperLookLimit);

        cameraRoot.localEulerAngles = new Vector3(desiredX, 0, 0);
    }

    void SetMoveInput(Vector2 inputVector)
    {
        moveInput = new Vector2(inputVector.x, inputVector.y ); // later * walkspeed or sprintSpeed
        Debug.Log($"Move Input: {moveInput}");
    }

    void SetLookInput(Vector2 inputVector)
    {
        lookInput = new Vector2(inputVector.x, inputVector.y);
        //Debug.Log($"Look Input: {lookInput}");
    }

    void SetSprintInput(bool isSprinting)
    {
        if (!sprintEnabled) return;

        sprintInput = isSprinting;
        //sprintInput = !sprintInput;
        Debug.Log($"Sprint Input Toggled: {sprintInput}");
    }

    void HandleCrouchInput()
    {
        if (!crouchEnabled) return;

        crouchInput = !crouchInput;
        currentMoveSpeed = crouchSpeed; // immediately set speed to crouch or walk speed
        Debug.Log("Crouch Input Toggled");
    }

    private void GroundedCheck()
    {
        isGrounded = characterController.isGrounded;
    }

    public void MovePlayerToSpawnPosition(Transform spawnPoint)
    {
        characterController.enabled = false;
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;
        characterController.enabled = true;

        velocity = Vector3.zero;
        cameraRoot.localEulerAngles = Vector3.zero;
    }

    void HandleJumpInput()
    {
        Debug.Log("Jump Started");

        if (jumpEnabled == true && isGrounded && jumpCooldownTimer <= 0f)
        {
            jumpRequested = true;

            jumpCooldownTimer = 0.1f; // small cooldown to prevent multiple jumps
        }
    }

    private void ApplyJumpAndGravity()
    {
        if(jumpEnabled == false) return;

        if (jumpRequested)
        {
            // calculate jump velocity using the jump height and gravity
            velocity.y = Mathf.Sqrt(jumpHeight * 2f * gravity);

            // reset the jump request flag so it only triggers once
            jumpRequested = false;

            // start jump cooldown timer to prevent immediate re-jump
            jumpCooldownTimer = jumpCooldown;
        }

        if (isGrounded && velocity.y < 0f)
        {
            velocity.y = -1f; // small downward force to keep grounded
        }
        else // apply gravity when not grounded
        {
            velocity.y -= gravity * Time.deltaTime;
        }

        // update jump cooldown timer
        if (jumpCooldownTimer > 0f)
        {
            jumpCooldownTimer -= Time.deltaTime;
        }
    }

    private void HandleCrouchTransition()
    {
        bool shouldCrouch = crouchInput == true;
 
        // if airborne and was crouching, maintain crouch state (prevents standing up from crouch while walking off a ledge)
        bool wasAlreadyCrouching = characterController.height < (standingHeight - 0.05f);
 
        if (isGrounded == false && wasAlreadyCrouching)
        {
            shouldCrouch = true; // Maintain crouch state if airborne (walking off ledge while crouching)
        }
 
        if (shouldCrouch)
        {
            targetHeight = crouchingHeight;
            targetCenter = crouchingCenter;
            targetCamY = crouchingCamY;
            isObstructed = false; // No obstruction when intentionally crouching
        }
        else
        {
            float maxAllowedHeight = GetMaxAllowedHeight();

            //float maxAllowedHeight = 3.0f; // debug

            if (maxAllowedHeight >= standingHeight - 0.05f)
            {
                // No obstruction, allow immediate transition to standing
                targetHeight = standingHeight;
                targetCenter = standingCenter;
                targetCamY = standingCamY;
                isObstructed = false;
            }

            else
            {
                // Obstruction detected, limit height and center
                targetHeight = Mathf.Min(standingHeight, maxAllowedHeight);
                float standRatio = Mathf.Clamp01((targetHeight - crouchingHeight) / (standingHeight - crouchingHeight));
                targetCenter = Vector3.Lerp(crouchingCenter, standingCenter, standRatio);
                targetCamY = Mathf.Lerp(crouchingCamY, standingCamY, standRatio);
                isObstructed = true;
            }
        }
 
        // Calculate lerp speed based on desired duration
        // This formula ensures the transition approximately reaches 99% of the target in 'crouchTransitionDuration' seconds.
        float lerpSpeed = 1f - Mathf.Pow(0.01f, Time.deltaTime / crouchTransitionDuration);
 
        // Smoothly transition to targets
        characterController.height = Mathf.Lerp(characterController.height, targetHeight, lerpSpeed);
        characterController.center = Vector3.Lerp(characterController.center, targetCenter, lerpSpeed);
 
        Vector3 currentCamPos = cameraRoot.localPosition;
        cameraRoot.localPosition = new Vector3(currentCamPos.x, Mathf.Lerp(currentCamPos.y, targetCamY, lerpSpeed), currentCamPos.z);
    }

    private float GetMaxAllowedHeight()
    {
        // get ray upwards from player position to check for obstructions
        RaycastHit hit;
        float maxCheckDistance = standingHeight + 0.15f; // small buffer above standing height

        if (Physics.Raycast(transform.position, Vector3.up, out hit, maxCheckDistance, playerLayerMask))
        {
            // return the height from the ground to the obstruction
            float maxHeight = hit.distance - 0.1f; // small buffer below obstruction to prevent clipping

            maxHeight = Mathf.Max(maxHeight, crouchingHeight);

            return maxHeight;
        }
        // if we hit something..
        // calculate the max allowed height based on the hit distance
        // if nothing hit, no obstruction, return full standing height
        return standingHeight;
    }

    void OnEnable()
    {
        inputManager.MoveInputEvent += SetMoveInput;
        inputManager.LookInputEvent += SetLookInput;

        // sprint
        inputManager.SprintInputEvent += SetSprintInput; 
        
        // crouch
        inputManager.CrouchInputEvent += HandleCrouchInput;

        // jump
        inputManager.JumpInputEvent += HandleJumpInput;
    }

    void OnDestroy()
    {
        inputManager.MoveInputEvent -= SetMoveInput;
        inputManager.LookInputEvent -= SetLookInput;

        // sprint
        inputManager.SprintInputEvent -= SetSprintInput; 
        
        // crouch
        inputManager.CrouchInputEvent -= HandleCrouchInput;

        // jump
        inputManager.JumpInputEvent -= () => Debug.Log("Jump Started");
    }
}
