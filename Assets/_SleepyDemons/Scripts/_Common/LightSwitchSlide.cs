using UnityEngine;

public class LightSwitchSlide : MonoBehaviour
{
    [SerializeField] private ConfigurableJoint configurableJoint;
    [SerializeField, ReadOnly] private float limit;
    [SerializeField, ReadOnly] private float currentIntensity;

    private LightSwitch _lightSwitch;
    private float _maxLimit;
    private float _minLimit;

    void Awake()
    {
        _lightSwitch = GetComponentInParent<LightSwitch>();
        limit = configurableJoint.linearLimit.limit;
        _minLimit = transform.position.y - limit;
        _maxLimit = transform.position.y + limit;
        currentIntensity = CalculateIntensity();
    }

    void Update()
    {
        currentIntensity = CalculateIntensity();
        _lightSwitch.DimLight(currentIntensity);   
    }

    private float CalculateIntensity()
    {
       var curYPos = transform.position.y;
       return (curYPos - _minLimit) / (_maxLimit - _minLimit);
    }
}
