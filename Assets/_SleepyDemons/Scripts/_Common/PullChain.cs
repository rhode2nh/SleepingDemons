using System.Collections;
using UnityEngine;

public class PullChain : Holdable
{
    [SerializeField] internal float pullDistance;
    [SerializeField] internal Rigidbody anchor;
    [SerializeField] private float tension;
    [SerializeField] private float pullStrength;
    [SerializeField] ITriggerable _triggerable;

    private Vector3 initialAnchorPos;

    public void Init(float pullDistance, Rigidbody anchor, ITriggerable _triggerable)
    {
        this.pullDistance = pullDistance;
        this.anchor = anchor;
        this._triggerable = _triggerable;
    }
    
    void Start()
    {
        initialAnchorPos = anchor.position;
    }
    
    public override void ExecuteInteraction(GameObject other)
    {
        var interact = other.gameObject.GetComponent<Interact>();
        base.ExecuteInteraction(other);
        StartCoroutine(MoveAnchor(interact));
    }

    IEnumerator MoveAnchor(Interact interact)
    {
        var initialAncPos = anchor.position;
        var relativeDistance = Vector3.Distance(Camera.main.transform.position, interact._inputRaycast.hit.point);
        var initialReferencePoint = interact._inputRaycast.hit.point;
        var distanceToTravel = Vector3.Distance(initialReferencePoint, Camera.main.transform.position + (Camera.main.transform.forward * relativeDistance));
        var lightSwitched = false;
        while (PlayerManager.instance.isHolding)
        {
            var calculatedDir = (Camera.main.transform.forward + new Vector3(0, distanceToTravel, 0)).normalized;
            var posToHold = Camera.main.transform.position + (calculatedDir * relativeDistance);
            var relativePointToHold = initialReferencePoint + (anchor.position - initialAncPos);
            var posToMove = anchor.position + 
                            new Vector3(0, (posToHold.y - relativePointToHold.y), 0) * pullStrength * Time.deltaTime;
            if (posToMove.y <= initialAncPos.y && !lightSwitched)
            {
                anchor.MovePosition(posToMove);
            }

            if (initialAncPos.y - anchor.position.y > pullDistance && !lightSwitched)
            {
                lightSwitched = true;
                _triggerable.ExecuteTrigger();
            }
            yield return new WaitForFixedUpdate();
        }

        while (anchor.position.y < initialAnchorPos.y)
        {
            Vector3 newPos = new Vector3(0, initialAnchorPos.y, 0);
            anchor.MovePosition(anchor.position + newPos * tension * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }

        if (lightSwitched)
        {
            _triggerable.ExecutePostTrigger();
        }
    }
}
