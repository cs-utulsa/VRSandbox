using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Transformers;

public class FloorViewController : MonoBehaviour
{
    public enum FloorViewModes
    {
        Interact,
        Move,
        Rotate,
        Scale
    }

    private FloorViewModes _currentMode;
    private XRGrabInteractable _grabInteractable;
    private XRGeneralGrabTransformer _grabTransformer;
    private Collider _mapCollider;
    private bool isGrabbed = false;
    private Quaternion initialRotation;
    private float previousAngle;
    private Vector3 initialScale;
    private float initialDistance;

    private void Awake()
    {
        _grabInteractable = GetComponent<XRGrabInteractable>();
        _grabTransformer = GetComponent<XRGeneralGrabTransformer>();
        _mapCollider = GetComponent<Collider>();
    }

    // Start is called before the first frame update
    void Start()
    {
        SetMode((int)FloorViewModes.Interact);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetMode(int newMode)
    {
        switch (_currentMode)
        {
            case FloorViewModes.Interact:
                _mapCollider.enabled = true;
                break;
            case FloorViewModes.Rotate:
                _grabInteractable.trackRotation = false;
                break;
            case FloorViewModes.Move:
                _grabInteractable.trackPosition = false;
                break;
            case FloorViewModes.Scale:
                _grabTransformer.allowTwoHandedScaling = false;
                break;
        }

        _currentMode = (FloorViewModes)newMode;

        switch (_currentMode)
        {
            case FloorViewModes.Interact:
                _mapCollider.enabled = false;
                break;
            case FloorViewModes.Rotate:
                _grabInteractable.trackRotation = true;
                break;
            case FloorViewModes.Move:
                _grabInteractable.trackPosition = true;
                break;
            case FloorViewModes.Scale:
                _grabTransformer.allowTwoHandedScaling = true;
                break;
        }
    }
}
