

using System;
using System.Collections.Generic;
using SocketIOClient;
using SocketIOClient.Newtonsoft.Json;
using UnityEngine;
using Newtonsoft.Json.Linq;

public class SocketConnection : MonoBehaviour
{
    public SocketIOUnity socket;

    public SensorManager sensorManager;

    
    public ReadingTypesHandler readingTypesHandler;

    public string installation_id = "office-testbed";
    public string collector_id = "collector-0";



    private string zoneId;


    void Start()
    {
        // Comment out the socket.Connect() line
        // so it doesn't automatically connect on start
        // socket.Connect();
    }

    public void InitializeSocketConnection(string zoneId)
    {

        this.zoneId = zoneId;
        JObject credentials = new JObject();
        credentials["username"] = "dashboard";
        credentials["password"] = "e58b69ad62f34002a344af2e6a7a19bd";

        var uri = new Uri("http://129.244.254.218:80");

        socket = new SocketIOUnity(uri, new SocketIOOptions
        {
            Query = new Dictionary<string, string>
            {
                {"token", "UNITY" }
            },
            EIO = 4,
            Transport = SocketIOClient.Transport.TransportProtocol.WebSocket
        });
        socket.JsonSerializer = new NewtonsoftJsonSerializer();

        socket.OnConnected += (sender, e) =>
        {
            UnityEngine.Debug.Log("socket.OnConnected");
            Authenticate();
        };
        socket.OnDisconnected += (sender, e) =>
        {
            Debug.Log("disconnect: " + e);
        };
        socket.On("auth-success", (data) => OnAuthSuccess(data));
        socket.On("sub-update", (data) => {
            UnityEngine.Debug.Log("Sub Update");
            OnSubUpdate(data);
        });

        socket.On("sub-success", (data) => {
            UnityEngine.Debug.Log("sub-success");
            UnityEngine.Debug.Log(data);
        });

        UnityEngine.Debug.Log("Connecting..");
        socket.Connect();
    }

    void Authenticate()
    {
        JObject credentials = new JObject();
        credentials["username"] = "dashboard";
        credentials["password"] = "e58b69ad62f34002a344af2e6a7a19bd";

        socket.Emit("auth-request", credentials);
    }

    void OnAuthSuccess(object data)
    {
        UnityEngine.Debug.Log("Successfully Authenticated");
        SubscribeToServices();
    }

    public void SubscribeToServices(string filterReadingType = null)
    {
        UnityEngine.Debug.Log("Subscribing to services");

        JObject channelSpec = new JObject();
        channelSpec["installation_id"] = installation_id;
        channelSpec["collector_id"] = collector_id;
        channelSpec["zone_id"] = zoneId;

        if (!string.IsNullOrEmpty(filterReadingType))
        {
            JObject filter = new JObject();
            filter["reading_type"] = filterReadingType;
            channelSpec["filter"] = filter;
        }

        JObject subRequest = new JObject();
        subRequest["channel_type"] = "zone";
        subRequest["channel_spec"] = channelSpec;

        socket.Emit("sub-request", subRequest);
    }


    public void UnsubscribeFromServices(string filterReadingType = null)
    {
        Debug.Log("Unsubscribing from services, zone: " + zoneId);

        JObject channelSpec = new JObject();
        channelSpec["installation_id"] = installation_id;
        channelSpec["collector_id"] = collector_id;
        channelSpec["zone_id"] = zoneId;

        if (!string.IsNullOrEmpty(filterReadingType))
        {
            JObject filter = new JObject();
            filter["reading_type"] = filterReadingType;
            channelSpec["filter"] = filter;
        }

        JObject unsubRequest = new JObject();
        unsubRequest["channel_type"] = "zone";
        unsubRequest["channel_spec"] = channelSpec;

        socket.Emit("unsub-request", unsubRequest);
    }


    void OnSubUpdate(SocketIOResponse response)
    {

        JObject jsonData = response.GetValue<JObject>();
        /*Debug.Log(jsonData);*/
        Debug.Log(jsonData);
        string dataType = jsonData["update_msg"]["reading"]["reading_type"].ToString();
        string dataValue = jsonData["update_msg"]["reading"]["reading_val"].ToString();
        string readingUnit = jsonData["update_msg"]["reading"]["reading_unit"]?.ToString() ?? "N/A";

        if (readingTypesHandler.IsNewReadingType(dataType))
        {
            readingTypesHandler.AddAndDisplayReadingType(dataType);
        }
        else
        {
            Debug.Log("NOT A NEW READING TYPE");
        }

        if (dataType == readingTypesHandler.GetCurrentSelectedReadingType())
        {

            sensorManager.HandleNewSensorData(dataType, dataValue, readingUnit);
        }

        

    }


    void Update()
    {
        // Update code here if needed
    }
}
