using UnityEngine;
using System;

namespace HeatmapVisualization
{
    public class HeatmapDataController : MonoBehaviour
    {
        public TextAsset csvAsset; // Drag and drop your CSV asset here in the inspector
        public HeatmapDataManagerCSV dataManager;

        // Define an event to notify other controllers
        public static event Action OnDataBroadcast;

        private void Start()
        {
            dataManager = HeatmapDataManagerCSV.Instance; // Access the singleton instance
            if (dataManager == null)
            {
                Debug.LogError("HeatmapDataManagerCSV instance not found!");
                return;
            }

            dataManager.csvAsset = csvAsset; // Set the csvAsset for the dataManager
            InvokeRepeating("BroadcastData", 0f, 1f / 3f); // Start broadcasting at 30fps
        }

        private void BroadcastData()
        {
            dataManager.MoveToNextRow();
            OnDataBroadcast?.Invoke(); // Notify all subscribed heatmap controllers to update
        }
    }
}
