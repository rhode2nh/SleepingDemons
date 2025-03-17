using UnityEngine;
using UnityEngine.UI;

public interface IInteractable
{
    public Sprite InteractableIcon { get; }
    void ExecuteInteraction(GameObject other);
    bool ExecuteOnRelease();
}
