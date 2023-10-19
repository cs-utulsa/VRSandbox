using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneRenderer : MonoBehaviour
{
    public GameObject cube; // Reference to the associated cube
    private Color _originalColor; // Store the original color of the cube
    private static GameObject _currentlyColoredCube; // Reference to the currently colored cube

    private void Start()
    {
        if (cube != null)
        {
            _originalColor = cube.GetComponent<Renderer>().material.color;
        }
    }

    public void ActivateTab()
    {
        // Reset the color of the previously colored cube
        if (_currentlyColoredCube != null)
        {
            _currentlyColoredCube.GetComponent<Renderer>().material.color = _originalColor;
        }

        // Change the color of the associated cube to green
        if (cube != null)
        {
            cube.GetComponent<Renderer>().material.color = Color.green;
            _currentlyColoredCube = cube;
        }
    }

    public void DeactivateTab()
    {
        // Reset the color of the associated cube to its original color
        if (cube != null)
        {
            cube.GetComponent<Renderer>().material.color = _originalColor;
        }
    }
}


