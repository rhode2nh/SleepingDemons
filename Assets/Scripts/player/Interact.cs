using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    [SerializeField]
    private bool _isHolding;

    public InputRaycast _inputRaycast;

    public void InteractWith(bool isPressed) {
        PlayerManager.instance.isHolding = isPressed;
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
