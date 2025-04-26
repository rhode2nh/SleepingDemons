using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Teleporter : MonoBehaviour
{
    [field: SerializeField] public Teleporter Destination { get; set; }
    [SerializeField] private bool _preservePlayerOffset = true;

    private Collider _trigger;
    private Action OnPortalEnter;

    private void Start()
    {
        _trigger = GetComponent<Collider>();
        _trigger.isTrigger = true;
    }

    public void Init(Action onPortalEnter, Teleporter destination)
    {
        OnPortalEnter += onPortalEnter;
        Destination = destination;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (Destination == null) return;
        if (other.CompareTag("Player"))
        {
            var offset = CalculateOffset(other);
            var dotProduct = Vector3.Dot(transform.forward, Destination.transform.forward);
            Destination.Teleport(other.gameObject, offset, other.transform.forward * -dotProduct);
            OnPortalEnter?.Invoke();
        }
    }

    protected Vector3 CalculateOffset(Collider other)
    {
        return (other.transform.position - transform.position);
    }

    public virtual void Teleport(GameObject teleportObject, Vector3 offset, Vector3 forward)
    {
        var teleportPos = transform.position;
        if (_preservePlayerOffset)
        {
            teleportPos += offset;
        }

        teleportObject.transform.position = teleportPos;
        teleportObject.transform.forward = forward;
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.color *= new Color(1, 1, 1, 0.0f);
        Gizmos.DrawCube(transform.position, new Vector3(1, 4, 2));
        if (Destination == null) return;
        
        Gizmos.DrawLine(transform.position, Destination.transform.position);
    }
}
