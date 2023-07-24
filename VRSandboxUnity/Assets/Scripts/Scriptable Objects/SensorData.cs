using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Sensor Data")]
public class SensorData : ScriptableObject
{
    public enum SensorTypes
    {
        temperature,
        humidity,
        pressure,
        motion
    }


    /// <summary>
    /// Type of this sensor
    /// </summary>
    public SensorTypes SensorType;
    /// <summary>
    /// Unique Identifier of this sensor
    /// </summary>
    public string SensorName;

    public int numValuesStored = 10;

    public ObservableList<string> SensorDataValues = new ObservableList<string>();

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

    public void SubscribeListeners()
    {
        OnDataListUpdated += TrimSensorData;
        OnDataValueChanged += delegate (int x) { TrimSensorData(); };
    }

    private void TrimSensorData()
    {
        Debug.LogWarning("Trim Sensor Data");
        if (SensorDataValues.Count > numValuesStored)
        {
            SensorDataValues.RemoveRange(0, SensorDataValues.Count - numValuesStored);
        }
    }

    private void OnValidate()
    {
        TrimSensorData();
    }

    private void OnDisable()
    {
        SensorDataValues.Clear();
    }
}
