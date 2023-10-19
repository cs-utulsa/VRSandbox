using System;
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
            /*return $"{Enum.GetName(typeof(SensorData.SensorTypes), SensorDataObject.SensorType)}/{SensorDataObject.SensorName}";*/
            return $"{SensorDataObject.SensorType}/{SensorDataObject.SensorName}";

        }
    }

    protected override void Awake()
    {
        base.Awake();
        Debug.Log(SensorHandle);
        topicsToSubscribe.Add(SensorHandle);
        SensorDataObject.SubscribeListeners();
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
        Debug.Log(message);
        SensorDataObject.SensorDataValues.Add(message);
    }

    /// <summary>
    /// Initializes the Sensor
    /// </summary>
    public virtual void InitializeSensor() {}
}
