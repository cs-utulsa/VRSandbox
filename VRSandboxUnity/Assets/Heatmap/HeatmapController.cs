using System.IO;
using System.Globalization;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

namespace HeatmapVisualization
{
    public class HeatmapController : MonoBehaviour
    {
        public string csvFilePath = "C:\\Users\\18dmo\\OneDrive\\Desktop\\Work\\Dr.GambleBIM\\csv\\1actor_50steps_walk.csv"; // Relative path from the Assets folder

        private string[] lines;
        private int currentLineIndex = 1; // Start from 1 to skip the header

        #region Globals
        public Heatmap ownHeatmap;
        private Heatmap OwnHeatmap { get { if (ownHeatmap == null) { ownHeatmap = GetComponent<Heatmap>(); } return ownHeatmap; } }
        #endregion

        private void Start()
        {
            TextAsset csvAsset = Resources.Load<TextAsset>("1actor_50steps_walk");

            if (csvAsset != null)
            {
                lines = csvAsset.text.Split(new char[] { '\n' });
                StartCoroutine(StreamDataFromCSV());
            }
            else
            {
                Debug.LogError("CSV file not found in Resources.");
            }
        }

        private IEnumerator StreamDataFromCSV()
        {
            while (true)
            {
                if (currentLineIndex >= lines.Length)
                {
                    currentLineIndex = 1; // Reset to the first data line to start over
                }

                string[] values = lines[currentLineIndex].Split(',');

                // Generate the overall heatmap based on the sum of the entire row
                float sum = 0;
                foreach (string value in values)
                {
                    sum += float.Parse(value, CultureInfo.InvariantCulture);
                }
                Bounds overallBounds = OwnHeatmap.BoundsFromTransform;
                Vector3 overallMidpoint = overallBounds.center;
                OwnHeatmap.GenerateHeatmap(new List<Vector3> { overallMidpoint }, sum);

                yield return new WaitForSeconds(3); // 3-second pause after generating the overall heatmap

                // Generate individual heatmaps for each node value
                for (int i = 0; i < values.Length; i++)
                {
                    float nodeValue = float.Parse(values[i], CultureInfo.InvariantCulture);
                    Debug.Log(nodeValue);

                    // Get the position for the node
                    // This might need to be adjusted if each node has a different position
                    Bounds nodeBounds = OwnHeatmap.BoundsFromTransform;
                    Vector3 nodePosition = nodeBounds.center; // Adjust this if each node has a different position

                    // Generate the heatmap using the node position and the node value
                    OwnHeatmap.GenerateHeatmap(new List<Vector3> { nodePosition }, nodeValue);

                    yield return new WaitForSeconds(1); // 1-second pause between each node value
                }

                currentLineIndex++;

                yield return new WaitForSeconds(3); // 3-second pause between each row
            }
        }
    }
}
