using System;
using System.Collections.Generic;


[Serializable]
public struct AnalyticsData
{
	public Guid sessionID;
	public SerializableDictionary<string, List<PositionTracker.StepData>> tracks;


	public void addTrackStep(string key, PositionTracker.StepData step)
	{
		tracks ??= new SerializableDictionary<string, List<PositionTracker.StepData>>();
		
		if (!tracks.ContainsKey(key))
		{
			tracks.Add(key, new List<PositionTracker.StepData>());
		}

		tracks[key].Add(step);
	}
}
