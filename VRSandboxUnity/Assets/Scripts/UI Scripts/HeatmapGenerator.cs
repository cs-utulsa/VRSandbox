using UnityEngine;
using System.Collections;

[System.Serializable]
public class RandomSensorData
{
    public Vector3 position; // Position of the sensor reading in Unity world coordinates
    public float intensity;  // Intensity of the reading (e.g., proximity value)
}

public class HeatmapGenerator : MonoBehaviour
{
    public int textureSize = 512;
    public Color hotColor = Color.red;
    public Color coldColor = Color.blue;
    public float maxIntensity = 1.0f;

    private Texture2D heatmapTexture;
    private Mesh mesh;

    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        heatmapTexture = new Texture2D(textureSize, textureSize);
        ClearTexture();
        GetComponent<Renderer>().material.mainTexture = heatmapTexture;
        StartCoroutine(GenerateHeatmapPointRoutine());
    }

    Vector2 WorldToUVPosition(Vector3 worldPosition)
    {
        RaycastHit hit;
        if (Physics.Raycast(worldPosition + Vector3.up * 10f, Vector3.down, out hit))
        {
            if (hit.transform == transform)
            {
                return hit.textureCoord;
            }
        }
        return Vector2.zero; // Default if not found
    }

    void ClearTexture()
    {

        Color[] clearColors = new Color[textureSize * textureSize];
        for (int i = 0; i < clearColors.Length; i++)
        {
            clearColors[i] = Color.clear;
        }
        heatmapTexture.SetPixels(clearColors);
        heatmapTexture.Apply();
    }

/*    IEnumerator GenerateHeatmapPointRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f); // Wait for 5 seconds

            Vector2 centerUVPosition = new Vector2(0.5f, 0.5f); // Directly set the center of the UV space

            SensorData data = new SensorData
            {
                position = new Vector3(centerUVPosition.x * textureSize, centerUVPosition.y * textureSize, 0),
                intensity = Random.Range(0.1f, maxIntensity)
            };

            DrawHeatPoint(data);
            heatmapTexture.Apply();
        }
    }*/

    IEnumerator GenerateHeatmapPointRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f); // Wait for 5 seconds

            Vector2 randomUVPosition = new Vector2(Random.value, Random.value); // Random UV coordinates between 0 and 1

            RandomSensorData data = new RandomSensorData
            {
                position = new Vector3(randomUVPosition.x * textureSize, randomUVPosition.y * textureSize, 0),
                intensity = Random.Range(0.1f, maxIntensity)
            };

            DrawHeatPoint(data);
            heatmapTexture.Apply();
        }
    }



    void DrawHeatPoint(RandomSensorData data)
    {
        int radius = Mathf.FloorToInt(textureSize * 0.05f); // Decreased the radius size

        for (int y = -radius; y <= radius; y++)
        {
            for (int x = -radius; x <= radius; x++)
            {
                if (x * x + y * y <= radius * radius) // Inside the circle
                {
                    float distanceToCenter = Mathf.Sqrt(x * x + y * y) / radius;
                    Color color = Color.Lerp(hotColor, coldColor, distanceToCenter); // Gradient from red to green

                    int texX = Mathf.Clamp((int)data.position.x + x, 0, textureSize - 1);
                    int texY = Mathf.Clamp((int)data.position.y + y, 0, textureSize - 1);
                    Color currentColor = heatmapTexture.GetPixel(texX, texY);
                    heatmapTexture.SetPixel(texX, texY, Color.Lerp(currentColor, color, 0.5f)); // Blend colors
                }
            }
        }
    }
}

/*using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SensorData
{
    public Vector3 position; // Position of the sensor reading in Unity world coordinates
    public float intensity;  // Intensity of the reading (e.g., proximity value)
}

[System.Serializable]
public class Sensor
{
    public Vector3 position; // Position of the sensor in the room

    public float GenerateDistance()
    {
        // Simulate a random distance within the room's dimensions
        return Random.Range(0f, Mathf.Max(6.7f, 5.42f));
    }
}

public class HeatmapGenerator : MonoBehaviour
{
    public int textureSize = 512;
    public Color hotColor = Color.red;
    public Color coldColor = Color.green;
    public float maxIntensity = 1.0f;
    public List<Sensor> sensors = new List<Sensor>();

    private Texture2D heatmapTexture;
    private Mesh mesh;

    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        heatmapTexture = new Texture2D(textureSize, textureSize);
        ClearTexture();
        GetComponent<Renderer>().material.mainTexture = heatmapTexture;
        StartCoroutine(GenerateHeatmapPointRoutine());
    }

    Vector2 WorldToUVPosition(Vector3 worldPosition)
    {
        RaycastHit hit;
        if (Physics.Raycast(worldPosition + Vector3.up * 10f, Vector3.down, out hit))
        {
            if (hit.transform == transform)
            {
                return hit.textureCoord;
            }
        }
        return Vector2.zero; // Default if not found
    }

    void ClearTexture()
    {
        Color[] clearColors = new Color[textureSize * textureSize];
        for (int i = 0; i < clearColors.Length; i++)
        {
            clearColors[i] = Color.clear;
        }
        heatmapTexture.SetPixels(clearColors);
        heatmapTexture.Apply();
    }

    IEnumerator GenerateHeatmapPointRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f); // Wait for 0.5 seconds

            foreach (Sensor sensor in sensors)
            {
                float distance = sensor.GenerateDistance();
                Vector2 uvPosition = new Vector2(sensor.position.x / 6.7f, sensor.position.z / 5.42f); // Normalize sensor position to UV coordinates

                SensorData data = new SensorData
                {
                    position = new Vector3(uvPosition.x * textureSize, uvPosition.y * textureSize, 0),
                    intensity = distance / Mathf.Max(6.7f, 5.42f) // Normalize distance to intensity
                };

                Debug.Log($"Generating heatmap at UV: {uvPosition} | World Position: {sensor.position} | Distance: {distance}");

                ClearTexture();
                DrawHeatPoint(data);
                heatmapTexture.Apply();

                Debug.Log("Heatmap displayed.");
            }
        }
    }

    void DrawHeatPoint(SensorData data)
    {
        int radius = Mathf.FloorToInt(textureSize * 0.05f); // Adjust the radius size

        for (int y = -radius; y <= radius; y++)
        {
            for (int x = -radius; x <= radius; x++)
            {
                if (x * x + y * y <= radius * radius) // Inside the circle
                {
                    float distanceToCenter = Mathf.Sqrt(x * x + y * y) / radius;
                    Color color = Color.Lerp(hotColor, coldColor, distanceToCenter); // Gradient from red to green

                    int texX = Mathf.Clamp((int)data.position.x + x, 0, textureSize - 1);
                    int texY = Mathf.Clamp((int)data.position.y + y, 0, textureSize - 1);
                    Color currentColor = heatmapTexture.GetPixel(texX, texY);
                    heatmapTexture.SetPixel(texX, texY, Color.Lerp(currentColor, color, 0.5f)); // Blend colors
                }
            }
        }
    }
}
*/