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
        isHitting = Physics.Raycast(transform.position, fwd, out hit, maxDistance);
        Debug.DrawRay(transform.position, fwd * maxDistance, Color.green);
        DisplayHoverText();
    }

    public void DisplayHoverText()
    {
        if (isHitting && !PlayerManager.instance.isAiming && !PlayerManager.instance.isHolding)
        {
            if (hit.transform.gameObject.GetComponentInParent<IInteractable>() != null
                || hit.transform.gameObject.GetComponentInParent<IHoldable>() != null
                || hit.transform.gameObject.GetComponentInParent<IPickupable>() != null)
            {
                CrosshairManager.instance.ShowInteractIcon();
            }
            else
            {
                CrosshairManager.instance.HideInteractIcon();
            }
        }
        else
        {
            CrosshairManager.instance.HideInteractIcon();
        }
    }
}
