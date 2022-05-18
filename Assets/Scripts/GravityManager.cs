using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityManager : MonoBehaviour
{
    [Tooltip("How much does air slow balls down.")]
    [Range(0.1f, 10.0f)]
    public float airResistanceForce = 0.01f;

    [Tooltip("How strong gravity works")]
    [Range(0.1f, 50.0f)]
    public float oneGravityBallPullPushForce = 1.0f;

    [Tooltip("How much more times is the gravitation radius catching other balls")]
    [Range(2.5f, 50.0f)]
    public float gravityDetectorRadiusMultiplier = 1.0f;

    [Tooltip("Radius of single Gravity Ball")]
    [Range(0.1f, 50.0f)]
    public float oneGravityBallRadius = 0.5f;

    [Tooltip("Random movement force - explosion")]
    [Range(1.0f, 100.0f)]
    public float after50GatheredExplosionMultiplier = 500.0f;

    [Tooltip("Mass of single Gravity Ball")]
    [Range(1.0f, 100.0f)]
    public float oneGravityBallMass = 5.0f;


    public static GravityManager instance = null;
    public TextBallsCounter onScreenBallCounter = null;
    public BallsSpawner ballSpawner = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
