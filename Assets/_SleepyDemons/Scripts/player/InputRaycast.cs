using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputRaycast : MonoBehaviour
{
    public float maxDistance;
    public bool isHitting;
    public RaycastHit hit;

    private void Start()
    {
        isHitting = false;
    }

    private void FixedUpdate()
    {
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        isHitting = Physics.Raycast(transform.position, fwd, out hit, maxDistance, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);
        Debug.DrawRay(transform.position, fwd * maxDistance, Color.green);
        DisplayHoverText();
    }

    public void DisplayHoverText()
    {
        if (isHitting && !PlayerManager.Instance.IsAiming && !PlayerManager.Instance.IsHolding)
        {
            var interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                CrosshairManager.Instance.ShowInteractIcon(interactable.InteractableIcon);
            }
            else
            {
                CrosshairManager.Instance.HideInteractIcon();
            }
        }
        else
        {
            CrosshairManager.Instance.HideInteractIcon();
        }
    }
}
