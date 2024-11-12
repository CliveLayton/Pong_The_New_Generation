using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class CollectableItems : MonoBehaviour
{
    #region Variables

    private enum Effect
    {
        SpeedUp,
        SlowDown,
        ThrowBack,
        CurveMove,
        WaveMove,
        MirrorField
    }

    [Header("Item Effect")]
    [SerializeField] private Effect effect;
    
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
    
    [Header("Mirror Field Item")]
    [SerializeField] private float rotationSpeed = 50f;

    private BallBehavior ball;
    private BoxCollider2D col;
    private SpriteRenderer sr;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        col = GetComponent<BoxCollider2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            ball = other.GetComponent<BallBehavior>();
            col.enabled = false;
            sr.enabled = false;

            switch (effect)
            {
                case Effect.SpeedUp:
                    SpeedBallUp();
                    break;
                case Effect.SlowDown:
                    SlowBallDown();
                    break;
                case Effect.ThrowBack:
                    ThrowBallBack();
                    break;
                case Effect.WaveMove:
                    ChangeBallMovementWavy();
                    break;
                case Effect.CurveMove:
                    break;
                case Effect.MirrorField:
                    MirrorField();
                    break;
            }
        }
    }

    #endregion

    #region Collectable Item Methods

    public void SpeedBallUp()
    {
        float targetSpeed = Mathf.Clamp(ball.currentSpeed + Random.Range(minSpeedValue, maxSpeedValue), minSpeedLimit, maxSpeedLimit);
        StartCoroutine(ChangeSpeedOverTime(targetSpeed, timeToEffectBall));
    }
    
    private void SlowBallDown()
    {
        float targetSpeed =
            Mathf.Clamp(ball.currentSpeed - Random.Range(minSpeedValue, maxSpeedValue), minSpeedLimit, maxSpeedLimit);
        StartCoroutine(ChangeSpeedOverTime(targetSpeed, timeToEffectBall));
    }

    private void ThrowBallBack()
    {
        Rigidbody2D ballRb = ball.GetComponent<Rigidbody2D>();

        ballRb.velocity = -ballRb.velocity;
        
        Destroy(gameObject);
    }

    private void ChangeBallMovementWavy()
    {
        Rigidbody2D ballRb = ball.GetComponent<Rigidbody2D>();

        StartCoroutine(ChangeMovementToWavy(ballRb, ball.transform.position.y, effectTime));
    }

    private void ChangeBallMovementCurvy()
    {
        
    }

    private void MirrorField()
    {
        GameObject gameField = GameObject.FindGameObjectWithTag("GameField");

        if (gameField.transform.rotation.y == 180)
        {
            gameField.transform.rotation = Quaternion.Slerp(gameField.transform.rotation, Quaternion.identity, rotationSpeed * Time.deltaTime); 
        }

        if (gameField.transform.rotation.y == 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(-transform.forward, Vector3.up);

            gameField.transform.rotation = Quaternion.Slerp(gameField.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        
        Destroy(gameObject);
    }

    private IEnumerator ChangeSpeedOverTime(float targetSpeed, float duration)
    {
        float startSpeed = ball.currentSpeed;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            ball.currentSpeed = Mathf.Lerp(startSpeed, targetSpeed, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null; // Wait until the next frame
        }

        // Ensure the final speed is set to the target speed at the end
        ball.currentSpeed = targetSpeed;
        Destroy(gameObject);
    }

    private IEnumerator ChangeMovementToWavy(Rigidbody2D ballRb ,float initialYPosition ,float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            //increment time to move the wave forward
            elapsed += Time.deltaTime;

            //calculate the new y position based on the sine function
            float newY = Mathf.Sin(elapsed * waveFrequency) * waveAmplitude;
            Vector2 orthogonalVector = new Vector2(ballRb.velocity.y, -ballRb.velocity.x);

            
            //set the velocity to move horizontally with a sinusoidal y component
            ballRb.AddForce(orthogonalVector * (newY * (Time.deltaTime * 50)));
            
            yield return null; //wait until the next frame
        }
        Destroy(gameObject);
    }
    
    private IEnumerator ChangeMovementToCircle(Rigidbody2D ballRb ,float initialYPosition ,float duration)
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
            ballRb.AddForce(new Vector2(newX,newY) * (Time.deltaTime * 500));
            
            yield return null; //wait until the next frame
        }
        Destroy(gameObject);
    }
    
    private IEnumerator ThrowBackCircle(Rigidbody2D ballRb ,float initialYPosition ,float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            //increment time to move the wave forward
            elapsed += Time.deltaTime;

            //calculate the new y position based on the sine function
            float newY = Mathf.Sin(elapsed * waveFrequency) * waveAmplitude;
            Vector2 orthogonalVector = new Vector2(ballRb.velocity.y, -ballRb.velocity.x);

            
            //set the velocity to move horizontally with a sinusoidal y component
            ballRb.AddForce(orthogonalVector * (newY * (Time.deltaTime * 500)));
            
            yield return null; //wait until the next frame
        }
        Destroy(gameObject);
    }

    #endregion
}
