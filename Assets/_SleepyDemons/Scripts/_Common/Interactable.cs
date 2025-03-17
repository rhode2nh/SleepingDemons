using UnityEngine;
using UnityEngine.UI;

public abstract class Interactable : MonoBehaviour, IInteractable
{
    [Header("Interactable Settings")]
    [Tooltip("Should the action be executed only on press or when the input is pressed and released?")]
    [SerializeField]
    private bool _executeOnRelease;

    [field: SerializeField] public Sprite InteractableIcon { get; private set;  }

    public virtual void ExecuteInteraction(GameObject other)
    {
        throw new System.NotImplementedException();
    }

    public virtual bool ExecuteOnRelease()
    {
        return _executeOnRelease;
    }
}