using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace HeatmapVisualization
{
    public class HeatmapFullManager : MonoBehaviour
    {
        public TextAsset csvAsset;

        #region Globals
        public Heatmap ownHeatmap;
        private Heatmap OwnHeatmap { get { if (ownHeatmap == null) { ownHeatmap = GetComponent<Heatmap>(); } return ownHeatmap; } }
        #endregion

        public List<Heatmap> nodeHeatmaps; // List of other heatmap objects

        private HeatmapDataManagerCSV dataManager;

        private void Start()
        {
            dataManager = HeatmapDataManagerCSV.Instance; // Access the singleton instance
            if (dataManager == null)
            {
                Debug.LogError("HeatmapDataManagerCSV instance not found!");
                return;
            }

            dataManager.csvAsset = csvAsset; // Set the csvAsset for the dataManager
            StartCoroutine(UpdateHeatmaps());
        }

        private IEnumerator UpdateHeatmaps()
        {
            while (true)
            {
                // Update overall heatmap
                float overallSum = dataManager.GetSumOfRow();
                Bounds overallBounds = OwnHeatmap.BoundsFromTransform; // Use the property here
                Vector3 overallMidpoint = overallBounds.center;
                OwnHeatmap.GenerateHeatmap(new List<Vector3> { overallMidpoint }, overallSum); // Use the property here

                yield return new WaitForSeconds(3);

                // Update individual node heatmaps
                for (int i = 0; i < nodeHeatmaps.Count; i++)
                {
                    float nodeValue = dataManager.GetNodeValue(i);
                    Bounds nodeBounds = nodeHeatmaps[i].BoundsFromTransform;
                    Vector3 nodePosition = nodeBounds.center;
                    nodeHeatmaps[i].GenerateHeatmap(new List<Vector3> { nodePosition }, nodeValue);

                    yield return new WaitForSeconds(1);
                }

                dataManager.MoveToNextRow();
                yield return new WaitForSeconds(3);
            }
        }
    }
}
