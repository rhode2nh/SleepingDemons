using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class SnapRotate
{
    static SnapRotate()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private static void OnSceneGUI(SceneView sceneView)
    {
        Event e = Event.current;
        GameObject selected = Selection.activeGameObject;

        if (e.type == EventType.KeyUp || !e.alt || selected == null) return;
        
        Quaternion selectedRotation = selected.transform.rotation;
        switch (e.keyCode)
        {
            case KeyCode.D:
                Undo.RecordObject(selected.transform, "Rotate 90 degrees");
                selected.transform.Rotate(Vector3.up, 90);
                break;
            case KeyCode.A:
                Undo.RecordObject(selected.transform, "Rotate 90 degrees");
                selected.transform.Rotate(Vector3.up, -90);
                break;
            case KeyCode.W:
                Undo.RecordObject(selected.transform, "Rotate 90 degrees");
                selected.transform.Rotate(Vector3.right, -90);
                break;
            case KeyCode.S:
                Undo.RecordObject(selected.transform, "Rotate 90 degrees");
                selected.transform.Rotate(Vector3.right, 90);
                break;
        }

        if (selected.transform.rotation != selectedRotation)
        {
            EditorUtility.SetDirty(selected);   
        }
    }
}
