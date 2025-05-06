using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    [SerializeField] private UserSettings _defaultSettings;
    [field: SerializeField] public UserSettings CurrentSettings;

    public static SaveLoadManager Instance;
    
    private void Awake()
    {
        Instance = this;
        CurrentSettings.ApplySettings(_defaultSettings);
    }

}
