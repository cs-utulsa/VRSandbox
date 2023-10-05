using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;


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
            //return $"{Enum.GetName(typeof(SensorData.SensorTypes), SensorDataObject.SensorType)}/{SensorDataObject.SensorName}";
            return $"readings/{SensorDataObject.SensorName}";
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

        // Process JSON String, find the Data Object's sensor type, and add the value to the SensorDataValues list 
        SensorReadings readingsSerializable = SensorReadings.CreateFromJSON(message);


        //SensorDataObject.SensorDataValues.Add(message);

        foreach (var reading in readingsSerializable.readings) {
            if (reading.reading_type == SensorDataObject.SensorType.ToString().ToLower()) {
                SensorDataObject.SensorDataValues.Add(reading.reading_val);
            }
        }

        
    }

    /// <summary>
    /// Initializes the Sensor
    /// </summary>
    public virtual void InitializeSensor() {}
}





/// <summary>
/// Object that holds the incoming sensor data from the MQTT network.
/// Parses the JSON string into a list of SensorReading objects.
/// </summary>

[System.Serializable]
public class SensorReadings {
    public List<SensorReading> readings {get; set;} // Represents all of the readings from the received JSON string.


/// <summary>
/// Class that holds a singular sensor reading from the device. Portion of the SensorReadings object.
/// </summary>

    [System.Serializable]
    public class SensorReading {
        public string reading_type; // e.g. "temperature"
        public string reading_unit; // e.g. "celsius"
        public string reading_val; // e.g. "32.5"
    }


    /// <summary>
    /// Parses the JSON string into a SensorReadings object.
    /// </summary>
    /// <param name="jsonString"> Received message of sensor readings, formatted as a JSON string. </param>
    public static SensorReadings CreateFromJSON(string jsonString)
    {
        //try {
            //jsonString = jsonString.Replace("\"", "'");
            //jsonString = jsonString.Trim(new char[]{'\uFEFF', '\u200B'});
            //jsonString = jsonString.Trim();

            // "readings/G1XE: "

            // Substring from first ": " to end of string
            jsonString = jsonString.Substring(jsonString.IndexOf(": ") + 2);
            Debug.Log("JSON String: " + jsonString);
            return JsonConvert.DeserializeObject<SensorReadings>(jsonString);
            //return JsonUtility.FromJson<SensorReadings>(jsonString); // Returns a SensorReadings object from the JSON string.
        //} catch(Exception e) {
        //    Debug.Log($"Error with CreateFromJSON: {e.Message}");
        //    return null;
        //}
    }
}
