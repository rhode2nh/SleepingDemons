using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalLightObject : MonoBehaviour
{
    public GameObject CloneObject { get; private set; }

    private Light _light;
    
    protected Portal InPortal;
    protected Portal OutPortal;
    
    protected static readonly Quaternion HalfTurn = Quaternion.Euler(0.0f, 180.0f, 0.0f);
    
    void Awake()
    {
        _light = GetComponent<Light>();
        
        CloneObject = new GameObject();
        CloneObject.SetActive(false);
        var cloneLight = CloneObject.AddComponent<Light>();
        cloneLight.type = _light.type;
        cloneLight.range = _light.range;
        cloneLight.spotAngle = _light.spotAngle;
        cloneLight.renderMode = _light.renderMode;
        cloneLight.intensity = _light.intensity;
        cloneLight.color = _light.color;
        cloneLight.colorTemperature = _light.colorTemperature;
        cloneLight.useColorTemperature = _light.useColorTemperature;
        cloneLight.cookie = _light.cookie;
        cloneLight.shadows = _light.shadows;
        
        CloneObject.transform.localScale = transform.localScale;
    }

    public void UpdatePos()
    {
        var inTransform = InPortal.Center;
        var outTransform = OutPortal.Center;

        // Update position of clone.
        Vector3 relativePos = inTransform.InverseTransformPoint(transform.position);
        relativePos = HalfTurn * relativePos;
        CloneObject.transform.position = outTransform.TransformPoint(relativePos);

        // Update rotation of clone.
        Quaternion relativeRot = Quaternion.Inverse(inTransform.rotation) * transform.rotation;
        relativeRot = HalfTurn * relativeRot;
        CloneObject.transform.rotation = outTransform.rotation * relativeRot;
    }

    public void SetPortals(Portal inPortal, Portal outPortal)
    {
        InPortal = inPortal;
        OutPortal = outPortal;
        
        CloneObject.SetActive(true);
    }

    public void Deactivate()
    {
        CloneObject.SetActive(false);
    }

    public void Activate()
    {
        CloneObject.SetActive(true);
    }

    public bool IsLightOn()
    {
        return _light.enabled;
    }
}
