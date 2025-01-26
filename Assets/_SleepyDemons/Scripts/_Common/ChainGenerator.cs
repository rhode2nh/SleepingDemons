using System.Collections.Generic;
using UnityEngine;

public class ChainGenerator : MonoBehaviour
{
    [SerializeField] private GameObject segment;
    [SerializeField] private int numSegments;
    [SerializeField] private List<GameObject> segments = new();
    [SerializeField] private Rigidbody chainHead;
    [SerializeField] private float distanceBetweenSegments;

    void Start()
    {
        GenerateChain();
    }
    
    public void GenerateChain()
    {
        var transform1 = chainHead.transform;
        var position = transform1.position;
        var rotation = transform1.rotation;
        var spawnPos = chainHead.position;
        var col = segment.GetComponent<SphereCollider>();
        var boundsY = col.radius * 2;
        for (var i = segments.Count; i < numSegments; i++)
        {
            var instantiatedSegment = Instantiate(segment, spawnPos, rotation, chainHead.transform);
            spawnPos = new Vector3(spawnPos.x, spawnPos.y - boundsY - distanceBetweenSegments, spawnPos.z);
            var chain = instantiatedSegment.GetComponent<IChain>(); 
            var connectedBody = i == 0 ? chainHead : segments[i - 1].GetComponent<Rigidbody>();
            chain.Init(connectedBody);
            segments.Add(instantiatedSegment);
        }
    }

    private void OnDrawGizmosSelected()
    {
        var col = segment.GetComponent<SphereCollider>();
        var boundsY = col.radius * 2;
        var currentPos = chainHead.position;
        for (var i = 0; i < numSegments; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(currentPos, boundsY);
            currentPos = new Vector3(currentPos.x, currentPos.y - boundsY - distanceBetweenSegments, currentPos.z);
        }
    }
}
