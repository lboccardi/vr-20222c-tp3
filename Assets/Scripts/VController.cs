using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class VController : MonoBehaviour
{
    public AudioSource SetAudioSource => _audioSource; 
    [SerializeField] private AudioSource _audioSource;
    
    [SerializeField] private GameObject[] _cubes;
    
    [SerializeField] private int _samplesAmount = 64;
    [SerializeField] private float _lerpSpeed = 10F;
    [SerializeField] private float _zOrigin = -32F;
    [SerializeField] private float[] _spectrum;
    

    void Start()
    {
        _audioSource = gameObject.GetComponent<AudioSource>();
        _cubes = new GameObject[_samplesAmount];
        _spectrum = new float [16 * _samplesAmount];

        for (int i = 0; i < _samplesAmount; i++) {
            Vector3 p = new Vector3(transform.position.x, transform.position.y, _zOrigin + i - .0001F);
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = p;
            cube.transform.parent = transform;
            _cubes[i] = cube;
        }
    }

    void Update()
    {
        if (_audioSource.isPlaying) {
            _audioSource.GetSpectrumData(_spectrum, 0, FFTWindow.Hamming);

            for (int i = 0; i < _samplesAmount; i++) {
                Vector3 newScale = new Vector3(1, GetFunctionValueForSample(_spectrum[i]), 1);
                _cubes[i].transform.localScale = Vector3.Lerp(_cubes[i].transform.localScale, newScale, _lerpSpeed * Time.deltaTime);
            }
        } else {
            foreach (GameObject cube in _cubes) cube.SetActive(false);
        }
    }

    private float GetFunctionValueForSample(float sample) {
        return 50 * Mathf.Pow(Mathf.Abs(sample), 0.2F);
    }
}
