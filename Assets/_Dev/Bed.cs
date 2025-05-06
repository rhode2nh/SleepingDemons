using UnityEngine;

public class Bed : Interactable
{
    private POVCamera _povCamera;

    private void Awake()
    {
        _povCamera = GetComponentInChildren<POVCamera>();
    }
    
    public override void ExecuteInteraction(GameObject other)
    {
        // Go to bed
        var sleep = other.GetComponent<Sleep>();
        sleep.GetInBed(_povCamera);
    }
}
