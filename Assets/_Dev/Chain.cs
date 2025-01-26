using UnityEngine;

public class Chain : Holdable, IChain
{
    private ConfigurableJoint _configurableJoint;

    internal override void Awake()
    {
        base.Awake();
        _configurableJoint = GetComponent<ConfigurableJoint>();
    }
    
    public void Init(Rigidbody previousSegment)
    {
        _configurableJoint.connectedBody = previousSegment;
    }
}
