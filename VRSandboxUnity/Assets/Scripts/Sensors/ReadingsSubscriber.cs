using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

/// <summary>
/// This class is used to subscribe to all readings sent through the MQTT broker.
/// It can be used to update the SensorData objects with the latest readings.
/// </summary>
public class ReadingsSubscriber : MqttReceiver
{

    // List of all SensorData objects in the scene.
    // This list is populated by the SensorInstance script that holds the actual SensorData objects.
    public List<SensorData> sensorDataObjects = new List<SensorData>(); 

    protected override void Awake()
    {
        base.Awake();
        topicsToSubscribe.Add("readings/#"); // Subscribe to all readings
        OnMessageArrived += ProcessSensorMessage; // Call the ProcessSensorMessage method when a message is received
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }



/// <summary>
/// Processes the Sensor Message and updates the corresponding SensorData object.
/// </summary>
/// <param name="message"></param>
    protected virtual void ProcessSensorMessage(string message) {
        // Message format -> readings/3WBK: {"readings":[{"reading_type":"CO2","reading_unit":"ppm","reading_val":"500.00"}]}'
        
        // Topic w/ Device ID is stored in the message, for some reason.
        // TODO: This should just be stored in SensorReadings object.
        String deviceID = message.Substring(message.IndexOf('/') + 1, message.IndexOf(':') - message.IndexOf('/')-1);

        SensorReadings readingsSerializable = SensorReadings.CreateFromJSON(message); // Convert the JSON string into a SensorReadings object (See Sensor.cs)


        // Notably, this ignores the SensorType of the SensorData object.

        foreach(var reading in readingsSerializable.readings) { // For each sensor reading in the message
            foreach(var sensorDataObject in sensorDataObjects) { // Find the corresponding SensorData object
                if(deviceID == sensorDataObject.SensorName) { 

                    String sensorDisplayValue = $"{reading.reading_type}: {reading.reading_val} {reading.reading_unit}"; // e.g. "temperature: 32.5 celsius
                    sensorDataObject.SensorDataValues.Add(sensorDisplayValue); // Add the reading value to the data object
                
                }
            }
        }


    }


}







// TODO: This script should hold the SensorReadings class for Json Analysis.
// However, the Sensor.cs script currently defines the class.
// This should be moved here if Sensor.cs is depreciated.




// /// <summary>
// /// Object that holds the incoming sensor data from the MQTT network.
// /// Parses the JSON string into a list of SensorReading objects.
// /// </summary>

// [System.Serializable]
// public class SensorReadings {
//     public List<SensorReading> readings {get; set;} // Represents all of the readings from the received JSON string.


// /// <summary>
// /// Class that holds a singular sensor reading from the device. Portion of the SensorReadings object.
// /// </summary>

//     [System.Serializable]
//     public class SensorReading {
//         public string reading_type; // e.g. "temperature"
//         public string reading_unit; // e.g. "celsius"
//         public string reading_val; // e.g. "32.5"
//     }


//     /// <summary>
//     /// Parses the JSON string into a SensorReadings object.
//     /// </summary>
//     /// <param name="jsonString"> Received message of sensor readings, formatted as a JSON string. </param>
//     public static SensorReadings CreateFromJSON(string jsonString)
//     {
//             jsonString = jsonString.Substring(jsonString.IndexOf(": ") + 2); // Remove topic from message string
//             //Debug.Log("JSON String: " + jsonString);

//             return JsonConvert.DeserializeObject<SensorReadings>(jsonString); // Returns a SensorReadings object from the JSON string.
//     }
// }
