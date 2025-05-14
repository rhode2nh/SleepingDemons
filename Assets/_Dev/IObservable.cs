using UnityEngine;

public interface IObservable
{
    float RollChance { get; }
    void OnRollSuccess();
    bool IsWithinRange(GameObject other);
    bool IsLookingAt(GameObject other);
}
