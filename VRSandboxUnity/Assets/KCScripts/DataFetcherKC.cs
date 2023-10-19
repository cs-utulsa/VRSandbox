using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class DataFetcher : MonoBehaviour
{
    public delegate void DataFetchedDelegate(SensorResponse sensorData);
    public event DataFetchedDelegate OnDataFetched;

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

    public void FetchData()
    {
        StartCoroutine(SendGetRequest("http://129.244.254.218:80/api/search/active/sensors", "dashboard", "e58b69ad62f34002a344af2e6a7a19bd"));
    }

    private IEnumerator SendGetRequest(string url, string username, string password)
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
                Debug.Log(jsonResponse);
                SensorResponse sensorData = JsonUtility.FromJson<SensorResponse>(jsonResponse);
                OnDataFetched?.Invoke(sensorData);
            }
        }
    }
}
