/**
* @file GetSensorData.cs
* @brief Pulls Sensor Device information from the Sensors' API and uses it to instantiate the sensors in the Unity scene.
*
*/
    

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking;
using Newtonsoft.Json;



public class GetSensorData : MonoBehaviour
{

    public string serverIP;
    public int serverPort;

    // Start is called before the first frame update
    void Start()
    {
        requestSensorData("building", "room", "zone");
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    /**
    * @brief Requests sensor data from the Sensors' API.
    * @param building The building the target sensors are in.
    * @param room The room the target sensors are in.
    * @param zone The zone the target sensors are in.
    * @return void
    */
    void requestSensorData(string building, string room, string zone="all") {

        string server = $"http://{serverIP}:{serverPort}/api/boards";
        // string server = $"http://{serverIP}:{serverPort}/api/boards?building={building}&room={room}&zone={zone}";
        StartCoroutine(GetRequest(server));
    }


    IEnumerator GetRequest(string uri) {

        using (UnityWebRequest request = UnityWebRequest.Get(uri)) {

            // Set headers to expect JSON
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Accept", "application/json");

            // Set query params
            // request.SetRequestHeader("building", building);
            // request.SetRequestHeader("room", room);
            // request.SetRequestHeader("zone", zone);


            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError) {
                Debug.Log(request.error);
            }
            // if(request.Result.ProtocolError)
            // {
            //     Debug.Log(request.error);
            // }
            else {
                // Show results as text
                Debug.Log(request.downloadHandler.text);

                // Or retrieve results as binary data
                byte[] results = request.downloadHandler.data;
            }
        }

    }

}





/// <summary>
/// Object that holds the Device information obtained from the Sensors' API.
/// </summary>

/*
{
    "T35T": {
            "device_type" : 'Arduino',
            "building": 'library',
            "room": '1000',
            "zone": 'A7'
    }, ...
}*/

[System.Serializable]
public class DeviceData {
    public Dictionary<string, DeviceObject> devices {get; set;} // Represents all of the readings from the received JSON string.

[System.Serializable]
public class DeviceObject {
    public string device_type {get; set;}
    public string building {get; set;}
    public string room {get; set;}
    public string zone {get; set;}
}

    /// <summary>
    /// Parses the JSON string into a DeviceData object.
    /// </summary>
    /// <param name="jsonString"> Received message of sensor readings, formatted as a JSON string. </param>
    public static DeviceData CreateFromJSON(string jsonString)
    {
        Debug.Log("JSON String: " + jsonString);
        return JsonConvert.DeserializeObject<DeviceData>(jsonString); // Returns a SensorReadings object from the JSON string.
    }

}