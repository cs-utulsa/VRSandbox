using UnityEngine;

public class ToggleGameObject : MonoBehaviour
{
    public GameObject heatmap;  // Drag the GameObject you want to toggle in the inspector

    public void ToggleActiveState()
    {
        if (heatmap != null)
        {
            heatmap.SetActive(!heatmap.activeSelf);  // Toggle active state
        }
        else
        {
            Debug.LogWarning("Target GameObject is not assigned.");
        }
    }
}
