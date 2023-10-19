using HeatmapVisualization;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Linq;
using UnityEngine;


public class DataReader : MonoBehaviour
{
    #region Settings
    [SerializeField]
    [Tooltip("The directory where the results of your data collection is.")]
    private string dataDirectory;
    [SerializeField]
    [Tooltip("The key of the position tracker to visualize.")]
    private string keyToRead;

    private string fileFilter = "*.xml";
    #endregion


    #region Globals
    //Not cached, because this needs to be run in edit mode.
    private Heatmap OwnHeatmap { get => GetComponent<Heatmap>(); }
    #endregion


    #region Functions
    public void GenerateHeatmap()
    {
        List<Vector3> tracks = LoadTracks();
        Debug.Log(tracks);
        OwnHeatmap.GenerateHeatmap(tracks, 0.6f);
    }


    private List<Vector3> LoadTracks()
    {
        List<AnalyticsData> datas = ReadDataFromFolder();

        List<Vector3> tracks = new List<Vector3>();
        foreach (AnalyticsData data in datas)
        {
            tracks.AddRange(data.tracks[keyToRead].Select((step) => step.pos));
        }

        return tracks;
    }



    private List<AnalyticsData> ReadDataFromFolder()
    {
        List<AnalyticsData> dataObjects = new List<AnalyticsData>();

        foreach (string filePath in Directory.EnumerateFiles(dataDirectory, fileFilter))
        {
            string fileContent = File.ReadAllText(filePath, System.Text.Encoding.Unicode);
            dataObjects.Add(XmlToObject<AnalyticsData>(fileContent));
        }

        return dataObjects;
    }


    private static T XmlToObject<T>(string xml)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(T));
        StringReader stringReader = new StringReader(xml);
        T dataObject = (T)serializer.Deserialize(stringReader);
        stringReader.Close();
        return dataObject;
    }
    #endregion
}
