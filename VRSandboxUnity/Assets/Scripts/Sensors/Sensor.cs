using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MqttReceiver
{
    /// <summary>
    /// Type of this sensor
    /// </summary>
    public string SensorType;
    /// <summary>
    /// Unique Identifier of this sensor
    /// </summary>
    public string SensorName;

    /// <summary>
    /// Handle used to subscribe to this sensor
    /// </summary>
    public string SensorHandle
    {
        get
        {
            return $"{SensorType}/{SensorName}";
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

    protected virtual void ProcessSensorMessage(string message) {}

    /// <summary>
    /// Initializes the Sensor
    /// </summary>
    public virtual void InitializeSensor() {}
}
