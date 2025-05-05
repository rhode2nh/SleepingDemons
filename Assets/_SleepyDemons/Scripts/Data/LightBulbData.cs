using UnityEngine;

public class LightBulbData : MonoBehaviour
{
    [field: SerializeField] public bool Flicker { get; set; }
    [field: SerializeField] public float ForceToBreakBulb { get; private set; }
    [field: SerializeField] public bool JustSpawned { get; set; }

    private void Awake()
    {
        JustSpawned = true;
    }

    public void Init(LightBulbData lightBulbData)
    {
        Flicker = lightBulbData.Flicker;
        ForceToBreakBulb = lightBulbData.ForceToBreakBulb;
    }
}
