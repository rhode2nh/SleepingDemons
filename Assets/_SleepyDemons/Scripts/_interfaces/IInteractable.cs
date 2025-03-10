using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    void ExecuteInteraction(GameObject other);
    bool ExecuteOnRelease();
}
