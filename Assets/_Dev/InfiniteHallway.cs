using UnityEngine;

public class InfiniteHallway : RecursiveTeleporterManager
{
    [SerializeField] private int _triggerDepth;
    [SerializeField] private Teleporter _triggerDestination;
    [SerializeField, ReadOnly] private bool _depthTriggered;
    protected override void Start()
    {
        base.Start();
    }

    protected override void OnSourceEnter()
    {
        base.OnSourceEnter();
    }

    protected override void OnDestinationEnter()
    {
        base.OnDestinationEnter();
        if (_currentDepth == _triggerDepth)
        {
            _depthTriggered = true;
        }

        if (_depthTriggered)
        {
            UpdateSourceDestination();
        }
    }

    private void UpdateSourceDestination()
    {
        _source.Destination = _triggerDestination;
    }
}
