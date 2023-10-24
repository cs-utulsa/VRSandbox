/*using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using System.Collections.Generic;


public class AlertPolling : MonoBehaviour
{
    [Header("Alert Configuration")]
    public float pollingInterval = 5f;
    public TMP_Text alertText;
    public GameObject alertPanel;
    public Button dismissButton;

    private const string url = "http://129.244.254.218:80/api/search/active/sensors";
    private const string username = "dashboard";
    private const string password = "e58b69ad62f34002a344af2e6a7a19bd";

    [System.Serializable]
    public class AlertEvent
    {
        public string collector;
        public string installation;
        public string name;
        public string obs_prop;
        public string sensor;
        public string timestamp;
        public string val;
        public string val_type;
        public string zone;
    }

    [System.Serializable]
    public class DogCommission
    {
        public string name;
        public List<AlertEvent> simple_events;
    }

    [System.Serializable]
    public class Payload
    {
        public List<DogCommission> dog_commission;
    }


    [System.Serializable]
    public class AlertResponse
    {
        [SerializeField]
        private Payload payload;
        [SerializeField]
        private int status_code;
        [SerializeField]
        private string success;

        public Payload Payload => payload;
        public int StatusCode => status_code;
        public string Success => success;
    }



    private void Start()
    {
        alertPanel.SetActive(false);
        Debug.Log("Start");
        StartCoroutine(AlertPollingCoroutine());
        dismissButton.onClick.AddListener(DismissAlert);
    }

    private IEnumerator AlertPollingCoroutine()
    {
        while (true)
        {
            yield return SendAlertRequest(url, username, password);
            yield return new WaitForSeconds(pollingInterval);
        }
    }

    private IEnumerator SendAlertRequest(string url, string username, string password)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            SetAuthHeader(www, username, password);
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + www.error);
            }
            else
            {


                string jsonResponse = www.downloadHandler.text;
                AlertResponse alertData = JsonUtility.FromJson<AlertResponse>(jsonResponse);
                ProcessAlertResponse(alertData);
                string jsonResponse = www.downloadHandler.text;

                // Hardcoded test response for debugging
                jsonResponse = @"{
                    ""payload"": {
                        ""dog_commission"": [{
                            ""name"": ""TestDog"",
                            ""simple_events"": [{
                                ""collector"": ""TestCollector"",
                                ""installation"": ""TestInstallation"",
                                ""name"": ""TestEventName"",
                                ""obs_prop"": ""TestObsProp"",
                                ""sensor"": ""TestSensor"",
                                ""timestamp"": ""TestTimestamp"",
                                ""val"": ""TestVal"",
                                ""val_type"": ""TestValType"",
                                ""zone"": ""TestZone""
                            }]
                        }]
                    },
                    ""status_code"": 200,
                    ""success"": ""true""
                }";

                Debug.Log("Using hardcoded jsonResponse: " + jsonResponse);

                AlertResponse alertData = JsonUtility.FromJson<AlertResponse>(jsonResponse);
                ProcessAlertResponse(alertData);
            }
        }
    }

    private void SetAuthHeader(UnityWebRequest www, string username, string password)
    {
        string authorization = username + ":" + password;
        byte[] bytes = System.Text.Encoding.ASCII.GetBytes(authorization);
        string base64Authorization = System.Convert.ToBase64String(bytes);
        www.SetRequestHeader("Authorization", "Basic " + base64Authorization);
    }

    private void ProcessAlertResponse(AlertResponse response)
    {
        if (response.Payload != null && response.Payload.dog_commission != null)
        {
            Debug.Log("Number of dog_commission entries: " + response.Payload.dog_commission.Count);
            foreach (var dogCommission in response.Payload.dog_commission)
            {
                foreach (var simpleEvent in dogCommission.simple_events)
                {
                    string message = $"Motion in {simpleEvent.name} detected in {simpleEvent.zone}, dog activated.";
                    alertText.text = message;
                    alertPanel.SetActive(true);
                }
            }
        }

        string responseJson = JsonUtility.ToJson(response, true); // The second argument (true) makes the JSON output pretty-printed.
        Debug.Log("AlertResponse JSON: " + responseJson);
    }

    private void DismissAlert()
    {
        alertPanel.SetActive(false);
    }
}
*/

using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using System.Collections.Generic;

public class AlertPolling : MonoBehaviour
{
    [Header("Alert Configuration")]

    // Time interval for polling the server for alerts.
    public float pollingInterval = 5f;

    // Text field to display alert messages.
    public TMP_Text alertText;

    // Panel to display when an alert is received.
    public GameObject alertPanel;

    // Button to dismiss the alert.
    public Button dismissButton;

    // Server details for API request.
    private const string url = "http://129.244.254.218:80/api/search/active/sensors";
    private const string username = "dashboard";
    private const string password = "e58b69ad62f34002a344af2e6a7a19bd";

    // Data structure to hold details of an alert event.
    [System.Serializable]
    public class AlertEvent
    {
        public string collector;
        public string installation;
        public string name;
        public string obs_prop;
        public string sensor;
        public string timestamp;
        public string val;
        public string val_type;
        public string zone;
    }

    // Data structure to hold a list of alert events grouped under a dog's name.
    [System.Serializable]
    public class DogCommission
    {
        public string name;
        public List<AlertEvent> simple_events;
    }

    // Data structure to hold the list of dog commissions.
    [System.Serializable]
    public class Payload
    {
        public List<DogCommission> dog_commission;
    }

    // Data structure to hold the response from the API, including the payload and some metadata.
    [System.Serializable]
    public class AlertResponse
    {
        [SerializeField]
        private Payload payload;
        [SerializeField]
        private int status_code;
        [SerializeField]
        private string success;

        public Payload Payload => payload;
        public int StatusCode => status_code;
        public string Success => success;
    }

    // Initialization method.
    private void Start()
    {
        // Hide the alert panel initially.
        alertPanel.SetActive(false);
        Debug.Log("Start");

        // Start the alert polling coroutine.
        StartCoroutine(AlertPollingCoroutine());

        // Attach the DismissAlert method to the dismiss button's click event.
        dismissButton.onClick.AddListener(DismissAlert);
    }

    // Coroutine to repeatedly send alert requests at regular intervals.
    private IEnumerator AlertPollingCoroutine()
    {
        while (true)
        {
            yield return SendAlertRequest(url, username, password);
            yield return new WaitForSeconds(pollingInterval);
        }
    }

    // Coroutine to send a request to the server to check for alerts.
    private IEnumerator SendAlertRequest(string url, string username, string password)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            // Add authentication headers.
            SetAuthHeader(www, username, password);

            // Send the web request.
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                // Log errors if the request fails.
                Debug.LogError("Error: " + www.error);
            }
            else
            {

                // Uncomment this code to actually poll the endpoint

                /*string jsonResponse = www.downloadHandler.text;
                AlertResponse alertData = JsonUtility.FromJson<AlertResponse>(jsonResponse);
                ProcessAlertResponse(alertData);*/

                // This is dummy data; comment to use above code
                string jsonResponse = www.downloadHandler.text;

                // Use a hardcoded response for debugging.
                jsonResponse = @"{
                    ""payload"": {
                        ""dog_commission"": [{
                            ""name"": ""TestDog"",
                            ""simple_events"": [{
                                ""collector"": ""TestCollector"",
                                ""installation"": ""TestInstallation"",
                                ""name"": ""TestEventName"",
                                ""obs_prop"": ""TestObsProp"",
                                ""sensor"": ""TestSensor"",
                                ""timestamp"": ""TestTimestamp"",
                                ""val"": ""TestVal"",
                                ""val_type"": ""TestValType"",
                                ""zone"": ""TestZone""
                            }]
                        }]
                    },
                    ""status_code"": 200,
                    ""success"": ""true""
                }";

               

                // Parse the JSON response into our AlertResponse class.
                AlertResponse alertData = JsonUtility.FromJson<AlertResponse>(jsonResponse);

                // Process the parsed data.
                ProcessAlertResponse(alertData);
            }
        }
    }

    // Method to set the authentication header for the web request.
    private void SetAuthHeader(UnityWebRequest www, string username, string password)
    {
        string authorization = username + ":" + password;
        byte[] bytes = System.Text.Encoding.ASCII.GetBytes(authorization);
        string base64Authorization = System.Convert.ToBase64String(bytes);
        www.SetRequestHeader("Authorization", "Basic " + base64Authorization);
    }

    // Process the alert response and display any relevant alerts.
    private void ProcessAlertResponse(AlertResponse response)
    {
        if (response.Payload != null && response.Payload.dog_commission != null)
        {
            Debug.Log("Number of dog_commission entries: " + response.Payload.dog_commission.Count);

            foreach (var dogCommission in response.Payload.dog_commission)
            {
                foreach (var simpleEvent in dogCommission.simple_events)
                {
                    string message = $"Motion in {simpleEvent.name} detected in {simpleEvent.zone}, dog activated.";
                    alertText.text = message;
                    alertPanel.SetActive(true);
                }
            }
        }

        string responseJson = JsonUtility.ToJson(response, true);  // Convert the response back to JSON for debugging.
        Debug.Log("AlertResponse JSON: " + responseJson);
    }

    // Method to hide the alert panel.
    private void DismissAlert()
    {
        alertPanel.SetActive(false);
    }
}
