using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Variables

    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float speedChangeRate = 10f;
    
    private PlayerInputMap inputActions;
    private InputAction moveAction;

    private Rigidbody2D rb;
    private BoxCollider2D col;
    private Vector2 lastMovement;
    private Vector2 moveInput;
    private bool downMovement;
    private int directionMultiply;

    #endregion

    #region UnityMethods

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();

        inputActions = new PlayerInputMap();
        moveAction = inputActions.Player.Move;
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Update()
    {
        moveInput = moveAction.ReadValue<Vector2>();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
    }

    #endregion

    #region InputMethods



    #endregion

    #region PlayerMethods

    private void Movement()
    {
        float currentSpeed = lastMovement.magnitude;
        
        if (moveInput.y > 0)
        {
            downMovement = false;
        }
        else if (moveInput.y < 0)
        {
            downMovement = true;
        }
        
        directionMultiply = downMovement ? -1 : 1;

        float targetSpeed = (moveInput.y == 0 ? 0 : movementSpeed * moveInput.magnitude);

        if (Mathf.Abs(currentSpeed - targetSpeed) > 0.01f)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, speedChangeRate * Time.deltaTime);
        }
        else
        {
            currentSpeed= targetSpeed;
        }

        rb.velocity = new Vector2(0, currentSpeed * directionMultiply);

        lastMovement.y = currentSpeed;
    }

    #endregion
}
