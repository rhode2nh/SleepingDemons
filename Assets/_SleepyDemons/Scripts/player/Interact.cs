using System;
using UnityEngine;

public class Interact : MonoBehaviour
{
    public InputRaycast _inputRaycast;

    private void Start()
    {
        InputManager.Instance.OnInteract += InteractWith;
    }
    
    private void InteractWith(bool isPressed) {
        if (!_inputRaycast.isHitting) return;
        var interactable = _inputRaycast.hit.collider.GetComponent<IInteractable>();
        if (interactable == null) return;
        
        var executeOnRelease = interactable.ExecuteOnRelease();
        if (isPressed)
        {
            interactable?.ExecuteInteraction(gameObject);
        }
        
        if (executeOnRelease && !isPressed)
        {
            interactable?.ExecuteInteraction(gameObject);
        }
    }
}
