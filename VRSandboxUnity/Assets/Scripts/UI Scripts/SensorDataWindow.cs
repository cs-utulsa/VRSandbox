/*using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class TemperatureSensorWindow : MonoBehaviour
{
    public SensorData TempSensorData;
    public DataLogWindow LogWindow;
    public DataGraphWindow GraphWindow;

    // Start is called before the first frame update
    void Awake()
    {
        TempSensorData.OnDataListUpdated += UpdateDisplays;
        TempSensorData.OnDataValueChanged += UpdateDisplays;
    }

    private void Start()
    {
        if(GraphWindow != null)
        {
            GraphWindow.PlotTitle = "Temperature Graph (C)";
            GraphWindow.XAxisLabel = "";
            GraphWindow.YAxisLabel = "";
            GraphWindow.Initialize();
        }
        
        UpdateDisplays();
    }

    private void UpdateDisplays()
    {
        LogWindow.DisplayText(TempSensorData.SensorDataValues.ToArray());

        int dataCount = TempSensorData.SensorDataValues.Count;

        if (GraphWindow != null)
        {
            float[] timeValues = new float[dataCount];
            float[] tempValues = new float[dataCount];

            for (int i = 0; i < dataCount; i++)
            {
                timeValues[i] = i;
                string line = TempSensorData.SensorDataValues[i];
                string valueString = line.Split(":")[1].Trim();
                string trimmedValueString = Regex.Match(valueString, @"[+-]?\d*\.?\d*").Value;
                float value = float.Parse(trimmedValueString);
                tempValues[i] = value;
            }
            GraphWindow.DisplayGraph(timeValues, tempValues);
        }
    }
    private void UpdateDisplays(int index)
    {
        UpdateDisplays();
    }

    private void OnDestroy()
    {
        TempSensorData.OnDataListUpdated -= UpdateDisplays;
        TempSensorData.OnDataValueChanged -= UpdateDisplays;
    }
}
*/

using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class SensorDataWindow : MonoBehaviour
{
    public SensorData CurrentSensorData;
    public DataLogWindow LogWindow;
    public DataGraphWindow GraphWindow;

    void Awake()
    {
        CurrentSensorData.OnDataListUpdated += UpdateDisplays;
        CurrentSensorData.OnDataValueChanged += UpdateDisplays;
    }

    private void Start()
    {
        Debug.Log("SENSOR DATA WINDOW: " + CurrentSensorData.SensorType);
        if (GraphWindow != null)
        {
            GraphWindow.PlotTitle = $"{CurrentSensorData.SensorType} Graph";
            GraphWindow.XAxisLabel = "";
            GraphWindow.YAxisLabel = "";
            GraphWindow.Initialize();
        }

        UpdateDisplays();
    }

    private void UpdateDisplays()
    {
        LogWindow.DisplayText(CurrentSensorData.SensorDataValues.ToArray());

        int dataCount = CurrentSensorData.SensorDataValues.Count;

        if (GraphWindow != null)
        {
            float[] timeValues = new float[dataCount];
            float[] sensorValues = new float[dataCount];

            for (int i = 0; i < dataCount; i++)
            {
                timeValues[i] = i;
                string line = CurrentSensorData.SensorDataValues[i];
                string valueString = line.Split(":")[1].Trim();
                string trimmedValueString = Regex.Match(valueString, @"[+-]?\d*\.?\d*").Value;
                float value = float.Parse(trimmedValueString);
                sensorValues[i] = value;
            }
            GraphWindow.DisplayGraph(timeValues, sensorValues);
        }
    }
    private void UpdateDisplays(int index)
    {
        UpdateDisplays();
    }

    private void OnDestroy()
    {
        CurrentSensorData.OnDataListUpdated -= UpdateDisplays;
        CurrentSensorData.OnDataValueChanged -= UpdateDisplays;
    }
}
