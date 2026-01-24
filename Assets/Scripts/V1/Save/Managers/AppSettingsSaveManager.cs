using UnityEngine;

public class AppSettingsSaveManager : MonoBehaviour
{
    private void OnApplicationPause()
    {
        SaveManager.SaveAppSettings();
    }
    private void OnApplicationQuit()
    {
        SaveManager.SaveAppSettings();
    }
    private static AppSettings _CurrentAppSettings;
    public static AppSettings CurrentAppSettings
    {
        get
        {
            _CurrentAppSettings ??= SaveManager.GetAppSettings();
            _CurrentAppSettings ??= new();
            return _CurrentAppSettings;
        }
    }

    public void SetAPIKey(string apiKey) => CurrentAppSettings.apiKey = apiKey;
}
