using System.Collections;
using UnityEngine;

public class PullChain : Chain
{
    [SerializeField] private float tension;
    [SerializeField] private float pullStrength;
    [SerializeField] ITriggerable triggerable;
    [SerializeField] private ChainAnchor chainAnchor;

    private Vector3 initialAnchorPos;
    
    internal override void Awake()
    {
        base.Awake();
        triggerable = GetComponentInParent<ITriggerable>();
        chainAnchor = GetComponentInParent<ChainAnchor>();
        initialAnchorPos = chainAnchor.rb.position;
    }
    
    public override void ExecuteInteraction(GameObject other)
    {
        var interact = other.gameObject.GetComponent<Interact>();
        base.ExecuteInteraction(other);
        StartCoroutine(MoveAnchor(interact));
    }

    IEnumerator MoveAnchor(Interact interact)
    {
        var initialAncPos = chainAnchor.rb.position;
        var relativeDistance = Vector3.Distance(Camera.main.transform.position, interact._inputRaycast.hit.point);
        var initialReferencePoint = interact._inputRaycast.hit.point;
        var distanceToTravel = Vector3.Distance(initialReferencePoint, Camera.main.transform.position + (Camera.main.transform.forward * relativeDistance));
        var lightSwitched = false;
        while (PlayerManager.instance.isHolding)
        {
            var calculatedDir = (Camera.main.transform.forward + new Vector3(0, distanceToTravel, 0)).normalized;
            var posToHold = Camera.main.transform.position + (calculatedDir * relativeDistance);
            var relativePointToHold = initialReferencePoint + (chainAnchor.rb.position - initialAncPos);
            var posToMove = chainAnchor.rb.position + 
                            new Vector3(0, (posToHold.y - relativePointToHold.y), 0) * pullStrength * Time.deltaTime;
            if (posToMove.y <= initialAncPos.y && !lightSwitched)
            {
                chainAnchor.rb.MovePosition(posToMove);
            }

            if (initialAncPos.y - chainAnchor.rb.position.y > chainAnchor.pullDistance && !lightSwitched)
            {
                lightSwitched = true;
                triggerable.ExecuteTrigger();
            }
            yield return new WaitForFixedUpdate();
        }

        while (chainAnchor.rb.position.y < initialAnchorPos.y)
        {
            Vector3 newPos = new Vector3(0, initialAnchorPos.y, 0);
            chainAnchor.rb.MovePosition(chainAnchor.rb.position + newPos * tension * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }

        if (lightSwitched)
        {
            triggerable.ExecutePostTrigger();
        }
    }
}
