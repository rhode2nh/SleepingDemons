using UnityEngine;

public abstract class DropdownOption : ScriptableObject
{
    [field: SerializeField] public string Label { get; private set; }
    public abstract void Process(Slot slot);
}
