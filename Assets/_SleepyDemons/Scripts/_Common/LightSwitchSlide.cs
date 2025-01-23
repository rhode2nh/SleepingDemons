using System.Collections.Generic;
using UnityEngine;

public class LightSwitchSlide : MonoBehaviour
{
    [SerializeField] private ConfigurableJoint configurableJoint;
    [SerializeField] private List<Light> lightSources;

    [SerializeField, ReadOnly] private float limit;
    [SerializeField, ReadOnly] private float currentIntensity;

    private float maxLimit;
    private float minLimit;

    void Start()
    {
        limit = configurableJoint.linearLimit.limit;
        minLimit = transform.position.y - limit;
        maxLimit = transform.position.y + limit;
    }

    void Update()
    {
        var curYPos = transform.position.y;
        currentIntensity = (curYPos - minLimit) / (maxLimit - minLimit);
        if (lightSources.Count > 0)
        {
            foreach (var lightSource in lightSources)
            {
                lightSource.intensity = currentIntensity;
            }
        } 
    }
}
