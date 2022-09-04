using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using System.Linq;

public class InputController : MonoBehaviour
{
    [SerializeField]
    private XRNode xrNode = XRNode.RightHand;
    private InputDevice device;
    private List<InputDevice> devices = new List<InputDevice>();

    void Start()
    {
    }

    void Update()
    {
        if (!device.isValid){
            GetDevice();
        }
    }

    private void GetDevice(){
        InputDevices.GetDevicesAtXRNode(xrNode, devices);
        device = devices.FirstOrDefault();
    }

    private void OnEnable(){
        if (!device.isValid){
            GetDevice();
        }
    }

    // Returns device position
    public Vector3 GetDevicePosition(){
        Vector3 devicePosition = Vector3.zero;
        if (device.isValid){
            device.TryGetFeatureValue(CommonUsages.devicePosition, out devicePosition);
        }
        return devicePosition;
    }

    // Returns device rotation
    public Quaternion GetDeviceRotation(){
        Quaternion deviceRotation = Quaternion.identity;
        if (device.isValid){
            device.TryGetFeatureValue(CommonUsages.deviceRotation, out deviceRotation);
        }
        return deviceRotation;
    }

    // Returns true if A button is pressed
    public bool GetAButtonPressed(){
        bool buttonPressed = false;
        if (device.isValid){
            device.TryGetFeatureValue(CommonUsages.primaryButton, out buttonPressed);
        }
        return buttonPressed;
    }

}