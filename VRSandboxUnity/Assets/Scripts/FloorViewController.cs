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

    private void RotateModeUpdate()
    {
        if (_grabInteractable.interactorsSelecting.Count == 2)
        {
            // Object is grabbed with both hands
            if (!isGrabbed)
            {
                // Store the initial rotation of the object
                initialRotation = transform.rotation;
                isGrabbed = true;
            }

            float currentAngle = GetInteractorsAngle();

            if (previousAngle != 0f)
            {
                // Calculate the delta angle and rotate the object accordingly
                float deltaAngle = currentAngle - previousAngle;
                transform.rotation = Quaternion.AngleAxis(deltaAngle, Vector3.forward) * transform.rotation;
            }

            previousAngle = currentAngle;
        }
        else if (isGrabbed)
        {
            // Object is released or not grabbed with both hands
            isGrabbed = false;
            previousAngle = 0f;
        }
    }

    private void ScaleModeUpdate()
    {
        if (_grabInteractable.interactorsSelecting.Count == 2)
        {
            // Object is grabbed with both hands
            if (!isGrabbed)
            {
                // Store the initial scale and distance between the hands
                initialScale = transform.localScale;
                initialDistance = GetInteractorsDistance();
                isGrabbed = true;
            }

            // Get the current distance between the hands
            float currentDistance = GetInteractorsDistance();

            // Calculate the scale factor based on the initial and current distances
            float scaleFactor = currentDistance / initialDistance;

            // Apply the scale factor to the initial scale
            transform.localScale = initialScale * scaleFactor;
        }
        else if (isGrabbed)
        {
            // Object is released or not grabbed with both hands
            isGrabbed = false;
            transform.localScale = initialScale;
        }
    }

    private float GetInteractorsDistance()
    {
        var interactor1 = _grabInteractable.interactorsSelecting[0];
        var interactor2 = _grabInteractable.interactorsSelecting[1];
        Vector3 interactor1Position = interactor1.transform.position;
        Vector3 interactor2Position = interactor2.transform.position;

        return Vector3.Distance(interactor1Position, interactor2Position);
    }

    private float GetInteractorsAngle()
    {
        // Get the interactor positions and calculate the twist angle
        var interactor1 = _grabInteractable.interactorsSelecting[0];
        var interactor2 = _grabInteractable.interactorsSelecting[1];
        Vector3 interactor1Position = interactor1.transform.position;
        Vector3 interactor2Position = interactor2.transform.position;
        Vector3 interactorDirection = interactor2Position - interactor1Position;
        return Mathf.Atan2(interactorDirection.y, interactorDirection.x) * Mathf.Rad2Deg;
    }
}
