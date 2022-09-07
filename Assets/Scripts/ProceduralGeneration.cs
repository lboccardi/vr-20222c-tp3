using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGeneration : MonoBehaviour
{
    public Material SetBlockMaterial => _blockMaterial;
    [SerializeField] private Material _blockMaterial;

    [SerializeField] private int _circleRadius = 32;

    void Start()
    {
        for (int i = 1; i < _circleRadius; i++) {
            float incrementationDegrees = 360F / (12 * i);
            for (float degrees = 0; degrees < 360F; degrees += incrementationDegrees) {
                float angle = degrees * Mathf.PI / 180;
                GameObject block = GameObject.CreatePrimitive(PrimitiveType.Cube);
                block.transform.position =  transform.position + new Vector3(i * Mathf.Sin(angle), 0, i * Mathf.Cos(angle));
                block.transform.parent = transform;
                block.transform.localScale = new Vector3(0.3F, 0.01F, 0.3F);
                block.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360),0);
                block.GetComponent<Renderer>().material = _blockMaterial;
            }
        }
    }
}
