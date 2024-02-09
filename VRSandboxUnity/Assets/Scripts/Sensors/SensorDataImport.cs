using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SensorDataImport : MonoBehaviour
{
    
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MakeRequest());
    }

    private IEnumerator MakeRequest()
    {
        var getRequest = CreateRequest("https://129.244.254.218/api/search/historical/readings?min_reading_time=2024-01-31T12:29:54-06:30&max_reading_time=2024-01-31T12:30:00-06:30&sensor_id=G1XE");
        //var getRequest = CreateRequest("catfact.ninja/fact");
        var cert = new ForceAcceptAll();
        getRequest.certificateHandler = cert;


        yield return getRequest.SendWebRequest();
        switch (getRequest.result)
        {
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.ProtocolError:
            case UnityWebRequest.Result.DataProcessingError:
                Debug.LogError("Something went wrong: " + getRequest.error);
                break;
        }
        var deserializedData = JsonUtility.FromJson<Todo>(getRequest.downloadHandler.text);
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
    //public string fact;
    //public int length;
}

public class ForceAcceptAll : CertificateHandler
{
    protected override bool ValidateCertificate(byte[] certificateData)
    {
        return true;
    }
}
