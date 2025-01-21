using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ChainTest : MonoBehaviour
{
    [SerializeField] private GameObject segment;
    [SerializeField] private int numSegments;
    [SerializeField] private List<GameObject> segments = new();
    [SerializeField] private Rigidbody chainHead;
    [SerializeField] private float distanceBetweenSegments;
    
    [SerializeField] private float pullDistance;
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Rigidbody anchor;
    [SerializeField] private Light lightBulb;
    [SerializeField] private float emissiveIntensity;
    [SerializeField] private AudioSource lampSwitchOn;
    [SerializeField] private AudioSource lampSwitchOff;
    
    public void ResetChain()
    {
        for (int i = 0; i < segments.Count; i++)
        {
            UnityEditor.Undo.DestroyObjectImmediate(segments[i]);
        }

        segments = new();
    }
    
    public void GenerateChain()
    {
        if (numSegments - segments.Count < 0)
        {
            for (int i = segments.Count - 1; i >= numSegments; i--)
            {
                var seg = segments[i];
                segments.RemoveAt(i);
                UnityEditor.Undo.DestroyObjectImmediate(seg);
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
                    var spawnPos = new Vector3(position.x, position.y - boundsY * distanceBetweenSegments, position.z);
                    segments[i].transform.position = spawnPos;
                }
            }
            for (var i = segments.Count; i < numSegments; i++)
            {
                var col = segment.GetComponent<SphereCollider>();
                var pullChain = segment.GetComponent<PullChain>();
                pullChain.Init(pullDistance, _renderer, chainHead, lightBulb, emissiveIntensity, lampSwitchOn, lampSwitchOff);
                var boundsY = col.radius * i * 2;
                var spawnPos = new Vector3(position.x, position.y - boundsY * distanceBetweenSegments, position.z);
                var instantiatedSegment = Instantiate(segment, spawnPos, rotation, transform);
                instantiatedSegment.GetComponent<ConfigurableJoint>().connectedBody = i == 0 ? chainHead : segments[i - 1].GetComponent<Rigidbody>();
                segments.Add(instantiatedSegment);
            }
        }
    }
}
