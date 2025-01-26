using System.Collections;
using UnityEngine;

public class Chain : Holdable, IChain
{
    [SerializeField] private ConfigurableJoint configurableJoint;

    public virtual void Awake()
    {
        configurableJoint = GetComponent<ConfigurableJoint>();
    }
    
    public void Init(Rigidbody previousSegment)
    {
        configurableJoint.connectedBody = previousSegment;
    }
}
