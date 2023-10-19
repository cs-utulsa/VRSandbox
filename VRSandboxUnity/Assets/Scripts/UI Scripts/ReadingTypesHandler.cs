using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

public class ReadingTypesHandler : MonoBehaviour
{
    public GameObject readingTypePrefab; // Drag and drop a prefab with a TextMeshProUGUI component in the inspector
    public Transform readingTypeParent; // Parent transform under which the reading types will be instantiated

    public SocketConnection socketConnectionScript; // Reference to the SocketConnection script
    public GameObject loadingPrefab;

    public GameObject sensorWindow; // Drag and drop the SensorWindow GameObject in the inspector

    public GameObject readingTypeDisplay; // Drag and drop the GameObject containing the loading text in the inspector


    private HashSet<string> uniqueReadingTypes = new HashSet<string>();
    private Dictionary<string, GameObject> readingTypeTabs = new Dictionary<string, GameObject>();  // New dictionary to keep track of tabs
    private string currentSelectedReadingType = null;

    public TextMeshProUGUI logText;
    public TextMeshProUGUI graphText;

    public void ActivateLoadingPrefab()
    {
        if (loadingPrefab != null)
        {

            loadingPrefab.SetActive(true);
        }
        else
        {
            Debug.LogError("Loading Prefab is not assigned in ReadingTypesHandler.");
        }
    }

    public string GetCurrentSelectedReadingType()
    {
        return currentSelectedReadingType;
    }

    private void HandleSensorClick(GameObject clickedSensor)
    {
        TextMeshProUGUI sensorText = clickedSensor.GetComponentInChildren<TextMeshProUGUI>();
        if (sensorText != null)
        {
            currentSelectedReadingType = sensorText.text;
            

            // Unsubscribe from the current zone
            socketConnectionScript.UnsubscribeFromServices();

            // Subscribe to the same zone with the selected reading type as a filter
            socketConnectionScript.SubscribeToServices(currentSelectedReadingType);

            if (sensorWindow != null)
            {
                sensorWindow.SetActive(true);
                if (logText != null)
                {
                    logText.text = $"{currentSelectedReadingType} Log";
                }
                if (graphText != null)
                {
                    graphText.text = $"{currentSelectedReadingType} Graph";
                }
            }
            else
            {
                Debug.LogError("SensorWindow GameObject is not assigned in ReadingTypesHandler.");
            }
        }
    }

    public void ResetUniqueReadingTypes()
    {
        Debug.Log("CLEARED UNIQUE READING TYPES: " + uniqueReadingTypes);
        uniqueReadingTypes.Clear();
    }


    public void ClearExistingReadingDisplays()
    {
        foreach (Transform child in readingTypeParent)
        {
            if (child.gameObject.name != "Reading_Type Tab")
            {
                Destroy(child.gameObject);
            }
        }
    }

    public void AddAndDisplayReadingType(string newReadingType)
    {
        // Add the new reading type to the HashSet
        if (uniqueReadingTypes.Contains(newReadingType))
        {
            Debug.LogWarning("Reading type already exists: " + newReadingType);
            return;  // Exit the method if the reading type already exists
        }

        readingTypeDisplay.SetActive(true);
        uniqueReadingTypes.Add(newReadingType);

        Debug.Log("Adding new reading type: " + newReadingType);

        MainThreadDispatcher.Instance.Enqueue(() =>
        {
            foreach (var sensor in uniqueReadingTypes)
            {
                // Check if a tab already exists for this reading type
                if (!readingTypeTabs.ContainsKey(sensor))
                {
                    // Create a new tab for the reading type
                    GameObject readingTypeTab = Instantiate(readingTypePrefab, readingTypeParent);
                    TextMeshProUGUI readingTypeText = readingTypeTab.GetComponentInChildren<TextMeshProUGUI>();
                    if (readingTypeText != null)
                    {
                        readingTypeText.text = sensor;
                    }

                    Button sensorButton = readingTypeTab.GetComponent<Button>();
                    sensorButton.onClick.AddListener(() => HandleSensorClick(readingTypeTab));

                    // Add the tab to the dictionary
                    readingTypeTabs[sensor] = readingTypeTab;
                }
            }

            // Hide the "Loading..." text if it's currently displayed
            TextMeshProUGUI loadingText = readingTypeDisplay.GetComponentInChildren<TextMeshProUGUI>();
            if (loadingText != null && loadingText.text == "Loading...")
            {
                readingTypeDisplay.SetActive(false);
            }
        });
    }

    public void ResetReadingTypeTabs()
    {
        // Clear the dictionary and destroy all tab GameObjects
        foreach (var tab in readingTypeTabs.Values)
        {
            Destroy(tab);
        }
        readingTypeTabs.Clear();
    }


    public bool IsNewReadingType(string readingType)
    {
        return !uniqueReadingTypes.Contains(readingType);
    }
}
