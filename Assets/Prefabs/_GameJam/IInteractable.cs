using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public void ExecuteInteraction(GameObject other);
    /// <summary>
    /// Executes interaction based when input is released. This is mostly useful for objects that need a second call to
    /// update it's state e.g. holding and letting go of an object.
    /// </summary>
    /// <returns>A bool indicating whether or not to execute on input release.</returns>
    public bool ExecuteOnRelease();
} 