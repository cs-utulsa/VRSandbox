using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BackButtonHandler : MonoBehaviour
{
    public SocketConnection socketConnectionScript; // Reference to the SocketConnection script
    public GameObject gameObjectToDeactivate; // The GameObject you want to set to inactive
    public GameObject gameObjectToActivate; // The GameObject you want to set to active
    public PostRequestTabSelect postRequestTabSelectScript;
    public ReadingTypesHandler readingTypesHandler;
    public DataLogWindow dataLogWindow;
    public DataGraphWindow dataGraphWindow;
    public SensorManager sensorManager;
    public TextMeshProUGUI logText;
    public TextMeshProUGUI graphText;

    private void Start()
    {
        // Assuming this script is attached to the button
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(HandleBackButton);
    }

    void HandleBackButton()
    {
        // Unsubscribe from the current zone
        string currentReadingType = readingTypesHandler.GetCurrentSelectedReadingType();
        socketConnectionScript.UnsubscribeFromServices(currentReadingType); // Assuming the method to unsubscribe is named "UnsubscribeFromServices"

        // Set the GameObjects active/inactive
        gameObjectToDeactivate.SetActive(false);
        gameObjectToActivate.SetActive(true);
        readingTypesHandler.ClearExistingReadingDisplays();
        readingTypesHandler.ResetReadingTypeTabs();
        dataLogWindow.ClearData();
        dataGraphWindow.ClearGraph();
        sensorManager.ClearData();
        logText.text = $"";
        graphText.text = $"";

        if (postRequestTabSelectScript != null)
        {
            postRequestTabSelectScript.ReloadZones();
        }
    }
}
