using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Tab : MonoBehaviour
{
    public event Action<bool> OnTabStateChanged;

    public GameObject TabWindow;
    public GameObject CubeObject; // Reference to the cube object

    private TextMeshProUGUI _text;
    private Renderer _cubeRenderer; // Reference to the cube's renderer

    private void Start()
    {
        _text = GetComponentInChildren<TextMeshProUGUI>();
        if (CubeObject != null)
        {
            _cubeRenderer = CubeObject.GetComponent<Renderer>();
        }
    }

    public void ActivateTab()
    {
        TabWindow.SetActive(true);
        ChangeCubeColor(Color.green); // Change the cube's color to green when the tab is activated
        OnTabStateChanged?.Invoke(true);

        Debug.Log(_text.text);
    }

    public void DeactivateTab()
    {
        TabWindow.SetActive(false);
        ChangeCubeColor(Color.red); // Reset the cube's color to red when the tab is deactivated
        OnTabStateChanged?.Invoke(false);
    }

    private void ChangeCubeColor(Color newColor)
    {
        if (_cubeRenderer != null)
        {
            _cubeRenderer.material.color = newColor;
        }
    }

    public void SetText(string newText)
    {
        _text.text = newText;
    }

    public void SetWindow(GameObject newWindow)
    {
        TabWindow = newWindow;
    }

    public void OnDestroy()
    {
        Destroy(TabWindow);
        GetComponent<Button>().onClick.RemoveAllListeners();
    }
}
