using System.Collections.Generic;
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
    private static Vector3 targetDistance;
    private static List<GameObject> duplicatedObjects = new();

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
            duplicatedObjects = new List<GameObject>();
            return;
        }
        
        if (Selection.activeGameObject != null)
        {
            var collider = Selection.activeGameObject.GetComponent<Collider>();
            if (collider == null)
            {
                targetDistance = new Vector3(1f, 1f, 1f);
            }
            else
            {
                targetDistance = collider.bounds.size;
            };
            
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

                if (math.abs(distanceTravelled.x) >= targetDistance.x
                    || (math.abs(distanceTravelled.y) >= targetDistance.y && targetDistance.y != 0)
                    || math.abs(distanceTravelled.z) >= targetDistance.z)
                {
                    // Check if object exists in position based on active selection
                    var objectDestroyed = DeleteObjectInPosition(selected);

                    if (!objectDestroyed)
                    {
                        DuplicateObject(selected);
                    }

                    // Move the clone back to original position, so the moved one stays moved
                    spawnPosition = selected.transform.position;
                    distanceTravelled = new Vector3();
                }
                
                // Update last position for next move
                lastPosition = selected.transform.position;
            }
        }
    }

    private static void DuplicateObject(GameObject selected)
    {
        GameObject duplicate;
        
        if (PrefabUtility.IsPartOfPrefabInstance(selected))
        {
            duplicate = InstantiatePrefab(selected);
            duplicate.transform.SetParent(selected.transform.parent);
            duplicate.transform.localPosition = selected.transform.localPosition + Vector3.right;
            duplicate.transform.localRotation = selected.transform.localRotation;
            duplicate.transform.localScale = selected.transform.localScale;
        }
        else
        {
            duplicate = Object.Instantiate(selected);
            duplicate.transform.SetParent(selected.transform.parent);
            duplicate.transform.position = selected.transform.position + Vector3.right;
            duplicate.transform.rotation = selected.transform.rotation;
            duplicate.transform.localScale = selected.transform.localScale;
            duplicate.name = selected.name;
        
            if (duplicate.transform.childCount > 0)
            {
                for (int i = 0; i < duplicate.transform.childCount; i++)
                {
                    if (!PrefabUtility.IsPartOfPrefabInstance(duplicate.transform.GetChild(i).gameObject))
                    {
                        Debug.Log("This is not part of a prefab asset");
                    }
                    GameObject prefabSource = GetPrefabSource(selected.transform.GetChild(i).gameObject);
                    PrefabUtility.ConnectGameObjectToPrefab(duplicate.transform.GetChild(i).gameObject, prefabSource);
                }
            }
        }
        
        duplicate.transform.position = spawnPosition;
        Undo.RegisterCreatedObjectUndo(duplicate, "Duplicate on Move");
        
       // Unsupported.DuplicateGameObjectsUsingPasteboard();
    }

    private static GameObject InstantiatePrefab(GameObject prefab)
    {
        GameObject go = GetPrefabSource(prefab);
        return PrefabUtility.InstantiatePrefab(go) as GameObject;
    }

    private static GameObject GetPrefabSource(GameObject prefab)
    {
        string assetPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(prefab);
        return AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject)) as GameObject;
    }

    private static bool DeleteObjectInPosition(GameObject selected)
    {
        var all = Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None);
        var selectedPrefab = GetPrefabSource(selected);
        var objectDeleted = false;
        foreach (var other in all)
        {
            if (other.gameObject == selected) continue;
            if (other.transform.position != selected.transform.position) continue;
            if (other.transform.rotation != selected.transform.rotation) continue;
            var otherPrefab = GetPrefabSource(other);
            if (selectedPrefab != otherPrefab) continue;
            
            Undo.DestroyObjectImmediate(other.gameObject);
            objectDeleted = true;
        }

        return objectDeleted;
    }
}

