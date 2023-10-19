using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class PostRequestTabSelect : MonoBehaviour
{
    private Tab _tab;

    public GameObject zonePrefab; // Drag and drop a prefab with a TextMeshProUGUI component in the inspector
    public Transform zoneParent; // Parent transform under which the zones will be instantiated
    public GameObject zoneDisplay;
    public SocketConnection socketConnectionScript;
    public ReadingTypesHandler readingTypesHandlerScript;
    public GameObject backButton;
    public GameObject loadingPrefab;
    public GameObject reactivate;
    public string targetCollectorId = "collector-0";

    [System.Serializable]
    public class SensorEntry
    {
        public string collector_id;
        public string installation_id;
        public string sensor_id;
        public List<string> zone_id;
    }

    [System.Serializable]
    public class Payload
    {
        public List<SensorEntry> sensor_entries;
    }

    [System.Serializable]
    public class SensorResponse
    {
        public Payload payload;
        public int status_code;
        public string success;
    }

    private void Awake()
    {
        _tab = GetComponent<Tab>();
        if (_tab != null)
        {
            _tab.OnTabStateChanged += OnTabStateChange;
        }
    }

    private void OnTabStateChange(bool isActive)
    {
        if (isActive)
        {
            StartCoroutine(SendPostRequest("http://129.244.254.218:80/api/search/active/sensors", "dashboard", "e58b69ad62f34002a344af2e6a7a19bd"));
        }
    }

    public void ReloadZones()
    {
        StartCoroutine(SendPostRequest("http://129.244.254.218:80/api/search/active/sensors", "dashboard", "e58b69ad62f34002a344af2e6a7a19bd"));
    }

    private IEnumerator SendPostRequest(string url, string username, string password)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            // Add Basic Authentication header
            string authorization = username + ":" + password;
            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(authorization);
            string base64Authorization = System.Convert.ToBase64String(bytes);
            www.SetRequestHeader("Authorization", "Basic " + base64Authorization);

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + www.error);
            }
            else
            {
                string jsonResponse = www.downloadHandler.text;
                SensorResponse sensorData = JsonUtility.FromJson<SensorResponse>(jsonResponse);

                // Extract and display zones
                DisplayZones(sensorData.payload.sensor_entries);
            }
        }
    }

    private void HandleZoneClick(GameObject clickedZone)
    {
        TextMeshProUGUI zoneText = clickedZone.GetComponentInChildren<TextMeshProUGUI>();
        if (zoneText != null)
        {
            Debug.Log("Zone clicked: " + zoneText.text);
            foreach (Transform child in zoneParent)
            {
                child.gameObject.SetActive(false);
            }
            reactivate.SetActive(true);
            readingTypesHandlerScript.ActivateLoadingPrefab();
            readingTypesHandlerScript.ResetUniqueReadingTypes();
            /*readingTypesHandlerScript.ClearExistingReadingDisplays();*/

            socketConnectionScript.InitializeSocketConnection(zoneText.text);
            backButton.SetActive(true);
            Debug.Log(loadingPrefab);
            loadingPrefab.SetActive(true);
        }
    }

    private void DisplayZones(List<SensorEntry> sensorEntries)
    {

        
        // Clear previous zones (if any)
        foreach (Transform child in zoneParent)
        {
            if (child.gameObject != zoneDisplay) // Ensure you don't destroy the original zoneDisplay
            {
                Destroy(child.gameObject);
            }
        }

        // Create a HashSet to ensure unique zones
        HashSet<string> uniqueZones = new HashSet<string>();

        foreach (var entry in sensorEntries)
        {
            // Only process entries with the specified collector_id
            if (entry.collector_id == targetCollectorId)
            {
                foreach (var zone in entry.zone_id)
                {
                    uniqueZones.Add(zone);
                }
            }
        }

        // Log the zones
        Debug.Log("Zones to be displayed: " + string.Join(", ", uniqueZones));

        // Instantiate a new tab for each unique zone
        /*zoneDisplay.SetActive(false);*/
        foreach (var zone in uniqueZones)
        {
            GameObject zoneTab = Instantiate(zonePrefab, zoneParent);
            TextMeshProUGUI zoneText = zoneTab.GetComponentInChildren<TextMeshProUGUI>();
            if (zoneText != null)
            {
                zoneText.text = zone;
            }

            Button zoneButton = zoneTab.GetComponent<Button>();
            zoneButton.onClick.AddListener(() => HandleZoneClick(zoneTab));
        }

        TextMeshProUGUI loadingText = zoneDisplay.GetComponentInChildren<TextMeshProUGUI>();
        if (loadingText != null && loadingText.text == "Loading...")
        {
            zoneDisplay.SetActive(false);
        }
    }





    private void OnDestroy()
    {
        if (_tab != null)
        {
            _tab.OnTabStateChanged -= OnTabStateChange;
        }
    }
}
