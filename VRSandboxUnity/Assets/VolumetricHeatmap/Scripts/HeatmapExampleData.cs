using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeatmapVisualization
{
	public class HeatmapExampleData : MonoBehaviour
	{
		#region Settings
		[SerializeField]
		private int examplePointsAmount = 100;
		#endregion

		#region Globals
		private Heatmap ownHeatmap;
		private Heatmap OwnHeatmap { get { if (ownHeatmap == null) { ownHeatmap = GetComponent<Heatmap>(); } return ownHeatmap; } }
		#endregion

		#region Functions
		public void GenerateExampleHeatmap()
		{
			// Generate random points and density
			(List<Vector3> points, float density) = GetRandomPointsAndDensities(examplePointsAmount, OwnHeatmap.BoundsFromTransform);

			// Call the GenerateHeatmap method in the Heatmap class
			OwnHeatmap.GenerateHeatmap(points, density);
		}

		private (List<Vector3>, float) GetRandomPointsAndDensities(int amount, Bounds bounds)
		{
			List<Vector3> points = new List<Vector3>();

			// Calculate the center of the bounds
			Vector3 centerPoint = bounds.center;

			// Add the center point to the points list
			points.Add(centerPoint);

			// Generate a random density value or set a specific density value
			/*float densityValue = Random.Range(0f, 1f);*/
			float densityValue = 0.77f;  // Or set a specific density value

			return (points, densityValue);
		}

		#endregion
	}
}
