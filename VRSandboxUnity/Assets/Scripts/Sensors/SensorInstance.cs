using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorInstance : MonoBehaviour
{
    public SensorData sensorData;
    //public ReadingsSubscriber mqttSubscriber;

    // Start is called before the first frame update
    void Start()
    {
        sensorData.SubscribeListeners();

        GameObject MqttCube = GameObject.Find("The MQTT Cube"); // Find the MQTT Cube in the scene
        ReadingsSubscriber mqttSubscriber = MqttCube.GetComponent<ReadingsSubscriber>(); // Get the readings subscriber component from the MQTT Cube
        
        mqttSubscriber.sensorDataObjects.Add(sensorData); // Add this sensor data object to the list of sensor data objects in the readings subscriber


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
