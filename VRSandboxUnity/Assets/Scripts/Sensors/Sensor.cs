using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MqttReceiver
{
    [Header("Sensor Data")]
    public SensorData SensorDataObject;

    /// <summary>
    /// Handle used to subscribe to this sensor
    /// </summary>
    public string SensorHandle
    {
        get
        {
            return $"{SensorDataObject.SensorType}/{SensorDataObject.SensorName}";
        }
    }

    protected override void Awake()
    {
        base.Awake();
        topicsToSubscribe.Add(SensorHandle);
        OnMessageArrived += ProcessSensorMessage;
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected virtual void ProcessSensorMessage(string message)
    {
        SensorDataObject.SensorDataValues.Add(message);
    }

    /// <summary>
    /// Initializes the Sensor
    /// </summary>
    public virtual void InitializeSensor() {}
}
