using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalLightObject : MonoBehaviour
{
    private GameObject _cloneObject;

    private Light _light;
    
    protected Portal InPortal;
    protected Portal OutPortal;
    
    protected static readonly Quaternion HalfTurn = Quaternion.Euler(0.0f, 180.0f, 0.0f);
    
    void Awake()
    {
        _light = GetComponent<Light>();
        
        _cloneObject = new GameObject();
        _cloneObject.SetActive(false);
        var cloneLight = _cloneObject.AddComponent<Light>();
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
        
        _cloneObject.transform.localScale = transform.localScale;
    }

    public void UpdatePos()
    {
        if(InPortal == null || OutPortal == null)
        {
            _cloneObject.SetActive(false);
            return;
        }
        _cloneObject.SetActive(_light.enabled);
        if(_cloneObject.activeSelf)
        {
            var inTransform = InPortal.Center;
            var outTransform = OutPortal.Center;

            // Update position of clone.
            Vector3 relativePos = inTransform.InverseTransformPoint(transform.position);
            relativePos = HalfTurn * relativePos;
            _cloneObject.transform.position = outTransform.TransformPoint(relativePos);

            // Update rotation of clone.
            Quaternion relativeRot = Quaternion.Inverse(inTransform.rotation) * transform.rotation;
            relativeRot = HalfTurn * relativeRot;
            _cloneObject.transform.rotation = outTransform.rotation * relativeRot;
        }
        else
        {
            _cloneObject.transform.position = new Vector3(-1000.0f, 1000.0f, -1000.0f);
        }
    }

    public void SetPortals(Portal inPortal, Portal outPortal)
    {
        InPortal = inPortal;
        OutPortal = outPortal;
        
        _cloneObject.SetActive(true);
    }

    public void ExitPortals()
    {
        OutPortal = null;
        InPortal = null;
        
        _cloneObject.SetActive(false);
    }
}
