using UnityEngine;

[CreateAssetMenu(fileName = "User Settings", menuName = "Scriptable Objects/User Settings")]
public class UserSettings : ScriptableObject
{
    [field: SerializeField] public float FieldOfView { get; private set; }

    public void ApplySettings(UserSettings settings)
    {
        FieldOfView = settings.FieldOfView;
    }
}
