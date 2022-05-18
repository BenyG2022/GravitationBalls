using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWorldSpace
{

    public Vector3 randomCoordinate = Vector3.zero;

    public CameraWorldSpace(Camera camera)
    {
        Transform cameraTransform = camera.transform;
        float z = Random.Range(camera.nearClipPlane, Camera.main.farClipPlane);

        float horizontal = Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad) * z * 2.0f;
        float vertical = Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad) * z;

        float x = Random.Range(-horizontal, horizontal);
        float y = Random.Range(-vertical, vertical);
        randomCoordinate = cameraTransform.TransformPoint(new Vector3(x, y, z));
    }

}
