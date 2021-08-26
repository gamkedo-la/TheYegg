using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeySpawner : MonoBehaviour
{
    [Header("Key Spawner settings")]
    public bool hasKey = false;
    public DoorKey key;
    [SerializeField] GameObject keySpawnPoint;
    [Range(0f, 1f)]
    [SerializeField] float throwMaxDistance;
    [SerializeField] float throwForce;

    [Header("Key Prefabs")]
    public GameObject blueKey;
    public GameObject redKey;
    public GameObject yellowKey;
    public GameObject greenKey;

    private GameObject spawnedKey;

    public void SpawnKey(){
        if(hasKey){
            if(key == DoorKey.Blue){
                spawnedKey = Instantiate<GameObject>(blueKey, keySpawnPoint.transform.position, Quaternion.identity);
            }

            else if(key == DoorKey.Red){
                spawnedKey = Instantiate<GameObject>(redKey, keySpawnPoint.transform.position, Quaternion.identity);
            }

            else if(key == DoorKey.Green){
                spawnedKey = Instantiate<GameObject>(greenKey, keySpawnPoint.transform.position, Quaternion.identity);
            }

            else if(key == DoorKey.Yellow){
                spawnedKey = Instantiate<GameObject>(yellowKey, keySpawnPoint.transform.position, Quaternion.identity);
            }
            hasKey = false;
        }

        //throw the key to a random direction a tiny bit
        spawnedKey.GetComponent<Rigidbody>().AddForce(Random.Range(0f, throwMaxDistance), Random.Range(0f, throwMaxDistance), Random.Range(0f, throwMaxDistance), ForceMode.Impulse);
    }


}
