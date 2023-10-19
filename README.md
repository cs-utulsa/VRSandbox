Understood! I'll adjust the README to reflect that the heatmap prefab comes with the `Heatmap.cs` script already attached.

---

# Heatmap Visualization from CSV in Unity

This package allows you to create heatmaps within Unity based on data from a CSV file. This system reads data sequentially from the CSV and generates a heatmap accordingly. Each zone controller reads from a specific column of the CSV file, represented by the `nodeIndex`.

## Table of Contents
1. Overview
2. Installation
3. Usage
    1. HeatmapZoneController
    2. HeatmapDataManagerCSV
    3. HeatmapDataController
    4. Using the Prefab with Heatmap.cs
4. Additional Notes

## Overview
There are three main scripts in this system:
1. `HeatmapZoneController`: Handles individual heatmap zones.
2. `HeatmapDataManagerCSV`: Manages CSV data and provides methods to fetch data from it.
3. `HeatmapDataController`: Controls the data broadcast frequency and invokes updates.

## Installation
1. Import the provided scripts into your Unity project.
2. If you don't have a CSV file, prepare one with your data.
3. Ensure you have the provided heatmap prefab with the `Heatmap.cs` script attached.

## Usage

### HeatmapZoneController
This script is responsible for controlling individual heatmap zones based on the data from a specific column in the CSV.
- Attach this script to a GameObject.
- Set the `nodeIndex` to the desired column of the CSV data.

### HeatmapDataManagerCSV
This is a singleton class responsible for managing the CSV data. You only need one instance of this in your scene. 
- Drag and drop your CSV file onto the `csvAsset` field in the inspector.

### HeatmapDataController
This script broadcasts the CSV data at a specified frequency to all heatmap zones.
- Attach it to a GameObject.
- Drag and drop your CSV file onto the `csvAsset` field in the inspector.
- This script broadcasts data every 1/3 of a second by default.


### Using the Prefab with Heatmap.cs

1. Drag and drop the provided heatmap prefab into your scene.
2. This prefab already has the `Heatmap.cs` script attached, which manages the visualization and rendering of the heatmap based on the data.
3. Drag and drop the `Heatmap.cs` script from the prefab into its own heatmap parameter in the inspector.
4. Add the `HeatmapZoneController` script to this prefab.
5. Set the `nodeIndex` on the `HeatmapZoneController` to the desired column from your CSV.


## Additional Notes
- The system uses categories to determine significant changes in data. You can adjust these categories or thresholds if required.
- Ensure that your CSV is well-formatted with the appropriate number of columns.
- The system is designed to loop over the CSV data once it reaches the end.

---

That's all you need to get started with heatmap visualization from CSV in Unity. If you run into any issues, message David Montgomery on teams.

---

