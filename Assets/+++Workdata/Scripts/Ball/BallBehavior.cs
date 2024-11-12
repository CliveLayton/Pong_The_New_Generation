using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BallBehavior : MonoBehaviour
{
    #region Variables

    [SerializeField] private float startingSpeed = 3f;
    [SerializeField] private BoxCollider2D hyphenCollider;
    [SerializeField] private PointCounter pointCounter;
    
    private Rigidbody2D rb;
    private CircleCollider2D col;

    #endregion

    #region UnityMethods

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        bool isRight = Random.value >= 0.5f;

        float xVelocity = isRight ? 1 : -1;
        float yVelocity = Random.Range(-1, 1);

        rb.velocity = new Vector2(xVelocity * startingSpeed, yVelocity * startingSpeed);
    }

    private void Update()
    {
        if (rb.velocity.magnitude != startingSpeed)
        {
            rb.velocity = rb.velocity.normalized * startingSpeed;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //float currentSpeed = rb.velocity.magnitude;
            
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y + other.rigidbody.velocity.y, -10, 10));

            //rb.velocity = rb.velocity.normalized * currentSpeed;
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



    #endregion
}
