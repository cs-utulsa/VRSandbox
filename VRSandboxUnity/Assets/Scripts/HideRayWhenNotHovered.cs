using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction;
using UnityEngine.XR.Interaction.Toolkit;

public class HideRayWhenNotHovered : MonoBehaviour
{
    public LineRenderer line;
    public XRInteractorLineVisual lineVisual;
    public XRRayInteractor interactor;

    private bool showRay = true;

    // Start is called before the first frame update
    void Start()
    {
        line.enabled = showRay;
    }

    // Update is called once per frame
    void Update()
    {
        bool temp = showRay;

        showRay = interactor.interactablesHovered.Count > 0;

        if(showRay != temp)
        {
            Debug.LogWarning($"Show Ray: {showRay}\nInteractables Hovered: {interactor.interactablesHovered.Count}");
        }

        lineVisual.enabled = showRay;
        line.enabled = showRay;
    }
}
