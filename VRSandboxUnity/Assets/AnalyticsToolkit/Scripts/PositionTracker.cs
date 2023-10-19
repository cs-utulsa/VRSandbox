using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PositionTracker : MonoBehaviour
{
	#region Settings
	[SerializeField]
	[Tooltip("A key to identify different position trackers.")]
	string key = "Unknown";
	[SerializeField]
	[Tooltip("If Tracking is enabled, the position will be saved each step interval.")]
	float stepIntervall = 0.5f;
	[SerializeField]
	[Tooltip("If enabled, the position will be recorded and sent to the server automatically.")]
	bool trackingEnabled = true;
	#endregion

	#region Globals
	private float nextStep;
	#endregion


	#region Defenitions
	public struct StepData
	{
		public Vector3 pos;
	}
	#endregion


	#region Functions
	void Start()
	{
		if (trackingEnabled)
		{
			StartTracking();
		}
	}


	void Update()
	{
		if (trackingEnabled)
		{
			if (Time.time >= nextStep)
			{
				RecordPosition();
				nextStep = Time.time + stepIntervall;
			}
		}
	}


	/// <summary>
	/// Starts the timed Tracking based on the step invervall.
	/// </summary>
	public void StartTracking()
	{
		nextStep = Time.time; //start with a step immediately

		trackingEnabled = true;
	}


	/// <summary>
	/// Stops the timed Tracking.
	/// </summary>
	public void StopTracking()
	{
		trackingEnabled = false;
	}


	/// <summary>
	/// Save the current position
	/// </summary>
	private void RecordPosition()
	{
		AnalyticsManager.Instance.AddTrackStep(key, new StepData
		{
			pos = transform.position
		});
	}
	#endregion
}
