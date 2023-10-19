using UnityEngine;
using System.Collections.Generic;

namespace HeatmapVisualization
{
    public class HeatmapZoneController : MonoBehaviour
    {
        public int nodeIndex; // Index of the node this controller is responsible for

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

        private float previousNodeValue = -1f; // Store the previous node value
        private const float nodeValueThreshold = 0.33f; // Threshold for significant change in node value

        private void Start()
        {
            dataManager = HeatmapDataManagerCSV.Instance; // Access the singleton instance
            if (dataManager == null)
            {
                Debug.LogError("HeatmapDataManagerCSV instance not found!");
                return;
            }
        }

/*        private HeatmapDataManagerCSV DataManager
        {
            get
            {
                if (dataManager == null)
                {
                    dataManager = HeatmapDataManagerCSV.Instance;
                    if (dataManager == null)
                    {
                        Debug.LogError("HeatmapDataManagerCSV instance not found!");
                    }
                }
                return dataManager;
            }
        }*/


        private void OnEnable()
        {
            HeatmapDataController.OnDataBroadcast += UpdateHeatmapForNode;
        }

        private void OnDisable()
        {
            HeatmapDataController.OnDataBroadcast -= UpdateHeatmapForNode;
        }

        private int GetCategory(float value)
        {
            if (value == 0) return 0;
            if (value <= 0.33f) return 1;
            if (value <= 0.66f) return 2;
            return 3; // else
        }


        private void UpdateHeatmapForNode()
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

            float nodeValue = dataManager.GetNodeValue(nodeIndex);
            Debug.Log("Value for node index is: " + nodeIndex + "is: " + nodeValue);

            int previousCategory = GetCategory(previousNodeValue);
            int currentCategory = GetCategory(nodeValue);


            // Check if the change in nodeValue exceeds the threshold
            /*            if (Mathf.Abs(previousNodeValue - nodeValue) < nodeValueThreshold)
                        {
                            return; // If the change is below the threshold, skip the update
                        }
            */
            if (previousCategory != currentCategory)
            {

                previousNodeValue = nodeValue;
                Bounds nodeBounds = OwnHeatmap.BoundsFromTransform;
                Vector3 nodePosition = nodeBounds.center;
                OwnHeatmap.GenerateHeatmap(new List<Vector3> { nodePosition }, nodeValue);

            }




            /*            previousNodeValue = nodeValue;
                        Bounds nodeBounds = OwnHeatmap.BoundsFromTransform;
                        Vector3 nodePosition = nodeBounds.center;
                        OwnHeatmap.GenerateHeatmap(new List<Vector3> { nodePosition }, nodeValue);*/
        }
    }
}
