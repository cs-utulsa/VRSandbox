using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataGraphWindow : MonoBehaviour
{
    public SimplestPlot PlotReference;

    [Header("Labels")]
    public string PlotTilte;
    public string XAxisLabel;
    public string YAxisLabel;
    [Header("Colors")]
    public Color BGColor;
    public Color TextColor;
    public Color PlotColor;
    public Vector2 PlotResolution;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Initialize()
    {
        PlotReference.Initialize();

        PlotReference.SetResolution(PlotResolution);
        PlotReference.BackGroundColor = BGColor;
        PlotReference.TextColor = TextColor;

        PlotReference.SeriesPlotY.Add(new SimplestPlot.SeriesClass());
        PlotReference.SeriesPlotY[0].MyColor = PlotColor;

        PlotReference.PlotTitle = PlotTilte;
        PlotReference.XAxisLabel = XAxisLabel;
        PlotReference.YAxisLabel = YAxisLabel;
        PlotReference.SetLabels();
    }

    public void DisplayGraph(float[] xValues, float[] yValues)
    {
        PlotReference.SeriesPlotX = xValues;

        PlotReference.SeriesPlotY[0].YValues = yValues;
        PlotReference.UpdatePlot();
    }
}
