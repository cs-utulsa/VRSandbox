using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using Newtonsoft.Json;

public class SensorDataImport : MonoBehaviour
{
    
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MakeRequest());
    }

    private IEnumerator MakeRequest()
    {
        //Link
        var getRequest = CreateRequest("https://129.244.254.218/api/search/historical/readings?min_reading_time=2024-01-31T12:29:54-06:30&max_reading_time=2024-01-31T12:30:00-06:30&sensor_id=G1XE");

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
        Root deserializedData = JsonUtility.FromJson<Root>(getRequest.downloadHandler.text);
        //Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        print("Raw Data: "+getRequest.downloadHandler.text);
        print("Deserialized Data: "+deserializedData);

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

public class Todo
{
    public string sensor_id;
    public string reading_time;
    public string reading_val;
}

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
public class HardwareInfo
{
    public string sensor_id;
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
}

public class Root
{
    public List<Payload> payload;
    public int status_code;
    public string success;
}

public class ForceAcceptAll : CertificateHandler
{
    protected override bool ValidateCertificate(byte[] certificateData)
    {
        return true;
    }
}
