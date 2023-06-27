/*using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System.Collections.Generic;
using SocketIOClient;
using SocketIOClient.Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class UnityWebRequestCertificateHandler : CertificateHandler
{
    private string certificateContents;

    public UnityWebRequestCertificateHandler(string certificateContents)
    {
        this.certificateContents = certificateContents;
    }

    protected override bool ValidateCertificate(byte[] certificateData)
    {
        // Implement your certificate validation logic here
        // For simplicity, we'll accept any certificate for now
        return true;
    }
}

public class SocketConnection : MonoBehaviour
{
    public SocketIOUnity socket;

    void Start()
    {
        // Load the certificate file
        string certificatePath = "Assets/tls-cert-RSRCH-9020-01.ad.utulsa.edu.crt";
        string certificateContents = File.ReadAllText(certificatePath);

        // Create a certificate handler with the certificate contents
        var certificateHandler = new UnityWebRequestCertificateHandler(certificateContents);

        // Create a UnityWebRequest with the server URL
        string serverUrl = "https://rsrch-9020-01.ad.utulsa.edu";
        UnityWebRequest request = UnityWebRequest.Get(serverUrl);

        // Assign the certificate handler to the request
        request.certificateHandler = certificateHandler;

        // Set username and password for authentication
        string username = "admin";
        string password = "5db4315058a24d17b21b3a5ccbcc9f06";
        string credentials = username + ":" + password;
        byte[] encodedCredentials = System.Text.Encoding.UTF8.GetBytes(credentials);
        string base64Credentials = System.Convert.ToBase64String(encodedCredentials);
        request.SetRequestHeader("Authorization", "Basic " + base64Credentials);

        // Send the request
        var operation = request.SendWebRequest();

        // Handle the request completion
        operation.completed += (asyncOperation) =>
        {
            if (request.result == UnityWebRequest.Result.Success)
            {
                // The request was successful, establish Socket.IO connection
                var uri = new System.Uri(serverUrl);
                var options = new SocketIOOptions
                {
                    Query = new Dictionary<string, string>
                    {
                        { "token", "UNITY" }
                    }
                };
                socket = new SocketIOUnity(uri, options);
                socket.JsonSerializer = new NewtonsoftJsonSerializer();

                socket.OnConnected += Socket_OnConnected;
                socket.On("data-update", response => Debug.Log(response.GetValue<JObject>().ToString()));
                socket.Connect();

                // Emit 'sub-request' event
                JObject subRequest = new JObject();
                subRequest["channel_type"] = "device-service";
                subRequest["installation_tag"] = "installation-0";
                subRequest["collector_tag"] = "installation-0";
                subRequest["device_tag"] = "device-2";
                subRequest["service_tag"] = "temperature";
                socket.Emit("sub-request", subRequest);
            }
            else
            {
                // The request failed, handle the error
                Debug.LogError("Certificate request failed: " + request.error);
            }
        };
    }

    private void Socket_OnConnected(object sender, System.EventArgs e)
    {
        Debug.Log("Connected to the server");
    }

    private void OnDestroy()
    {
        if (socket != null)
        {
            socket.Disconnect();
        }
    }
}*/






/*using UnityEngine;
using SocketIOClient;
using SocketIOClient.Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

public class SocketConnection : MonoBehaviour
{
    private SocketIOUnity socket;


    void Start()
    {
        JObject credentials = new JObject();
        credentials["username"] = "admin";
        credentials["password"] = "5db4315058a24d17b21b3a5ccbcc9f06";

        var uri = new Uri("http://localhost:3001");  // change this to your server's URI
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

        socket.Emit("auth-request", credentials);

        socket.On("auth-success", (data) =>
        {
            UnityEngine.Debug.Log("Data Update: " + data.GetValue<JObject>().ToString());
        });

        socket.On("app-error", (data) =>
        {
            UnityEngine.Debug.Log("App error: " + data.GetValue<JObject>().ToString());
        });

        socket.On("connect", (data) =>
        {
            UnityEngine.Debug.Log("Connected: " + data.GetValue<JObject>().ToString());
        });

        socket.On("connect_error", (data) =>
        {
            UnityEngine.Debug.Log("Connect-error: " + data.GetValue<JObject>().ToString());
        });

        socket.On("connect_timeout", (data) =>
        {
            UnityEngine.Debug.Log("Connect-timeout: " + data.GetValue<JObject>().ToString());
        });

        socket.On("error", (data) =>
        {
            UnityEngine.Debug.Log("error: " + data.GetValue<JObject>().ToString());
        });

        socket.On("disconnect", (data) =>
        {
            UnityEngine.Debug.Log("disconnect: " + data.GetValue<JObject>().ToString());
        });

        socket.OnConnected += (sender, e) =>
        {
            UnityEngine.Debug.Log("socket.OnConnected");
        };
        socket.OnDisconnected += (sender, e) =>
        {
            UnityEngine.Debug.Log("disconnect: " + e);
        };
        socket.On("data-update", (data) =>
        {
            UnityEngine.Debug.Log("Data Update: " + data.GetValue<JObject>().ToString());
        });
        socket.OnError += (sender, e) =>
        {
            UnityEngine.Debug.Log("Socket.IO Error");
            UnityEngine.Debug.Log(e);
        };

        socket.Connect();
    }
}*/



/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIOClient;

public class SocketConnection : MonoBehaviour
{
    private SocketIOUnity socket;

    void Start()
    {
        socket = new SocketIOUnity("http://localhost:3001");

        socket.OnConnected += Socket_OnConnected;
        socket.On("data-update", response => Debug.Log(response.GetValue<string>()));
        socket.Connect();
    }

    private void Socket_OnConnected(object sender, System.EventArgs e)
    {
        Debug.Log("Connected");
        socket.Emit("subscribe", "some-channel");
    }

    private void OnDestroy()
    {
        socket.Disconnect();
    }
}*/


/*using System;
using System.Collections.Generic;
using SocketIOClient;
using SocketIOClient.Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;

using Debug = System.Diagnostics.Debug;


public class SocketManager : MonoBehaviour
{
    public SocketIOUnity socket;

    public InputField EventNameTxt;
    public InputField DataTxt;
    public Text ReceivedText;

    public GameObject objectToSpin;

    // Start is called before the first frame update
    void Start()
    {
        //TODO: check the Uri if Valid.
        var uri = new Uri("http://localhost:3001");
        socket = new SocketIOUnity(uri, new SocketIOOptions
        {
            Query = new Dictionary<string, string>
                {
                    {"token", "UNITY" }
                }
            ,
            EIO = 4
            ,
            Transport = SocketIOClient.Transport.TransportProtocol.WebSocket
        });
        socket.JsonSerializer = new NewtonsoftJsonSerializer();

        ///// reserved socketio events
        socket.OnConnected += (sender, e) =>
        {
            Debug.Print("socket.OnConnected");
        };
        socket.OnPing += (sender, e) =>
        {
            Debug.Print("Ping");
        };
        socket.OnPong += (sender, e) =>
        {
            Debug.Print("Pong: " + e.TotalMilliseconds);
        };
        socket.OnDisconnected += (sender, e) =>
        {
            Debug.Print("disconnect: " + e);
        };
        socket.OnReconnectAttempt += (sender, e) =>
        {
            Debug.Print($"{DateTime.Now} Reconnecting: attempt = {e}");
        };
        ////

        Debug.Print("Connecting...");
        socket.On("data-update", response => Debug.Print(response.GetValue<string>()));
        socket.Connect();
        //socket.Connect();

        socket.OnUnityThread("spin", (data) =>
        {
            rotateAngle = 0;
        });

        ReceivedText.text = "";
        socket.OnAnyInUnityThread((name, response) =>
        {
            ReceivedText.text += "Received On " + name + " : " + response.GetValue<JToken>().ToString() + "\n";
        });


    }

    public void EmitTest()
    {
        string eventName = EventNameTxt.text.Trim().Length < 1 ? "hello" : EventNameTxt.text;
        string txt = DataTxt.text;
        if (!IsJSON(txt))
        {
            socket.Emit(eventName, txt);
        }
        else
        {
            socket.EmitStringAsJSON(eventName, txt);
        }
    }

    public static bool IsJSON(string str)
    {
        if (string.IsNullOrWhiteSpace(str)) { return false; }
        str = str.Trim();
        if ((str.StartsWith("{") && str.EndsWith("}")) || //For object
            (str.StartsWith("[") && str.EndsWith("]"))) //For array
        {
            try
            {
                var obj = JToken.Parse(str);
                return true;
            }
            catch (Exception ex) //some other exception
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public void EmitSpin()
    {
        socket.Emit("spin");
    }

    public void EmitClass()
    {
        TestClass testClass = new TestClass(new string[] { "foo", "bar", "baz", "qux" });
        TestClass2 testClass2 = new TestClass2("lorem ipsum");
        socket.Emit("class", testClass2);
    }

    // our test class
    [System.Serializable]
    class TestClass
    {
        public string[] arr;

        public TestClass(string[] arr)
        {
            this.arr = arr;
        }
    }

    [System.Serializable]
    class TestClass2
    {
        public string text;

        public TestClass2(string text)
        {
            this.text = text;
        }
    }
    //


    float rotateAngle = 45;
    readonly float MaxRotateAngle = 45;
    void Update()
    {
        if (rotateAngle < MaxRotateAngle)
        {
            rotateAngle++;
            objectToSpin.transform.Rotate(0, 1, 0);
        }
    }
}
*/