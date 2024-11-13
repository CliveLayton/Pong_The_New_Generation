using System.Collections;
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
    private bool isWavyReflect = false;
    private bool isCurvyReflect = false;
    private BallBehavior ball;
    private bool wasPerformed = false;

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

        inputActions.Player.CurvyReflect.performed += CurvyReflect;
        inputActions.Player.WavyReflect.performed += WavyReflect;
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
        
        inputActions.Player.CurvyReflect.performed -= CurvyReflect;
        inputActions.Player.WavyReflect.performed -= WavyReflect;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            ball = other.gameObject.GetComponent<BallBehavior>();

            if (isWavyReflect)
            {
                ball.ChangeBallMovementWavy();
            }
            else if (isCurvyReflect)
            {
                ball.ChangeBallMovementCurvy();
            }
        }
    }

    #endregion

    #region InputMethods

    private void CurvyReflect(InputAction.CallbackContext context)
    {
        if (context.performed && !wasPerformed)
        {
            wasPerformed = true;
            isCurvyReflect = true;
            StartCoroutine(ResetReflection(true));
        }
    }

    private void WavyReflect(InputAction.CallbackContext context)
    {
        if (context.performed && !wasPerformed)
        {
            wasPerformed = true;
            isWavyReflect = true;
            StartCoroutine(ResetReflection(false));
        }
    }

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

    private IEnumerator ResetReflection(bool isCurvy)
    {
        if (isCurvy)
        {
            yield return new WaitForSeconds(0.2f);
            isCurvyReflect = false;
            wasPerformed = false;
        }
        else
        {
            yield return new WaitForSeconds(0.2f);
            isWavyReflect = false;
            wasPerformed = false;
        }
    }

    #endregion
}
