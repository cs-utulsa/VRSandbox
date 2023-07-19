using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemperatureSensor : Sensor
{
    public string TemperatureReading
    {
        get
        {
            return $"{_tempValue} {_tempUnit}";
        }
    }

    public string TempUnit
    {
        get
        {
            return _tempUnit;
        }

        set
        {
            _tempUnit = value;
        }
    }

    public float TempValue
    {
        get
        {
            return _tempValue;
        }

        set
        {
            _tempValue = value;
        }
    }

    private string _tempUnit;
    private float _tempValue;

    protected override void ProcessSensorMessage(string message)
    {
        base.ProcessSensorMessage(message);
    }
}
