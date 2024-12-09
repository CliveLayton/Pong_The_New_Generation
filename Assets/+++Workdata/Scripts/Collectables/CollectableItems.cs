using System;
using System.Collections;
using Unity.VisualScripting;
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
        ThrowBackCircle,
        CircleMove,
        CurveMove,
        WaveMove,
        MirrorField
    }

    [Header("Item Effect")]
    [SerializeField] private Effect effect;

    [Header("Item Color")] 
    [ColorUsage(true, true)]
    [SerializeField] private Color glowColor;
    [SerializeField] private SpriteRenderer iconRenderer;

    private BallBehavior ball;

    private int glowColorProperty = Shader.PropertyToID("_Glow_Color");
    private Material itemMaterial;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        itemMaterial = GetComponent<SpriteRenderer>().material;

        itemMaterial.SetColor(glowColorProperty, glowColor);
        iconRenderer.material.SetColor(glowColorProperty, glowColor);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            ball = other.GetComponent<BallBehavior>();

            switch (effect)
            {
                case Effect.SpeedUp:
                    ball.SpeedBallUp();
                    break;
                case Effect.SlowDown:
                    ball.SlowBallDown();
                    break;
                case Effect.ThrowBack:
                    ball.ThrowBallBack();
                    break;
                case Effect.ThrowBackCircle:
                    ball.ThrowBallBackInCircles();
                    break;
                case Effect.CircleMove:
                    ball.ChangeBallMovementCircle();
                    break;
                case Effect.WaveMove:
                    ball.ChangeBallMovementWavy();
                    break;
                case Effect.CurveMove:
                    ball.ChangeBallMovementCurvy();
                    break;
                case Effect.MirrorField:
                    ball.MirrorField();
                    break;
            }
            
            Destroy(gameObject);
        }
    }

    #endregion

    #region Collectable Items Methods

    

    #endregion
}
