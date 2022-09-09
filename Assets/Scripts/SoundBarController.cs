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
    [SerializeField] private int _samplesAmount = 450;
    [SerializeField] private float _lerpSpeed = 10F;
    [SerializeField] private float[] _spectrum;

    [SerializeField] private float currentUpdateTime = 0f;
    [SerializeField] private float updateStep = 0.05f;
    [SerializeField] private int sampleDataLength = 1024;
    [SerializeField] private float clipLoudness;
    [SerializeField] private float[] clipSampleData;


    void Start()
    {
        _audioSource = gameObject.GetComponent<AudioSource>();
        _cubes = new GameObject[_samplesAmount];
        _spectrum = new float [16 * _samplesAmount];
        clipSampleData = new float[sampleDataLength];

        int squaresCount = _samplesAmount;
        int distance = 32;
        int multi = 1;

        for (int i = 0; i < _samplesAmount; i++) {
            //Vector3 p = new Vector3(transform.localPosition.x + System.Math.Abs(distance), transform.localPosition.y, (-_samplesAmount / 2) + i - .0001F);

           

            float theta = (float) (i * 2 * System.Math.PI / _samplesAmount);
            float xValue = (float) (System.Math.Sin(theta) * 10);
            float zValue = (float) (System.Math.Cos(theta) * 10);
            Vector3 p = new Vector3(transform.localPosition.x + xValue, transform.localPosition.y, transform.localPosition.z + zValue);
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = p;
            cube.transform.parent = transform;
            cube.transform.localScale -= new Vector3(0, 0, 0.2F);
            Vector3 normal = (p - transform.position).normalized;
            cube.transform.LookAt(normal, Vector3.up);
            cube.GetComponent<Renderer>().material = _barMaterial;
            _cubes[i] = cube;
            distance--;
        }
    }

    void Update()
    {
        if (_audioSource.isPlaying) {
            _audioSource.GetSpectrumData(_spectrum, 0, FFTWindow.Hamming);

            // Work with music bars
            for (int i = 0; i < _samplesAmount; i++) {
                Vector3 cubeLocalScale = _cubes[i].transform.localScale;
                Vector3 newScale = new Vector3(cubeLocalScale.x, GetFunctionValueForSample(_spectrum[i]), cubeLocalScale.z);
                _cubes[i].transform.localScale = Vector3.Lerp(cubeLocalScale, newScale, _lerpSpeed * Time.deltaTime);
            }

            // Update clip loudness for music note speed
            currentUpdateTime += Time.deltaTime;
            if (currentUpdateTime >= updateStep) {
                currentUpdateTime = 0f;
                _audioSource.clip.GetData(clipSampleData, _audioSource.timeSamples); //I read 1024 samples, which is about 80 ms on a 44khz stereo clip, beginning at the current sample position of the clip.
                clipLoudness = 0f;
                foreach (var sample in clipSampleData) {
                    clipLoudness += Mathf.Abs(sample);
                }
                clipLoudness /= sampleDataLength; //clipLoudness is what you are looking for
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
        clipLoudness = 0f;
    }


    public void ChangeSong(int index){
        if (index < songs.Length && index >= 0) {
            _audioSource.Stop();
            clipLoudness = 0f;
            _audioSource.clip = songs[index];
            _audioSource.Play();
            foreach (GameObject cube in _cubes) cube.SetActive(true);
        }
    }


    public float GetClipLoudness() {
        return clipLoudness;
    }
}
