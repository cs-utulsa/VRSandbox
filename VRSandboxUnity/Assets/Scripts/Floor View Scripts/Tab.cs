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
    private TextMeshProUGUI _text;

    private void Start()
    {
        _text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void ActivateTab()
    {
        TabWindow.SetActive(true);

        OnTabStateChanged?.Invoke(true);
    }

    public void DeactivateTab()
    {
        TabWindow.SetActive(false);
        OnTabStateChanged?.Invoke(false);
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
