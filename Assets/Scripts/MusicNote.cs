using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicNote : MonoBehaviour
{
    [SerializeField] private Vector3 center = Vector3.zero;
    [SerializeField] private SoundBarController soundBarController;

    private float SPEED_MULTIPLIER = 100f;
    private float DURATION = 0.01f;

    private float prevLoudness = 0f;

    void Start() {

    }

    void Update(){
        // Spin the object around the target with speed depending on clip loudness.
        RotateWithLoudness();
    }

    private void RotateWithLoudness(){
        float elapsed = 0.0f;
        float currentLoudness = soundBarController.GetClipLoudness();
        while (elapsed < DURATION){
            float smoothLoudness = Mathf.Lerp(prevLoudness, currentLoudness, elapsed / DURATION);
            elapsed += Time.deltaTime;
            transform.RotateAround(center, Vector3.up, SPEED_MULTIPLIER * smoothLoudness * Time.deltaTime);
        }
        prevLoudness = currentLoudness;
    }
}