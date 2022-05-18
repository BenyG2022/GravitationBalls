using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushPullGravitation : MonoBehaviour
{
    public GravityBall gravityBall = null;
    public List<GravityBall> ballsInGravitationField = new List<GravityBall>();

    private Transform _thisParentTransform = null;
    private SphereCollider _collider = null;


    private void OnEnable()
    {
        this._collider.radius = GravityManager.instance.oneGravityBallRadius * GravityManager.instance.gravityDetectorRadiusMultiplier;

    }
    private void Awake()
    {
        gravityBall = this.GetComponentInParent<GravityBall>();
        _thisParentTransform = this.transform.parent;
        _collider = this.GetComponent<SphereCollider>();

    }


    private void FixedUpdate()
    {
        for (int i = 0; i < ballsInGravitationField.Count; i++)
        {
            GravityBall ball = ballsInGravitationField[i];
            if (ball == null || ball.gameObject.activeSelf == false)
            {
                ballsInGravitationField.Remove(ball);
            }
            else
            {
                ball.useGravity(_thisParentTransform);
            }
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        ballsInGravitationField.Add(other.gameObject.GetComponent<GravityBall>());
    }

    private void OnTriggerExit(Collider other)
    {
        ballsInGravitationField.Remove(other.gameObject.GetComponent<GravityBall>());
    }
}
