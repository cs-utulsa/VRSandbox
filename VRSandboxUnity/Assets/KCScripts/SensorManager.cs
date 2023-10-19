using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorManager : MonoBehaviour
{
    public SensorData sensorDataPrefab;
    public SensorDataWindow sensorDataWindowPrefab;
    public DataLogWindow dataLogWindow;
    public DataGraphWindow dataGraphWindow;

    private Dictionary<string, SensorData> sensorDataDictionary = new Dictionary<string, SensorData>();

    public void HandleNewSensorData(string sensorType, string dataValue, string readingUnit)
    {
        MainThreadDispatcher.Instance.Enqueue(() =>
        {
            SensorData currentSensorData;
            if (!sensorDataDictionary.TryGetValue(sensorType, out currentSensorData))
            {
                currentSensorData = Instantiate(sensorDataPrefab);
                currentSensorData.SensorType = sensorType;
                sensorDataDictionary[sensorType] = currentSensorData;

                SensorDataWindow newWindow = Instantiate(sensorDataWindowPrefab);
                newWindow.CurrentSensorData = currentSensorData;
            }

            string combinedValue = $"{dataValue} {readingUnit}";
            currentSensorData.SensorDataValues.Add(combinedValue);

            if (dataLogWindow != null)
            {
                dataLogWindow.DisplayText(currentSensorData.SensorDataValues.ToArray());
            }

            if (dataGraphWindow != null)
            {
                float[] xValues = new float[currentSensorData.SensorDataValues.Count];
                float[] yValues = new float[currentSensorData.SensorDataValues.Count];

                for (int i = 0; i < currentSensorData.SensorDataValues.Count; i++)
                {
                    xValues[i] = i; // Assuming x-values are just a sequence
                    yValues[i] = float.Parse(currentSensorData.SensorDataValues[i].Split(' ')[0]); // Convert the string data (without unit) to float
                }

                dataGraphWindow.DisplayGraph(xValues, yValues);
            }
        });
    }

    public void ClearData()
    {
        // Clear the sensor data dictionary
        sensorDataDictionary.Clear();

        // Optionally, you could also destroy the game objects associated with the sensor data,
        // if they are no longer needed.
        // foreach (var sensorData in sensorDataDictionary.Values)
        // {
        //     Destroy(sensorData.gameObject);
        // }
    }

}

