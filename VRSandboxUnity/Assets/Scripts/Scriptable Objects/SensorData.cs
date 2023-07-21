using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Sensor Data")]
public class SensorData : ScriptableObject
{
    /// <summary>
    /// Type of this sensor
    /// </summary>
    public string SensorType;
    /// <summary>
    /// Unique Identifier of this sensor
    /// </summary>
    public string SensorName;

    public int numValuesStored = 10;

    public ObservableList<string> SensorDataValues;

    //Wrappers for list events
    public event Action OnDataListUpdated
    {
        add
        {
            SensorDataValues.ListUpdated += value;
        }

        remove
        {
            SensorDataValues.ListUpdated -= value;
        }
    }
    public event Action<int> OnDataValueChanged
    {
        add
        {
            SensorDataValues.ElementChanged += value;
        }

        remove
        {
            SensorDataValues.ElementChanged -= value;
        }
    }

    private void TrimSensorData()
    {
        if (SensorDataValues.Count > numValuesStored)
        {
            SensorDataValues.RemoveRange(0, SensorDataValues.Count - numValuesStored);
        }
    }

    private void OnValidate()
    {
        TrimSensorData();
    }
}
