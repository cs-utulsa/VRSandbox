using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OccupancySensor : Sensor
{
    public int currentOccupancy = 0;
    public int maxOccupancy;

    public GameObject OccupantVisualPrefab;

    public float OccupantShowTime;
    public float OccupantHideTime;
    public float OccupantFadeTime;

    protected override void InitializeSensorOverride()
    {
        
    }


    public override void StartDisplayVisual()
    {
        StartCoroutine(DisplayOccupants());
    }

    public override void EndDisplayVisual()
    {
        
    }

    private IEnumerator DisplayOccupants()
    {
        yield return new WaitForSeconds(OccupantShowTime);
        yield return new WaitForSeconds(OccupantHideTime);
    }

    public override string GetTextReadout()
    {
        return $"Occupancy: {currentOccupancy}/{maxOccupancy}";
    }

    public void OnValidate()
    {
        currentOccupancy = Mathf.Clamp(currentOccupancy, 0, int.MaxValue);
        maxOccupancy = Mathf.Clamp(maxOccupancy, 0, int.MaxValue);
    }
}
