using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace HeatmapVisualization
{
	[CustomEditor(typeof(HeatmapExampleData))]
	public class HeatmapExampleDataEditor : Editor
	{
		//Globals
		private new HeatmapExampleData target;


		//Functions
		private void Awake()
		{
			target = (HeatmapExampleData)base.target;
		}


		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();


			if (GUILayout.Button("Generate Example Heatmap"))
			{
				target.GenerateExampleHeatmap();
			}
		}
	}
}
