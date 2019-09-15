using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleWithPerspective : MonoBehaviour
{

    Vector3 normalScale;

    private void Awake()
    {
        normalScale = transform.localScale;   
    }

    void Update()
    {
        float[] persValues = GameManager.map.perspectiveValues;
        transform.localScale = normalScale * (persValues[0] * transform.position.y + persValues[1]);
    }
}
