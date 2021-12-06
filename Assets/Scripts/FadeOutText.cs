using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FadeOutText : MonoBehaviour
{
    public float fadeOutSpeed = 0.005f;
    public float riseUpSpeed = 0.001f;
    private TextMeshPro txt;
    private Color rgba;
    
    // Start is called before the first frame update
    void Start()
    {
        txt = GetComponent<TextMeshPro>();
        rgba = txt.color;
    }

    // Update is called once per frame
    void Update()
    {
        rgba.a-= fadeOutSpeed;
        if (rgba.a>0f) {
            //Debug.Log("Fading out a door closed fx prefab.");
            txt.color = rgba; // fade out
            // also rise up?
            transform.position = new Vector3(transform.position.x,transform.position.y+riseUpSpeed,transform.position.z);
        } else {
            //Debug.Log("Destroying a door closed fx prefab.");
            Destroy(this); // go away now
        }
    }
}
