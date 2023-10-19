using UnityEngine;

public class HeatmapManager : MonoBehaviour
{
    public GameObject[] heatmaps;  // Array to hold references to all heatmap GameObjects

    // Method to toggle the active state of all heatmap GameObjects
    public void ToggleHeatmaps()
    {
        foreach (GameObject heatmap in heatmaps)
        {
            heatmap.SetActive(!heatmap.activeSelf);  // Toggle active state
        }
    }
}
