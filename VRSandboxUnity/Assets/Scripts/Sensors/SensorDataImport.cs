using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using Newtonsoft.Json;
using TMPro;

public class SensorDataImport : MonoBehaviour
{
    private List<string> Requests =new List<string>();
    private List<Payload> AllPayloads = new List<Payload>();
    
    // Start is called before the first frame update
    void Start()
    {
        Requests.Add("https://129.244.254.218/api/search/historical/readings?min_reading_time=2024-01-31T12:29:54-06:30&max_reading_time=2024-01-31T12:30:00-06:30&sensor_id=G1XE");
        Requests.Add("https://129.244.254.218/api/search/historical/readings?min_reading_time=2024-01-31T12:29:45-06:30&max_reading_time=2024-01-31T12:29:53-06:30&sensor_id=G1XE");
        
        for(int i = 0; i < Requests.Count; i++) 
        {
            StartCoroutine(MakeRequest(Requests[i]));
        }
        print(AllPayloads.Count);

        for (int i = 0; i < AllPayloads.Count; i++)
        {
            print("Deserialized Data " + i + ": " + AllPayloads[i].PayloadString());
        }
    }

    private IEnumerator MakeRequest(string path)
    {
        //Link
        var getRequest = CreateRequest(path);

        //forces Unity to accept certification
        var cert = new ForceAcceptAll();
        getRequest.certificateHandler = cert;

        //waits for request to return
        yield return getRequest.SendWebRequest();
        switch (getRequest.result) //Error handling
        {
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.ProtocolError:
            case UnityWebRequest.Result.DataProcessingError:
                Debug.LogError("Something went wrong: " + getRequest.error);
                break;
        }
        Root deserializedData = JsonConvert.DeserializeObject<Root>(getRequest.downloadHandler.text);
        //print("Raw Data: "+getRequest.downloadHandler.text);
        List<Payload> currentPayload = deserializedData.getPayload();
        
        for(int i = 0;i<currentPayload.Count;i++)
        {
            print("Deserialized Data V1 "+i+": " + currentPayload[i].PayloadString());
        }
        

        AllPayloads.AddRange(currentPayload);
        //print("Deserialized Data: "+ currentPayload[0].PayloadString());

        cert?.Dispose();

    }

    private UnityWebRequest CreateRequest(string path)
    {
        var request = new UnityWebRequest(path);
        request.downloadHandler = new DownloadHandlerBuffer();

        string authorization = authenticate("dashboard", "e58b69ad62f34002a344af2e6a7a19bd");

        request.SetRequestHeader("AUTHORIZATION", authorization);
        //request.SetRequestHeader("Username", "dashboard");
        //request.SetRequestHeader("Password", "e58b69ad62f34002a344af2e6a7a19bd");


        return request;
    }

    string authenticate(string username, string password)
    {
        string auth = username + ":" + password;
        auth = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(auth));
        auth = "Basic " + auth;
        return auth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


public class HardwareInfo
{
    public string sensor_id;

    public string GetSensorID()
    {
        return sensor_id;
    }
}

public class Payload
{
    public string collector_id;
    public HardwareInfo hardware_info;
    public string installation_id;
    public object reading_metadata;
    public DateTime reading_time;
    public string reading_type;
    public string reading_unit;
    public object reading_val;
    public string reading_val_type;
    public string zone_id;

    public string PayloadString()
    {
        string myString =
            "\nZone_id: " + zone_id +
            "\nCollector_id: " + collector_id +
            "\nSensor_id: " + hardware_info.GetSensorID() +
            "\nReading_Time: " + reading_time +
            "\nReading_Type: " + reading_type +
            "\nReading_Unit: " + reading_unit +
            "\nReading_Value: " + reading_val +
            "\nReading_Value_Type: " + reading_val_type;
        return myString;
    }


}

public class Root
{
    public List<Payload> payload;
    public int status_code;
    public string success;

    public List<Payload> getPayload()
    {
        return payload;
    }
}

public class ForceAcceptAll : CertificateHandler
{
    protected override bool ValidateCertificate(byte[] certificateData)
    {
        return true;
    }
}
