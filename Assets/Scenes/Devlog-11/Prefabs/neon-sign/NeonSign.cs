using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeonSign : MonoBehaviour, IInteractable
{
    [SerializeField] private Light areaLight;
    [SerializeField] private Material[] _materials;
    public void ExecuteInteraction(GameObject other)
    {
        if (PlayerManager.instance.isHolding)
        {
            areaLight.enabled = !areaLight.enabled;
            if (areaLight.enabled)
            {
                foreach (var material in _materials)
                {
                    material.EnableKeyword("_EMISSION");
                }
            }
            else
            {
                foreach (var material in _materials)
                {
                    material.DisableKeyword("_EMISSION");
                }
            }
        }
    }

    public bool ExecuteOnRelease() { return false; }
}
