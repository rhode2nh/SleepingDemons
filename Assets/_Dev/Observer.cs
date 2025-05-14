using UnityEngine;

public class Observer : MonoBehaviour
{
    public static Observer Instance;
    private int attempts = 0;

    private void Awake()
    {
        Instance = this;
    }

    public bool Roll()
    {
        if (attempts < 3) return false;
        
        attempts = 0;
        return true;

    }
}
