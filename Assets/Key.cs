using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    [Header("Key Properties")]
    public DoorKey keyType;

    public DoorKey GetKeyType(){
        return keyType;
    }

    public void DestroyKey(){
        Destroy(gameObject);
    }
}
