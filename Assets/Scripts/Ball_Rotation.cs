using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball_Rotation : MonoBehaviour
{
    public float velocity = 7;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0f, 0f, velocity) * Time.deltaTime);
    }
}
