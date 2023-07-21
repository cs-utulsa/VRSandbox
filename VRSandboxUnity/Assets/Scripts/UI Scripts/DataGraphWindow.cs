using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataGraphWindow : MonoBehaviour
{
    public SimplestPlot PlotReference;
    public Color BGColor;
    public Color TextColor;
    public Color PlotColor;
    public Vector2 PlotResolution;

    // Start is called before the first frame update
    void Start()
    {
        PlotReference.SetResolution(PlotResolution);
        PlotReference.BackGroundColor = BGColor;
        PlotReference.TextColor = TextColor;

        PlotReference.SeriesPlotY.Add(new SimplestPlot.SeriesClass());
        PlotReference.SeriesPlotY[0].MyColor = PlotColor;
    }

    public void DisplayGraph(float[] xValues, float[] yValues)
    {
        PlotReference.SeriesPlotX = xValues;
        PlotReference.SeriesPlotY[0].YValues = yValues;
        PlotReference.UpdatePlot();
    }
}
