using UnityEngine;
using System.Collections.Generic;

namespace HeatmapVisualization
{
    public class HeatmapOccupancyController : MonoBehaviour
    {
        #region Globals
        public Heatmap ownHeatmap;
        private Heatmap OwnHeatmap
        {
            get
            {
                if (ownHeatmap == null)
                {
                    ownHeatmap = GetComponent<Heatmap>();
                }
                return ownHeatmap;
            }
        }
        #endregion

        private HeatmapDataManagerCSV dataManager;

        private float previousSum = -1f; // Store the previous sum value
        private const float densityThreshold = 1.8f; // Threshold for significant change in density

        private void Awake()
        {
            dataManager = HeatmapDataManagerCSV.Instance; // Access the singleton instance
            if (dataManager == null)
            {
                Debug.LogError("HeatmapDataManagerCSV instance not found!");
                return;
            }
        }

        private void OnEnable()
        {
            HeatmapDataController.OnDataBroadcast += UpdateHeatmapForOccupancy;
        }

        private void OnDisable()
        {
            HeatmapDataController.OnDataBroadcast -= UpdateHeatmapForOccupancy;
        }

        private void UpdateHeatmapForOccupancy()
        {
            if (dataManager == null)
            {
                Debug.LogError("dataManager is null!");
                return;
            }

            if (OwnHeatmap == null)
            {
                Debug.LogError("OwnHeatmap is null!");
                return;
            }

            float sum = dataManager.GetSumOfRow();

            // Check if the change in sum exceeds the threshold
            if (Mathf.Abs(previousSum - sum) < densityThreshold)
            {
                return; // If the change is below the threshold, skip the update
            }

            previousSum = sum; // Update the previous sum value

            Bounds overallBounds = OwnHeatmap.BoundsFromTransform;
            Vector3 overallMidpoint = overallBounds.center;
            OwnHeatmap.GenerateHeatmap(new List<Vector3> { overallMidpoint }, sum);
        }
    }
}
