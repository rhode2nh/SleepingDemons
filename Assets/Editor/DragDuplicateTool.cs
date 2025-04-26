using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class DragDuplicateTool
{
    private static Vector3 lastPosition;
    private static Vector3 spawnPosition;
    private static GameObject lastObject;
    private static Vector3 distanceTravelled;

    static DragDuplicateTool()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private static void OnSceneGUI(SceneView sceneView)
    {
        Event e = Event.current;
        if (!e.alt)
        {
            // Selection.activeGameObject = null;
            lastObject = null;
            lastPosition = new Vector3();
            distanceTravelled = new Vector3();
            spawnPosition = new Vector3();
            return;
        }
        
        if (Selection.activeGameObject != null)
        {
            var collider = Selection.activeGameObject.GetComponent<Collider>();
            if (collider == null) return;
            
            GameObject selected = Selection.activeGameObject;

            // Only check position if the object has changed or is being moved
            if (selected != lastObject)
            {
                lastObject = selected;
                lastPosition = selected.transform.position;
                spawnPosition = lastPosition;
            }
            else if (selected.transform.position != lastPosition && e.alt && e.type == EventType.MouseDrag)
            {
                distanceTravelled += lastPosition - selected.transform.position;

                if (math.abs(distanceTravelled.x) >= collider.bounds.size.x
                    || math.abs(distanceTravelled.y) >= collider.bounds.size.y
                    || math.abs(distanceTravelled.z) >= collider.bounds.size.z)
                {
                    // Duplicate the object
                    GameObject clone = Object.Instantiate(selected);
                    Undo.RegisterCreatedObjectUndo(clone, "Duplicate on Move");

                    // Move the clone back to original position, so the moved one stays moved
                    clone.transform.position = spawnPosition;
                    spawnPosition = selected.transform.position;
                    distanceTravelled = new Vector3();
                }
                
                // Update last position for next move
                lastPosition = selected.transform.position;
            }
        }
    }
}

