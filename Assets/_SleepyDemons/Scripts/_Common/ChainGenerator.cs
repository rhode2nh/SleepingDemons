using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ChainGenerator : MonoBehaviour
{
    [SerializeField] private GameObject segment;
    [SerializeField] private int numSegments;
    [SerializeField] private List<GameObject> segments = new();
    [SerializeField] private Rigidbody chainHead;
    [SerializeField] private float distanceBetweenSegments;
    
    [SerializeField] private float pullDistance;
    [SerializeField] private Rigidbody anchor;
    [SerializeField] private Lamp lamp;

    public void ResetChain()
    {
        try
        {
            foreach (var t in segments)
            {
                Undo.DestroyObjectImmediate(t);
            }

            segments.Clear();
        }
        catch (SystemException e)
        {
            if (e is MissingReferenceException or ArgumentNullException)
            {
                segments.Clear();
            }
        }
    }
    
    public void GenerateChain()
    {
        try
        {
            if (numSegments - segments.Count < 0)
            {
                for (var i = segments.Count - 1; i >= numSegments; i--)
                {
                    var seg = segments[i];
                    segments.RemoveAt(i);
                    Undo.DestroyObjectImmediate(seg);
                    
                }
            }
            else
            {
                var transform1 = chainHead.transform;
                var position = transform1.position;
                var rotation = transform1.rotation;
                if (segments.Count == numSegments)
                {
                    for (var i = 0; i < segments.Count; i++)
                    {
                        var col = segments[i].GetComponent<SphereCollider>();
                        var boundsY = col.radius * i * 2;
                        var spawnPos = new Vector3(position.x, position.y - boundsY * distanceBetweenSegments,
                            position.z);
                        segments[i].transform.position = spawnPos;
                    }
                }

                for (var i = segments.Count; i < numSegments; i++)
                {
                    var col = segment.GetComponent<SphereCollider>();
                    var boundsY = col.radius * i * 2;
                    var spawnPos = new Vector3(position.x, position.y - boundsY * distanceBetweenSegments, position.z);
                    var instantiatedSegment = Instantiate(segment, spawnPos, rotation, transform);
                    var pullChain = instantiatedSegment.GetComponent<PullChain>();
                    pullChain.Init(pullDistance, chainHead, lamp);
                    instantiatedSegment.GetComponent<ConfigurableJoint>().connectedBody =
                        i == 0 ? chainHead : segments[i - 1].GetComponent<Rigidbody>();
                    segments.Add(instantiatedSegment);
                }
            }
        }
        catch (SystemException e)
        {
            if (e is MissingReferenceException or ArgumentNullException)
            {
                segments.Clear();
                GenerateChain();
            }
        }
    }
}
