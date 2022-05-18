using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityBall : MonoBehaviour
{
    private Transform _thisTransform = null;
    private Rigidbody _thisRigidbody = null;
    private List<GravityBall> _disabledSubpartedBalls = new List<GravityBall>();
    public bool collisionHandled = false;

    private float _radius = 0.5f;
    private float _totalArea = 3.14f;
    private float _gravityForce = 10.0f;

    public int ballSubpartCount = 1;

    private const float COLLISION_OFF_TIME = 0.5f;
    private const int BALL_SUBPART_EXPLOSION_THRESHOLD = 50;

    private event System.Action<GravityBall> _onCollisionUpdate = null;

    private void Awake()
    {
        _thisTransform = this.transform;
        _thisRigidbody = this.GetComponent<Rigidbody>();
        _radius = GravityManager.instance.oneGravityBallRadius;
    }
    private void OnEnable()
    {
        _onCollisionUpdate += joinMeWithOther;
    }
    private void OnDisable()
    {
        _onCollisionUpdate -= joinMeWithOther;
    }
    private void OnCollisionEnter(Collision collision)
    {
        _onCollisionUpdate?.Invoke(collision.collider.gameObject.GetComponent<GravityBall>());
    }
    private void FixedUpdate()
    {
        if (this._thisRigidbody.velocity.magnitude > 0.05f)
            this._thisRigidbody.AddForce(-this._thisRigidbody.velocity * Time.fixedDeltaTime * GravityManager.instance.airResistanceForce);
    }


    public void stopHandlingCollision() => this._onCollisionUpdate -= joinMeWithOther;

    private void setRadius()
    {
        _totalArea = 4.0f * Mathf.PI * GravityManager.instance.oneGravityBallRadius * GravityManager.instance.oneGravityBallRadius * ballSubpartCount;
        _radius = Mathf.Sqrt(_totalArea / (4.0f * Mathf.PI));
        this.transform.localScale = _radius * 2.0f * Vector3.one;

    }

    public List<GravityBall> getSubpartSpheres() => _disabledSubpartedBalls;
    public void togglePullingPushing() => _gravityForce = -_gravityForce;
    public void afterHalfSecondTurnOnCollider() => Invoke(nameof(invoke_ExplosionCollisionOff), COLLISION_OFF_TIME);
    private void invoke_ExplosionCollisionOff() => this.GetComponent<Collider>().enabled = true;
    public void addExplosionForce() => this._thisRigidbody.AddForce(_gravityForce * GravityManager.instance.after50GatheredExplosionMultiplier * (new Vector3(Random.Range(-100.0f, 100.0f), Random.Range(-100.0f, 100.0f), Random.Range(-100.0f, 100.0f))).normalized * Time.fixedDeltaTime/*, ForceMode.Impulse*/);
    private void joinMeWithOther(GravityBall other)
    {
        if (other != null && other != this && other.collisionHandled == false)
        {
            other.collisionHandled = true;
            this.ballSubpartCount = this.ballSubpartCount + other.ballSubpartCount;
            this.resizeAndUpdateGravitation(other);
            if (this.ballSubpartCount >= BALL_SUBPART_EXPLOSION_THRESHOLD)
            {
                Debug.LogError("EXPLOSION HANDLED");
                explosion();
                this.setRadius();
            }
        }
    }

    private void explosion()
    {
        for (int i = 0; i < ballSubpartCount - 1; i++)
        {
            explosionHandler(subtractGravityBall());
        }
        explosionHandler(this);
    }

    private void explosionHandler(GravityBall ball)
    {
        ball.GetComponent<Collider>().enabled = false;
        ball.afterHalfSecondTurnOnCollider();
        ball.addExplosionForce();
        ball.getSubpartSpheres().Clear();
        ball.ballSubpartCount = 1;
        ball.collisionHandled = false;
        ball._thisRigidbody.mass = GravityManager.instance.oneGravityBallMass;
        ball._radius = GravityManager.instance.oneGravityBallRadius;
    }

    private GravityBall subtractGravityBall()
    {
        GravityBall gBall = _disabledSubpartedBalls[0];
        gBall.gameObject.SetActive(true);
        _disabledSubpartedBalls.RemoveAt(0);
        return gBall;
    }
   


    private void resizeAndUpdateGravitation(GravityBall otherBall)
    {
        _disabledSubpartedBalls.Add(otherBall);
        _disabledSubpartedBalls.AddRange(otherBall.getSubpartSpheres());
        otherBall.gameObject.SetActive(false);
        this._gravityForce = this.ballSubpartCount * GravityManager.instance.oneGravityBallPullPushForce;
        this.setRadius();
        this._thisRigidbody.mass = GravityManager.instance.oneGravityBallMass * ballSubpartCount;
    }

    public void useGravity(Transform body)
    {
        Vector3 targetDir = (body.position - _thisTransform.position);
        Vector3 bodyUp = body.up;

        body.rotation = Quaternion.FromToRotation(bodyUp, targetDir) * body.rotation;
        this._thisRigidbody.AddForce(targetDir * _gravityForce * Time.fixedDeltaTime);
    }
}
