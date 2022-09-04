using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundBarController : MonoBehaviour
{
    public AudioSource SetAudioSource => _audioSource; 
    [SerializeField] private AudioSource _audioSource;

    [SerializeField] private AudioClip[] songs;

    public Material SetBarMaterial => _barMaterial;
    [SerializeField] private Material _barMaterial;

    [SerializeField] private GameObject[] _cubes;
    [SerializeField] private int _samplesAmount = 64;
    [SerializeField] private float _lerpSpeed = 10F;
    [SerializeField] private float[] _spectrum;
    

    void Start()
    {
        _audioSource = gameObject.GetComponent<AudioSource>();
        _cubes = new GameObject[_samplesAmount];
        _spectrum = new float [16 * _samplesAmount];

        for (int i = 0; i < _samplesAmount; i++) {
            Vector3 p = new Vector3(transform.localPosition.x, transform.localPosition.y, (-_samplesAmount / 2) + i - .0001F);
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = p;
            cube.transform.parent = transform;
            cube.transform.localScale -= new Vector3(0, 0, 0.2F); 
            cube.GetComponent<Renderer>().material = _barMaterial;
            _cubes[i] = cube;
        }
    }

    void Update()
    {
        if (_audioSource.isPlaying) {
            _audioSource.GetSpectrumData(_spectrum, 0, FFTWindow.Hamming);

            for (int i = 0; i < _samplesAmount; i++) {
                Vector3 cubeLocalScale = _cubes[i].transform.localScale;
                Vector3 newScale = new Vector3(cubeLocalScale.x, GetFunctionValueForSample(_spectrum[i]), cubeLocalScale.z);
                _cubes[i].transform.localScale = Vector3.Lerp(cubeLocalScale, newScale, _lerpSpeed * Time.deltaTime);
            }
        } else {
            foreach (GameObject cube in _cubes) cube.SetActive(false);
        }
    }

    private float GetFunctionValueForSample(float sample) {
        return 50 * Mathf.Pow(Mathf.Abs(sample), 0.2F);
    }


    public void Stop(){
        foreach (GameObject cube in _cubes) cube.SetActive(false);
        _audioSource.Stop();
    }


    public void ChangeSong(int index){
        print("\n\n\nCHANGESONG\n\n\n");
        if (index < songs.Length && index >= 0) {
            _audioSource.Stop();
            _audioSource.clip = songs[index];
            _audioSource.Play();
            foreach (GameObject cube in _cubes) cube.SetActive(true);
        }
    }

}
