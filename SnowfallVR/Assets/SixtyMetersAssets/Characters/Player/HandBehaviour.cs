using System.Collections;
using System.Collections.Generic;
using SixtyMetersAssets.Characters.Player;
using SixtyMetersAssets.Scripts;
using UnityEngine;

public class HandBehaviour : MonoBehaviour
{
    public HandOrientation handOrientation = HandOrientation.Right;
    
    private OVRGrabber _grabber;
    private OVRInput.RawButton _indexTrigger;

    // Start is called before the first frame update
    void Start()
    {
        _grabber = GetComponent<OVRGrabber>();
        InitControllerScheme();
    }

    private void InitControllerScheme()
    {
        if (handOrientation == HandOrientation.Right)
        {
            _indexTrigger = OVRInput.RawButton.RIndexTrigger;
        }
        else
        {
            _indexTrigger = OVRInput.RawButton.LIndexTrigger;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(_indexTrigger) || Input.GetButtonDown("Fire1"))
        {
            if (_grabber.grabbedObject != null)
            {
                IActionableItem actionableItem = _grabber.grabbedObject.GetComponent<IActionableItem>();
                actionableItem?.TriggerAction();
            }
        }
    }
}