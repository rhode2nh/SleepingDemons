using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ChainGenerator))]
public class ChainEditor : Editor
{
    public override void OnInspectorGUI()
    {
        GUILayout.Label("Chain Settings");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("segment"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("numSegments"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("chainHead"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("distanceBetweenSegments"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("pullDistance"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("segments"));
        
        if (GUILayout.Button("Reset Chain"))
        {
            var chainTest = target as ChainGenerator;
            chainTest.ResetChain();
        }
        if (GUILayout.Button("Generate Chain"))
        {
            var chainTest = target as ChainGenerator;
            chainTest.GenerateChain();
            EditorUtility.SetDirty(chainTest);
            PrefabUtility.RecordPrefabInstancePropertyModifications(chainTest);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
