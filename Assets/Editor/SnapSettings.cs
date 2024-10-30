using System.Collections;
using System.Collections.Generic;
using PlasticPipe.PlasticProtocol.Messages;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class SnapSettings : EditorWindow
{
    // Grid Values
    private Vector3 baseGridValue = new Vector3();
    private int gridGranularity = 0;
    
    // Increment Values
    private Vector3 _baseIncrementValue = new Vector3();
    private int _incrementGranularity = 0;
    
    private string[] divisions = new string[] { "1", "1/2", "1/4", "1/8", "1/16" };

    [MenuItem("Window/Snap Settings")]
    public static void ShowMyEditor()
    {
        GetWindow<SnapSettings>();
    }

    public void Awake()
    {
        baseGridValue = EditorSnapSettings.gridSize;
        _baseIncrementValue = EditorSnapSettings.move;
    }

    public void OnGUI()
    {
        GUILayout.Label("Grid Settings", EditorStyles.boldLabel);
        GUILayout.Label($"Current Value: {EditorSnapSettings.gridSize}");
        baseGridValue = EditorGUILayout.Vector3Field("Base Grid Value", baseGridValue);
        gridGranularity = GUILayout.SelectionGrid(gridGranularity, divisions, 1, EditorStyles.radioButton);
        var gridValue = CalculateMeasurements(baseGridValue, gridGranularity);
        GUILayout.Label($"Grid Snap Value: {gridValue}");

        GUILayout.Label("Snap Settings", EditorStyles.boldLabel);
        GUILayout.Label($"Current Value: {EditorSnapSettings.move}");
        _baseIncrementValue = EditorGUILayout.Vector3Field("Base Increment Value", _baseIncrementValue);
        _incrementGranularity = GUILayout.SelectionGrid(_incrementGranularity, divisions, 1, EditorStyles.radioButton);
        var incrementValue = CalculateMeasurements(_baseIncrementValue, _incrementGranularity);
        GUILayout.Label($"Increment Snap Value: {incrementValue}");
        
        GUILayout.Label("Rotation Snap Settings", EditorStyles.boldLabel);
        EditorSnapSettings.rotate = EditorGUILayout.FloatField("Rotation Value", EditorSnapSettings.rotate);
        
        EditorSnapSettings.move = incrementValue;
        EditorSnapSettings.gridSize = gridValue;
    }

    Vector3 CalculateMeasurements(Vector3 snapValue, int granularity)
    {
        if (snapValue.x <= 0.0f) return new Vector3(1.0f, 1.0f, 1.0f);
        if (snapValue.y <= 0.0f) return new Vector3(1.0f, 1.0f, 1.0f);
        if (snapValue.z <= 0.0f) return new Vector3(1.0f, 1.0f, 1.0f);
        if (granularity == 0) return snapValue;

        var x = snapValue.x / Mathf.Pow(2, ((float)granularity));
        var y = snapValue.y / Mathf.Pow(2, ((float)granularity));
        var z = snapValue.z / Mathf.Pow(2, ((float)granularity));
        return new Vector3(x, y, z);
    }
}
