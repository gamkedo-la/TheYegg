using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDriftWobble : MonoBehaviour
{
    Quaternion initialRot;
    // Start is called before the first frame update
    void Start()
    {
        initialRot = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        float horizPace = 0.15f, horizTilt = 0.7f;
        float vertPace = 0.33f, vertTilt = 1.6f;
        transform.rotation = initialRot *
            Quaternion.AngleAxis(Mathf.Sin(Time.timeSinceLevelLoad* vertPace) * vertTilt, transform.right) *
            Quaternion.AngleAxis(Mathf.Sin(Time.timeSinceLevelLoad * horizPace) * horizTilt, transform.up);
    }
}
