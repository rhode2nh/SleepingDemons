using UnityEngine;

public abstract class Interactable : MonoBehaviour, IInteractable
{
    [SerializeField] private bool _executeOnRelease;

    public virtual void ExecuteInteraction(GameObject other)
    {
        throw new System.NotImplementedException();
    }

    public virtual bool ExecuteOnRelease()
    {
        return _executeOnRelease;
    }
}
