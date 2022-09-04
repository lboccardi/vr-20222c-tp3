using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LaserPointer : MonoBehaviour
{
    private Camera cam;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private SoundBarController soundBarController;
    [SerializeField] private InputController inputController;

    private Vector3 devicePosition;
    private int rotationOffset = 30;

    private int UI_LAYER = 5;
    private string STOP_BUTTON = "Stop";

    void Start() {
        cam = Camera.main;
        devicePosition = transform.position;
    }

    void Update(){

        // Update object with device position and rotation
        transform.localPosition = inputController.GetDevicePosition();
        transform.localRotation = inputController.GetDeviceRotation() * Quaternion.AngleAxis(rotationOffset, Vector3.right);

        // Shoot a ray forward from controller
        Ray ray = cam.ScreenPointToRay(transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity)) {
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, hit.point);

            GameObject gameObj = hit.collider.gameObject;
            // If hits a button, change song
            if (gameObj.layer == UI_LAYER && inputController.GetAButtonPressed()) {
                string name = gameObj.name;
                if (name == STOP_BUTTON)
                    soundBarController.Stop();
                else
                    soundBarController.ChangeSong(name[name.Length-1] - '0');
            }
        } else {
            lineRenderer.enabled = false;
        }
    }
}