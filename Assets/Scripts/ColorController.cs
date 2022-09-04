using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorController : MonoBehaviour
{
    [SerializeField] private float _time = 1F;
    [SerializeField] private Color _color;
    [SerializeField] private Color _colorInverted;
    [SerializeField] private GameObject[] _objectsToColor;

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

    private Color GetInvertedColor(Color color) {
        return new Color(1 - color.r, 1 - color.g, 1 - color.b);
    }

    void UpdateColor() {
        if (_time - Time.deltaTime < 0) {
            _color = GetRandomColor();
            _colorInverted = GetInvertedColor(_color);
            _time = 1F;
        }

        if (transform.childCount > 0) {
            MeshRenderer[] childRenderers = GetComponentsInChildren<MeshRenderer>();

            foreach (MeshRenderer childRenderer in childRenderers) {
                Color color = childRenderer.GetComponent<MeshRenderer>().material.color;
                childRenderer.material.color = Color.Lerp(color, _color, Time.deltaTime);
            }
        }

        foreach (GameObject gameObject in _objectsToColor) {
            // Gets primitives composing the music note prefab and color them
            MeshRenderer[] childRenderers = gameObject.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer childRenderer in childRenderers) {
                Color color = childRenderer.GetComponent<MeshRenderer>().material.color;
                // Color with inverted color for contrast
                childRenderer.material.color = Color.Lerp(color, _colorInverted, Time.deltaTime);
            }
        }

        _time -= Time.deltaTime;
    }
}
