using System;
using System.Collections;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Networking;


public class AnalyticsManager : MonoBehaviour
{
	#region Settings
	[SerializeField]
	[Tooltip("The directory to save the data in, if export method is write to file.")]
	private string saveDirectory;
	[SerializeField]
	[Tooltip("The address of the server to send the data to, if export method is send to server.")]
	private string serverAddress;
	[SerializeField]
	[Tooltip("Choose the method how to export data.")]
	private ExportMethod exportMethod;
	[SerializeField]
	[Tooltip("Automatically export the collected analytics data.")]
	private bool autoExportEnabled = true;
	[SerializeField]
	[Tooltip("The time between to auto export cycles.")]
	private float autoExportIntervall = 10.0f;

	private string fileExtension = ".xml";
	#endregion


	#region Globals
	static private AnalyticsManager instance = null;
    static public AnalyticsManager Instance
    {
        get
		{
            if (instance == null)
			{
                instance = Instantiate(Resources.Load("SingeltonPrefabs/AnalyticsManager") as GameObject).GetComponent<AnalyticsManager>();
            }
            return instance;
		}
        private set => instance = value;
    }
    private Guid sessionID;
	private AnalyticsData analyticsData;
	private int dataPointCount = 0;
	private float nextAutoExportTime = 0.0f;
	#endregion


	#region Definitions
	private enum ExportMethod { WriteToFile, SendToServer };
	#endregion


	#region Functions
	void Awake()
	{
        DontDestroyOnLoad(gameObject);
        sessionID = Guid.NewGuid();
		ResetAnalyticsData();
	}


	private void Update()
	{
		if (autoExportEnabled && dataPointCount > 0)
		{
			if (Time.time >= nextAutoExportTime)
			{
				ExportData();
				nextAutoExportTime = Time.time + autoExportIntervall;
			}
		}
	}


	/// <summary>
	/// Add a step to the analytics data struct.
	/// </summary>
	/// <param name="step">The step to add.</param>
	public void AddTrackStep(string key, PositionTracker.StepData step)
	{
		analyticsData.addTrackStep(key, step);
		dataPointCount++;
	}


	/// <summary>
	/// Send all accumulated datapoints by the selected export method.
	/// </summary>
	public void ExportData()
	{
		if (dataPointCount == 0)
		{
			return;
		}

		string dataString = ObjectToXml(analyticsData);

		switch (exportMethod)
		{
			case ExportMethod.WriteToFile:
				SaveDataToDirectory(dataString);
				break;
			case ExportMethod.SendToServer:
				SendDataToServer(dataString);
				break;
			default:
				throw(new NotImplementedException("Export method not implemented."));
		}

		ResetAnalyticsData();
		dataPointCount = 0;
	}


	/// <summary>
	/// Check if the Server is available by http get for later use.
	/// </summary>
	/// <param name="callback"></param>
	/// <returns></returns>
	public IEnumerator CheckServerAvailability(Action<bool> callback)
	{
		CheckServerAddress();

		UnityWebRequest www = UnityWebRequest.Get(serverAddress);
		yield return www.SendWebRequest();

		if (www.result != UnityWebRequest.Result.Success)
		{
			//Server is unavailable
			Debug.LogWarning(www.error);
			callback(false);
		}
		else
		{
			//Server is available
			callback(true);
		}
	}


	private void ResetAnalyticsData()
	{
		analyticsData = new AnalyticsData { sessionID = sessionID };
	}


	private static string ObjectToXml(object dataObject)
	{
		StringWriter stringWriter = new StringWriter();
		XmlSerializer xmlSerializer = new XmlSerializer(dataObject.GetType());
		xmlSerializer.Serialize(stringWriter, dataObject);
		stringWriter.Close();
		return stringWriter.ToString();
	}


	private void SaveDataToDirectory(string data)
	{
		CheckSaveDirectory();
		string path = Path.Combine(saveDirectory, DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff") + fileExtension);
		File.WriteAllText(path, data, System.Text.Encoding.Unicode);
		Debug.Log($"Saved analytics data to \"{path}\".");
	}


	private void SendDataToServer(string data)
	{
		CheckServerAddress();
		StartCoroutine(SendDataToServerCoroutine(data));
	}


	private IEnumerator SendDataToServerCoroutine(string data)
	{
        WWWForm form = new WWWForm();
        form.AddField("data", data);

        UnityWebRequest www = UnityWebRequest.Post(serverAddress, form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogWarning(www.error);
        }
        else
        {
            Debug.Log($"Analytics data uploaded sucessfully to \"{serverAddress}\".");
        }
	}


	private void CheckSaveDirectory()
	{
		if (saveDirectory == "")
		{
			throw (new ArgumentException("The save directory of the analytics manager is not set.\nTo set, open the prefab within the \"Resources\" folder in the Assets."));
		}
	}


	private void CheckServerAddress()
	{
		if (serverAddress == "")
		{
			throw (new ArgumentException("The server address of the analytics manager is not set.\nTo set, open the prefab within the \"Resources\" folder in the Assets."));
		}
	}
	#endregion
}
