using UnityEngine;

public class AppSettingsSaveManager : MonoBehaviour
{
    public static AppSettingsSaveManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
        CurrentAppSettings = SaveManager.GetAppSettings();
    }
    private void OnApplicationPause()
    {
        SaveManager.SaveAppSettings();
    }

    public AppSettings CurrentAppSettings;
}
