using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class BallBehavior : MonoBehaviour
{
    #region Variables

    public float currentSpeed;
    
    [SerializeField] private float startingSpeed = 3f;
    [SerializeField] private BoxCollider2D hyphenCollider;
    [SerializeField] private PointCounter pointCounter;

    [Header("Speed Change Item")]
    [SerializeField] private float timeToEffectBall = 0.5f;
    [SerializeField] private float maxSpeedValue = 10f;
    [SerializeField] private float minSpeedValue = 1f;
    [SerializeField] private float maxSpeedLimit = 12f;
    [SerializeField] private float minSpeedLimit = 2f;

    [Header("Wave Move Item")] 
    [Tooltip("Height of the wave")]
    [SerializeField] private float waveAmplitude = 1f;
    [Tooltip("Frequency of the wave")] 
    [SerializeField] private float waveFrequency = 2f;
    [SerializeField] private float effectTime;
    
    [Header("Curve Move Item")]
    public float curveIntensity = 1f;   // Intensity of the curve (how steep the curve is)
    public float curveDuration = 2f;    // Duration for which the curve pattern is active


    [Header("Mirror Field Item")]
    [SerializeField] private float rotationSpeed = 50f;

    private Rigidbody2D rb;
    private CircleCollider2D col;
    private float targetSpeed;
    private bool isChangingSpeed = false;
    private bool isWavy = false;
    private bool isCurvy = false;
    private GameObject gameField;
    private float startTime;

    #endregion

    #region UnityMethods

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CircleCollider2D>();
        currentSpeed = startingSpeed;
    }

    private void Start()
    {
        gameField = GameObject.FindGameObjectWithTag("GameField");
        startTime = Time.time;
        
        bool isRight = Random.value >= 0.5f;

        float xVelocity = isRight ? 1 : -1;
        float yVelocity = Random.Range(-1, 1);

        rb.velocity = new Vector2(xVelocity * startingSpeed, yVelocity * startingSpeed);
    }

    private void Update()
    {
        if (isChangingSpeed)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, timeToEffectBall);
            isChangingSpeed = false;
        }

        if (isWavy)
        {
            WavyMovement();
        }

        if (isCurvy)
        {
            CurvyMovement();
        }
        
        rb.velocity = rb.velocity.normalized * currentSpeed;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isWavy = false;
            isCurvy = false;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y + other.rigidbody.velocity.y, -10, 10));
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Goal1"))
        {
            pointCounter.CountPointsUp(2);
            this.transform.position = Vector3.zero;
        }

        if (other.gameObject.CompareTag("Goal2"))
        {
            pointCounter.CountPointsUp(1);
            this.transform.position = Vector3.zero;
        }
    }

    #endregion

    #region BallMethods

    public void SpeedBallUp()
    {
        targetSpeed = Mathf.Clamp(currentSpeed + Random.Range(minSpeedValue, maxSpeedValue), minSpeedLimit, maxSpeedLimit);
        isChangingSpeed = true;
    }
    
    public void SlowBallDown()
    {
        targetSpeed = Mathf.Clamp(currentSpeed - Random.Range(minSpeedValue, maxSpeedValue), minSpeedLimit, maxSpeedLimit);
        isChangingSpeed = true;
    }

    public void ThrowBallBack()
    {
        rb.velocity = -rb.velocity;
    }

    public void ThrowBallBackInCircles()
    {
        StartCoroutine(ThrowBackCircle(this.transform.position.y, effectTime));
    }

    public void ChangeBallMovementCircle()
    {
        StartCoroutine(ChangeMovementToCircle(this.transform.position.y, effectTime));
    }

    public void ChangeBallMovementWavy()
    {
        isWavy = true;
        //StartCoroutine(ChangeMovementToWavy(this.transform.position.y, effectTime));
    }

    public void ChangeBallMovementCurvy()
    {
        isCurvy = true;
    }

    public void MirrorField()
    {
        if (gameField.transform.rotation.eulerAngles.y != 0)
        {
            gameField.transform.rotation = Quaternion.Slerp(gameField.transform.rotation, Quaternion.identity, rotationSpeed * Time.deltaTime);
        } 
        else if (gameField.transform.rotation.eulerAngles.y == 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(-transform.forward, Vector3.up);

            gameField.transform.rotation = Quaternion.Slerp(gameField.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void WavyMovement()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
            
        // Apply wave force (vertical oscillation)
        float wave = Mathf.Sin((Time.time - startTime) * waveFrequency) * (waveAmplitude * 20);
        Vector2 waveForce = new Vector2(0, wave);

        // Add the vertical force to the Rigidbody2D
        rb.AddForce(waveForce, ForceMode2D.Force);
    }

    private void CurvyMovement()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        
        // Calculate a smooth, controlled curve using a normalized time factor
        float elapsedTime = Time.time - startTime;
        float curve = curveIntensity * Mathf.Sin(elapsedTime * Mathf.PI / curveDuration);

        // Apply vertical movement as a force
        Vector2 curveForce = new Vector2(0, curve);
        rb.AddForce(curveForce, ForceMode2D.Force);
    }

    private IEnumerator ChangeSpeedOverTime(float targetSpeed, float duration)
    {
        float startSpeed = currentSpeed;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            currentSpeed = Mathf.Lerp(startSpeed, targetSpeed, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null; // Wait until the next frame
        }

        // Ensure the final speed is set to the target speed at the end
        currentSpeed = targetSpeed;
    }

    private IEnumerator ChangeMovementToWavy(float initialYPosition ,float duration)
    {
        float elapsed = 0f;
    
        while (elapsed < duration)
        {
            //increment time to move the wave forward
            elapsed += Time.deltaTime;
    
            //calculate the new y position based on the sine function
            float newY = Mathf.Sin(elapsed * waveFrequency) * waveAmplitude;
            Vector2 orthogonalVector = new Vector2(rb.velocity.y, -rb.velocity.x);
    
            
            //set the velocity to move horizontally with a sinusoidal y component
            rb.AddForce(orthogonalVector * (newY * (Time.deltaTime * 50)));
            
            yield return null; //wait until the next frame
        }
    }
    
    private IEnumerator ChangeMovementToCircle(float initialYPosition ,float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            //increment time to move the wave forward
            elapsed += Time.deltaTime;

            //calculate the new y position based on the sine function
            float newY = Mathf.Sin(elapsed * waveFrequency) * waveAmplitude;
            float newX = Mathf.Cos(elapsed * waveFrequency) * waveAmplitude;
            
            //set the velocity to move horizontally with a sinusoidal y component
            rb.AddForce(new Vector2(newX,newY) * (Time.deltaTime * 500));
            
            yield return null; //wait until the next frame
        }
    }
    
    private IEnumerator ThrowBackCircle(float initialYPosition ,float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            //increment time to move the wave forward
            elapsed += Time.deltaTime;

            //calculate the new y position based on the sine function
            float newY = Mathf.Sin(elapsed * waveFrequency) * waveAmplitude;
            Vector2 orthogonalVector = new Vector2(rb.velocity.y, -rb.velocity.x);

            
            //set the velocity to move horizontally with a sinusoidal y component
            rb.AddForce(orthogonalVector * (newY * (Time.deltaTime * 500)));
            
            yield return null; //wait until the next frame
        }
    }

    #endregion
}
