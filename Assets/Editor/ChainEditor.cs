using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

[CustomEditor(typeof(ChainTest))]
public class ChainEditor : Editor
{
    private SerializedProperty segment;
    private int numSegments;
    private List<GameObject> segments = new();
    private Rigidbody chainHead;
    private float distanceBetweenSegments;
    
    private float pullDistance;
    private Renderer _renderer;
    private Light lightBulb;
    private float emissiveIntensity;
    private Transform transform;
    
    public override void OnInspectorGUI()
    {
        GUILayout.Label("Chain Settings");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("segment"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("numSegments"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("chainHead"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("distanceBetweenSegments"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("pullDistance"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_renderer"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("lightBulb"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("emissiveIntensity"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("lampSwitchOn"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("lampSwitchOff"));
        
        if (GUILayout.Button("Reset Chain"))
        {
            var chainTest = target as ChainTest;
            chainTest.ResetChain();
        }
        if (GUILayout.Button("Generate Chain"))
        {
            var chainTest = target as ChainTest;
            chainTest.GenerateChain();
        }

        serializedObject.ApplyModifiedProperties();
    }
}
