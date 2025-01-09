using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Note", menuName = "Items/New Note")]
public class NoteData : Item
{
    [field: SerializeField] public string Name { get; private set; }   
    [field: SerializeField] public string Description { get; private set; }

    public override void Use()
    {
        base.Use();
        Debug.Log("Opening note...");
        Debug.Log("Name: " + Name);
        Debug.Log("Description: " + Description);
    }
}
