using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RebuildDatabases : MonoBehaviour
{
    [MenuItem("Database/Rebuild Databases")]
    public static void RebuildDBs() {
        // Grab the GUIDs
        var itemGUIDs = AssetDatabase.FindAssets("t:Item");
        var itemDatabaseGuid = AssetDatabase.FindAssets("ItemDatabase t:ItemDatabase")[0];
        // string prefabDatabaseGUID = AssetDatabase.FindAssets("PrefabItemDatabase t:PrefabDatabase")[0];
        
        ItemDatabase itemDatabase = (ItemDatabase)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(itemDatabaseGuid), typeof(ItemDatabase));
        itemDatabase.items.Clear();
        
        foreach (var t in itemGUIDs)
        {
            var path = AssetDatabase.GUIDToAssetPath(t);
            var item = (Item)AssetDatabase.LoadAssetAtPath(path, typeof(Item));
            itemDatabase.items.Add(item);
        }

        // PrefabDatabase prefabDatabase = (PrefabDatabase)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(prefabDatabaseGUID), typeof(PrefabDatabase));
        // prefabDatabase.prefabs.Clear();
        // itemGUIDs = AssetDatabase.FindAssets("t:prefab", new [] {"Assets/1 - Prefabs/World/Entities/Items"});
        // for (int i = 0; i < itemGUIDs.Length; i++) {
        //     string path = AssetDatabase.GUIDToAssetPath(itemGUIDs[i]);
        //     GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath(path, typeof(GameObject));
        //     if (prefab.GetComponent<WorldItem>() != null) {
                // prefabDatabase.prefabs.Add(prefab);
            // }
        // }
        AssetDatabase.SaveAssets();
    } 

}
