using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Sensor : MonoBehaviour
{
    private Room _owningRoom;

    public void InitializeSensor(Room owner)
    {
        _owningRoom = owner;
        InitializeSensorOverride();
    }

    protected abstract void InitializeSensorOverride();

    /// <summary>
    /// Starts displaying the data from this sensor in a visual format
    /// </summary>
    public abstract void StartDisplayVisual();

    /// <summary>
    /// Stops displaying the data from this sensor in a visual format
    /// </summary>
    public abstract void EndDisplayVisual();

    /// <summary>
    /// Gets the text representation of the data from this sensor
    /// </summary>
    /// <returns></returns>
    public virtual string GetTextReadout()
    {
        return null;
    }
}
