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



    


//     void getAllZonePositions() {
//         // Look for gameobject with tag ZoneGraph
//         GameObject zoneGraph = GameObject.FindGameObjectWithTag("Zone Graph");

//         // Get all children of ZoneGraph
//         for (int i = 0; i < zoneGraph.transform.childCount; i++) {
//             GameObject child = zoneGraph.transform.GetChild(i).gameObject;

//             zonePositions.Add(child.name, child.transform.position);
//             char letter = child.name[0];

//             for(int j = 0; j < child.transform.childCount; j++) {
//                 GameObject grandchild = child.transform.GetChild(j).gameObject;

//                 string number = grandchild.name;

//                 string zone = letter + number;

//                 zonePositions.Add(zone, grandchild.transform.position);
//             }
            
//     }

//     // Print the zonePositions dictionary
//     foreach (KeyValuePair<string, Vector3> kvp in zonePositions)
//     {
//         Debug.Log($"Key = {kvp.Key}, Value = {kvp.Value}");
//     }
// }


    public string serverIP;
    public int serverPort;
    public GameObject sensorPrefab;

    // Start is called before the first frame update
    void Start()
    {
        requestSensorData("Rayzor", "2055");
        // getAllZonePositions();

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

        // string server = $"http://{serverIP}:{serverPort}/api/data/boards";
        // string server = $"http://{serverIP}:{serverPort}/api/data/boards?building={building}&room={room}&zone={zone}";

        string server = $"http://{serverIP}:{serverPort}/api/data/boards";

        if (zone == "all")
            server += $"?building={building}&room={room}";
        else
            server += $"?building={building}&room={room}&zone={zone}";

        StartCoroutine(GetRequest(server));
    }


    IEnumerator GetRequest(string uri) {

        using (UnityWebRequest request = UnityWebRequest.Get(uri)) {

            // Set headers to expect JSON
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Accept", "application/json");

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
                // decode results into a string
                // string resultsString = System.Text.Encoding.UTF8.GetString(results);
                // Debug.Log(resultsString);

                // Create a DeviceData object from the JSON string
                DeviceData deviceData;
                deviceData = DeviceData.CreateFromJSON(request.downloadHandler.text);

                //getKeysReturned


                List<string> keysReturned = new List<string>(deviceData.devices.Keys);
                Debug.Log("Device IDs: " + string.Join(", ", keysReturned));
                Debug.Log("Device Zone: " + deviceData.devices["B35T"].zone);
                Debug.Log("Device Position: " + zonePositions[deviceData.devices["B35T"].zone]);
                Instantiate(sensorPrefab, zonePositions[deviceData.devices["B35T"].zone], Quaternion.identity);


            }
        }

    }

    public static float sensorYLevel = 1f; // The y-level at which the sensors will be instantiated.

      // // Dictionary containing zone names and their corresponding x,y,z coordinates.
    // From A1 to P11
    private Dictionary<string, Vector3> zonePositions = new Dictionary<string, Vector3> {
        {"A1", new Vector3(-8.51f, sensorYLevel, 3.14f)},
        {"A2", new Vector3(-8.19f, sensorYLevel, 3.63f)},
        {"A3", new Vector3(-7.87f, sensorYLevel, 4.12f)},
        {"A4", new Vector3(-7.55f, sensorYLevel, 4.61f)},
        {"A5", new Vector3(-7.23f, sensorYLevel, 5.10f)},
        {"A6", new Vector3(-6.91f, sensorYLevel, 5.59f)},
        {"A7", new Vector3(-6.59f, sensorYLevel, 6.08f)},
        {"A8", new Vector3(-6.27f, sensorYLevel, 6.57f)},
        {"A9", new Vector3(-5.95f, sensorYLevel, 7.06f)},
        {"A10", new Vector3(-5.63f, sensorYLevel, 7.55f)},
        {"A11", new Vector3(-5.32f, sensorYLevel, 8.04f)},
        {"B1", new Vector3(-7.98f, sensorYLevel, 2.8f)},
        {"B2", new Vector3(-7.66f, sensorYLevel, 3.29f)},
        {"B3", new Vector3(-7.34f, sensorYLevel, 3.78f)},
        {"B4", new Vector3(-7.02f, sensorYLevel, 4.27f)},
        {"B5", new Vector3(-6.70f, sensorYLevel, 4.76f)},
        {"B6", new Vector3(-6.38f, sensorYLevel, 5.25f)},
        {"B7", new Vector3(-6.06f, sensorYLevel, 5.74f)},
        {"B8", new Vector3(-5.74f, sensorYLevel, 6.23f)},
        {"B9", new Vector3(-5.42f, sensorYLevel, 6.72f)},
        {"B10", new Vector3(-5.10f, sensorYLevel, 7.21f)},
        {"B11", new Vector3(-4.78f, sensorYLevel, 7.70f)},
        {"C1", new Vector3(-7.45f, sensorYLevel, 2.46f)},
        {"C2", new Vector3(-7.13f, sensorYLevel, 2.95f)},
        {"C3", new Vector3(-6.81f, sensorYLevel, 3.44f)},
        {"C4", new Vector3(-6.49f, sensorYLevel, 3.93f)},
        {"C5", new Vector3(-6.17f, sensorYLevel, 4.42f)},
        {"C6", new Vector3(-5.85f, sensorYLevel, 4.91f)},
        {"C7", new Vector3(-5.53f, sensorYLevel, 5.40f)},
        {"C8", new Vector3(-5.21f, sensorYLevel, 5.89f)},
        {"C9", new Vector3(-4.89f, sensorYLevel, 6.38f)},
        {"C10", new Vector3(-4.57f, sensorYLevel, 6.87f)},
        {"C11", new Vector3(-4.25f, sensorYLevel, 7.36f)},
        {"D1", new Vector3(-6.92f, sensorYLevel, 2.12f)},
        {"D2", new Vector3(-6.60f, sensorYLevel, 2.61f)},
        {"D3", new Vector3(-6.28f, sensorYLevel, 3.10f)},
        {"D4", new Vector3(-5.96f, sensorYLevel, 3.59f)},
        {"D5", new Vector3(-5.64f, sensorYLevel, 4.08f)},
        {"D6", new Vector3(-5.32f, sensorYLevel, 4.57f)},
        {"D7", new Vector3(-5.00f, sensorYLevel, 5.06f)},
        {"D8", new Vector3(-4.68f, sensorYLevel, 5.55f)},
        {"D9", new Vector3(-4.36f, sensorYLevel, 6.04f)},
        {"D10", new Vector3(-4.04f, sensorYLevel, 6.53f)},
        {"D11", new Vector3(-3.72f, sensorYLevel, 7.02f)},
        {"E1", new Vector3(-6.39f, sensorYLevel, 1.78f)},
        {"E2", new Vector3(-6.07f, sensorYLevel, 2.27f)},
        {"E3", new Vector3(-5.75f, sensorYLevel, 2.76f)},
        {"E4", new Vector3(-5.43f, sensorYLevel, 3.25f)},
        {"E5", new Vector3(-5.11f, sensorYLevel, 3.74f)},
        {"E6", new Vector3(-4.79f, sensorYLevel, 4.23f)},
        {"E7", new Vector3(-4.47f, sensorYLevel, 4.72f)},
        {"E8", new Vector3(-4.15f, sensorYLevel, 5.21f)},
        {"E9", new Vector3(-3.83f, sensorYLevel, 5.70f)},
        {"E10", new Vector3(-3.51f, sensorYLevel, 6.19f)},
        {"E11", new Vector3(-3.19f, sensorYLevel, 6.68f)},
        {"F1", new Vector3(-5.86f, sensorYLevel, 1.44f)},
        {"F2", new Vector3(-5.54f, sensorYLevel, 1.93f)},
        {"F3", new Vector3(-5.22f, sensorYLevel, 2.42f)},
        {"F4", new Vector3(-4.90f, sensorYLevel, 2.91f)},
        {"F5", new Vector3(-4.58f, sensorYLevel, 3.40f)},
        {"F6", new Vector3(-4.26f, sensorYLevel, 3.89f)},
        {"F7", new Vector3(-3.94f, sensorYLevel, 4.38f)},
        {"F8", new Vector3(-3.62f, sensorYLevel, 4.87f)},
        {"F9", new Vector3(-3.30f, sensorYLevel, 5.36f)},
        {"F10", new Vector3(-2.98f, sensorYLevel, 5.85f)},
        {"F11", new Vector3(-2.66f, sensorYLevel, 6.34f)},
        {"G1", new Vector3(-5.33f, sensorYLevel, 1.10f)},
        {"G2", new Vector3(-5.01f, sensorYLevel, 1.59f)},
        {"G3", new Vector3(-4.69f, sensorYLevel, 2.08f)},
        {"G4", new Vector3(-4.37f, sensorYLevel, 2.57f)},
        {"G5", new Vector3(-4.05f, sensorYLevel, 3.06f)},
        {"G6", new Vector3(-3.73f, sensorYLevel, 3.55f)},
        {"G7", new Vector3(-3.41f, sensorYLevel, 4.04f)},
        {"G8", new Vector3(-3.09f, sensorYLevel, 4.53f)},
        {"G9", new Vector3(-2.77f, sensorYLevel, 5.02f)},
        {"G10", new Vector3(-2.45f, sensorYLevel, 5.51f)},
        {"G11", new Vector3(-2.13f, sensorYLevel, 6.00f)},
        {"H1", new Vector3(-4.80f, sensorYLevel, 0.76f)},
        {"H2", new Vector3(-4.48f, sensorYLevel, 1.25f)},
        {"H3", new Vector3(-4.16f, sensorYLevel, 1.74f)},
        {"H4", new Vector3(-3.84f, sensorYLevel, 2.23f)},
        {"H5", new Vector3(-3.52f, sensorYLevel, 2.72f)},
        {"H6", new Vector3(-3.20f, sensorYLevel, 3.21f)},
        {"H7", new Vector3(-2.88f, sensorYLevel, 3.70f)},
        {"H8", new Vector3(-2.56f, sensorYLevel, 4.19f)},
        {"H9", new Vector3(-2.24f, sensorYLevel, 4.68f)},
        {"H10", new Vector3(-1.92f, sensorYLevel, 5.17f)},
        {"H11", new Vector3(-1.60f, sensorYLevel, 5.66f)},
        {"I1", new Vector3(-4.27f, sensorYLevel, 0.42f)},
        {"I2", new Vector3(-3.95f, sensorYLevel, 0.91f)},
        {"I3", new Vector3(-3.63f, sensorYLevel, 1.40f)},
        {"I4", new Vector3(-3.31f, sensorYLevel, 1.89f)},
        {"I5", new Vector3(-2.99f, sensorYLevel, 2.38f)},
        {"I6", new Vector3(-2.67f, sensorYLevel, 2.87f)},
        {"I7", new Vector3(-2.35f, sensorYLevel, 3.36f)},
        {"I8", new Vector3(-2.03f, sensorYLevel, 3.85f)},
        {"I9", new Vector3(-1.71f, sensorYLevel, 4.34f)},
        {"I10", new Vector3(-1.39f, sensorYLevel, 4.83f)},
        {"I11", new Vector3(-1.07f, sensorYLevel, 5.32f)},
        {"J1", new Vector3(-3.74f, sensorYLevel, 0.08f)},
        {"J2", new Vector3(-3.42f, sensorYLevel, 0.57f)},
        {"J3", new Vector3(-3.10f, sensorYLevel, 1.06f)},
        {"J4", new Vector3(-2.78f, sensorYLevel, 1.55f)},
        {"J5", new Vector3(-2.46f, sensorYLevel, 2.04f)},
        {"J6", new Vector3(-2.14f, sensorYLevel, 2.53f)},
        {"J7", new Vector3(-1.82f, sensorYLevel, 3.02f)},
        {"J8", new Vector3(-1.50f, sensorYLevel, 3.51f)},
        {"J9", new Vector3(-1.18f, sensorYLevel, 4.00f)},
        {"J10", new Vector3(-0.86f, sensorYLevel, 4.49f)},
        {"J11", new Vector3(-0.54f, sensorYLevel, 4.98f)},
        {"K1", new Vector3(-3.21f, sensorYLevel, -0.26f)},
        {"K2", new Vector3(-2.89f, sensorYLevel, 0.23f)},
        {"K3", new Vector3(-2.57f, sensorYLevel, 0.72f)},
        {"K4", new Vector3(-2.25f, sensorYLevel, 1.21f)},
        {"K5", new Vector3(-1.93f, sensorYLevel, 1.70f)},
        {"K6", new Vector3(-1.61f, sensorYLevel, 2.19f)},
        {"K7", new Vector3(-1.29f, sensorYLevel, 2.68f)},
        {"K8", new Vector3(-0.97f, sensorYLevel, 3.17f)},
        {"K9", new Vector3(-0.65f, sensorYLevel, 3.66f)},
        {"K10", new Vector3(-0.33f, sensorYLevel, 4.15f)},
        {"K11", new Vector3(-0.01f, sensorYLevel, 4.64f)},
        {"L1", new Vector3(-2.68f, sensorYLevel, -0.60f)},
        {"L2", new Vector3(-2.36f, sensorYLevel, -0.11f)},
        {"L3", new Vector3(-2.04f, sensorYLevel, 0.38f)},
        {"L4", new Vector3(-1.72f, sensorYLevel, 0.87f)},
        {"L5", new Vector3(-1.40f, sensorYLevel, 1.36f)},
        {"L6", new Vector3(-1.08f, sensorYLevel, 1.85f)},
        {"L7", new Vector3(-0.76f, sensorYLevel, 2.34f)},
        {"L8", new Vector3(-0.44f, sensorYLevel, 2.83f)},
        {"L9", new Vector3(-0.12f, sensorYLevel, 3.32f)},
        {"L10", new Vector3(0.20f, sensorYLevel, 3.81f)},
        {"L11", new Vector3(0.52f, sensorYLevel, 4.30f)},
        {"M1", new Vector3(-2.15f, sensorYLevel, -0.94f)},
        {"M2", new Vector3(-1.83f, sensorYLevel, -0.45f)},
        {"M3", new Vector3(-1.51f, sensorYLevel, 0.04f)},
        {"M4", new Vector3(-1.19f, sensorYLevel, 0.53f)},
        {"M5", new Vector3(-0.87f, sensorYLevel, 1.02f)},
        {"M6", new Vector3(-0.55f, sensorYLevel, 1.51f)},
        {"M7", new Vector3(-0.23f, sensorYLevel, 2.00f)},
        {"M8", new Vector3(0.09f, sensorYLevel, 2.49f)},
        {"M9", new Vector3(0.41f, sensorYLevel, 2.98f)},
        {"M10", new Vector3(0.73f, sensorYLevel, 3.47f)},
        {"M11", new Vector3(1.05f, sensorYLevel, 3.96f)},
        {"N1", new Vector3(-1.62f, sensorYLevel, -1.28f)},
        {"N2", new Vector3(-1.30f, sensorYLevel, -0.79f)},
        {"N3", new Vector3(-0.98f, sensorYLevel, -0.30f)},
        {"N4", new Vector3(-0.66f, sensorYLevel, 0.19f)},
        {"N5", new Vector3(-0.34f, sensorYLevel, 0.68f)},
        {"N6", new Vector3(-0.02f, sensorYLevel, 1.17f)},
        {"N7", new Vector3(0.30f, sensorYLevel, 1.66f)},
        {"N8", new Vector3(0.62f, sensorYLevel, 2.15f)},
        {"N9", new Vector3(0.94f, sensorYLevel, 2.64f)},
        {"N10", new Vector3(1.26f, sensorYLevel, 3.13f)},
        {"N11", new Vector3(1.58f, sensorYLevel, 3.62f)},
        {"O1", new Vector3(-1.09f, sensorYLevel, -1.62f)},
        {"O2", new Vector3(-0.77f, sensorYLevel, -1.13f)},
        {"O3", new Vector3(-0.45f, sensorYLevel, -0.64f)},
        {"O4", new Vector3(-0.13f, sensorYLevel, -0.15f)},
        {"O5", new Vector3(0.19f, sensorYLevel, 0.34f)},
        {"O6", new Vector3(0.51f, sensorYLevel, 0.83f)},
        {"O7", new Vector3(0.83f, sensorYLevel, 1.32f)},
        {"O8", new Vector3(1.15f, sensorYLevel, 1.81f)},
        {"O9", new Vector3(1.47f, sensorYLevel, 2.30f)},
        {"O10", new Vector3(1.79f, sensorYLevel, 2.79f)},
        {"O11", new Vector3(2.11f, sensorYLevel, 3.28f)},
        {"P1", new Vector3(-0.56f, sensorYLevel, -1.96f)},
        {"P2", new Vector3(-0.24f, sensorYLevel, -1.47f)},
        {"P3", new Vector3(0.08f, sensorYLevel, -0.98f)},
        {"P4", new Vector3(0.40f, sensorYLevel, -0.49f)},
        {"P5", new Vector3(0.72f, sensorYLevel, 0.00f)},
        {"P6", new Vector3(1.04f, sensorYLevel, 0.49f)},
        {"P7", new Vector3(1.36f, sensorYLevel, 0.98f)},
        {"P8", new Vector3(1.68f, sensorYLevel, 1.47f)},
        {"P9", new Vector3(2.00f, sensorYLevel, 1.96f)},
        {"P10", new Vector3(2.32f, sensorYLevel, 2.45f)},
        {"P11", new Vector3(2.64f, sensorYLevel, 2.94f)}
    };

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

    // Initializer to ensure that the dictionary is not null.
    public DeviceData() {
        devices = new Dictionary<string, DeviceObject>();
    }

    /// <summary>
    /// Parses the JSON string into a DeviceData object.
    /// </summary>
    /// <param name="jsonString"> Received message of sensor readings, formatted as a JSON string. </param>
    public static DeviceData CreateFromJSON(string jsonString)
    {
        //Debug.Log("JSON String: " + jsonString);
        // Doesn't support top-level dictionaries, could modify the response format, but this is a simple solution for now
        string modifiedJson = "{\"devices\":" + jsonString + "}";
        Debug.Log("Modified JSON: " + modifiedJson);
        return JsonConvert.DeserializeObject<DeviceData>(modifiedJson); // Returns a SensorReadings object from the JSON string.
    }

}


[System.Serializable]
public class DeviceObject {
    public string building {get; set;}
    public string device_type {get; set;}
    public string room {get; set;}
    public string zone {get; set;}

    /// <summary>
    /// calculates the position of the sensor based on the zone. The first character is a letter between A-Z, representing space from west to east.
    /// The second character is a number, starting with 1, representing space from north to south.
    /// </summary>
    /// <returns></returns>
    private Vector3 calculationPosition() {
        return new Vector3(0,0,0);
    }
    
}