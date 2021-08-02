using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    [Header("Key Properties")]
    public DoorKey keyType;

    public DoorKey PickUpKey(){
        return keyType;
    }

    public void DestroyKey(){
        Destroy(gameObject);
    }
}
