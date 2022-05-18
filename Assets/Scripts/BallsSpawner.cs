using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallsSpawner : MonoBehaviour
{


    private Transform _thisTransform = null;
    private List<GravityBall> _gravityBalls = new List<GravityBall>();

    [SerializeField] private GravityBall _prefabGravityBall = null;

    private const float BALL_SPAWN_FREQUENCY = 0.25f;
    private const int MAX_BALLS_COUNT = 250;

    private void Awake()
    {
        _thisTransform = this.transform;
    }
    private void changeGravityWork()
    {
        foreach (GravityBall gBall in _gravityBalls)
        {
            gBall.togglePullingPushing();
            gBall.stopHandlingCollision();
        }
    }

    private void spawnBallInCameraSpace(Camera camera)
    {

        CameraWorldSpace space = new CameraWorldSpace(camera);
        GravityBall gBall = Instantiate(_prefabGravityBall, space.randomCoordinate, Quaternion.identity, _thisTransform) as GravityBall;
        _gravityBalls.Add(gBall);
        GravityManager.instance.onScreenBallCounter.assignValueToTextCounter(_gravityBalls.Count);

    }


    private void invokeRepeating_SpawningBalls()
    {
        if (_gravityBalls.Count >= MAX_BALLS_COUNT)
        {
            CancelInvoke(nameof(invokeRepeating_SpawningBalls));
            changeGravityWork();
        }
        else
        {
            spawnBallInCameraSpace(Camera.main);
        }
    }

    void Start()
    {
        //_prefabGravityBall.setRadius(GravityManager.instance.oneGravityBallRadius);
        _prefabGravityBall.transform.localScale = GravityManager.instance.oneGravityBallRadius * 2.0f * Vector3.one;
        InvokeRepeating(nameof(invokeRepeating_SpawningBalls), BALL_SPAWN_FREQUENCY, BALL_SPAWN_FREQUENCY);
    }



}
