using UnityEngine;
using System.Globalization;

public class HeatmapDataManagerCSV : MonoBehaviour
{
    public static HeatmapDataManagerCSV Instance { get; private set; }

    private string[] lines;
    private int currentLineIndex = 1;

    public TextAsset csvAsset; // Drag and drop your CSV asset here in the inspector 

    private void Awake()
    {
        Debug.Log("HeatmapDataManagerCSV Awake called.");
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (csvAsset != null)
        {
            lines = csvAsset.text.Split(new char[] { '\n' });
        }
        else
        {
            Debug.LogError("CSV file not found in Resources.");
        }
    }

    public float GetSumOfRow()
    {
        if (currentLineIndex >= lines.Length)
        {
            return 0f; // Return 0 if out of bounds, or handle as needed
        }

        string[] values = lines[currentLineIndex].Split(',');
        float sum = 0;

        foreach (string value in values)
        {
            sum += float.Parse(value, CultureInfo.InvariantCulture);
        }

        return sum;
    }

    public float GetNodeValue(int nodeIndex)
    {
        if (currentLineIndex >= lines.Length)
        {
            return 0f; // Return 0 if out of bounds, or handle as needed
        }

        string[] values = lines[currentLineIndex].Split(',');

        if (nodeIndex >= 0 && nodeIndex < values.Length)
        {
            return float.Parse(values[nodeIndex], CultureInfo.InvariantCulture);
        }
        else
        {
            return 0f; // Return 0 if nodeIndex is out of bounds, or handle as needed
        }
    }

    public void MoveToNextRow()
    {
        currentLineIndex++;
        if (currentLineIndex >= lines.Length)
        {
            currentLineIndex = 1; // Reset to the first data line to start over
        }
    }
}
