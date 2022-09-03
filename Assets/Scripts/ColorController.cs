using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorController : MonoBehaviour
{
    [SerializeField] private float _time = 1F;
    [SerializeField] private Color _color;
    void Start()
    {
        _color = GetRandomColor();
    }

    void Update()
    {
        UpdateColor();
    }

    private Color GetRandomColor() {
        return new Color(Random.value, Random.value, Random.value);
    }

    void UpdateColor() {
        if (_time - Time.deltaTime < 0) {
            _color = GetRandomColor();
            _time = 1F;
        }

        if (transform.childCount > 0) {
            MeshRenderer[] childRenderers = GetComponentsInChildren<MeshRenderer>();

            foreach (MeshRenderer childRenderer in childRenderers) {
                Color color = childRenderer.GetComponent<MeshRenderer>().material.color;
                childRenderer.material.color = Color.Lerp(color, _color, Time.deltaTime);
            }
        }

        _time -= Time.deltaTime;
    }
}
