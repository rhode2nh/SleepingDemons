using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : MonoBehaviour, IInteractable
{
    public void ExecuteInteraction(GameObject other)
    {
        SanityManager.Instance.SleepManager.Sleep();
    }

    public bool ExecuteOnRelease()
    {
        return false;
    }
}
