using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float[] XminMax = new float[2];
    public float[] YminMax = new float[2];
    public static float followSpeed = 0.5f;
    Vector2 size;

    private void Start()
    {
        float height = Camera.main.orthographicSize;
        size = new Vector2(height * 16/9, height);
    }

    private void Update()
    {
        Vector3 whereCameraShouldBe = Vector3.Lerp(transform.position, PlayerMovement.instance.transform.position + Vector3.up, followSpeed * Time.deltaTime);
        Vector3 pos = whereCameraShouldBe;

        if (whereCameraShouldBe.x < XminMax[0] + size.x)
        {
            pos.x = XminMax[0] + size.x;
        }
        else if (whereCameraShouldBe.x > XminMax[1] - size.x)
        {
            pos.x = XminMax[1] - size.x;
        }
        if (whereCameraShouldBe.y < YminMax[0] + size.y - 1)
        {
            pos.y = YminMax[0] + size.y - 1;
        }
        else if (whereCameraShouldBe.y > YminMax[1] - size.y)
        {
            pos.y = YminMax[1] - size.y;
        }
        pos.z = -10;
        transform.position = pos;
    }
}
