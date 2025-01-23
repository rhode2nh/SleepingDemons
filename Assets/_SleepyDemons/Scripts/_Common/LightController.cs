using UnityEngine;

public class LightController : MonoBehaviour, ITriggerable
{
    [SerializeField] internal new Light light;

    public virtual void SwitchLight()
    {
        light.enabled = !light.enabled;
    }

    public void ExecuteTrigger()
    {
        SwitchLight();
    }

    public virtual void ExecutePostTrigger()
    {
        return;
    }
}
