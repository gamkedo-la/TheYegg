using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayNorthForAudioListener : MonoBehaviour
{
    // used to keep audio listener on player for volume & distance while keeping left/right camera relative
    void LateUpdate()
    {
        transform.rotation = Camera.main.transform.rotation;
    }
}
