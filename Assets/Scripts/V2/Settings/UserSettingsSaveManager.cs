using UnityEngine;
using UnityEngine.Events;

public class UserSettingsSaveManager : MonoBehaviour
{
    private void Awake() => onChangeUser += _onChangeUser.Invoke;
    private void OnApplicationPause()
    {
        SaveManager.SaveUserSettings();
    }
    private void OnApplicationQuit()
    {
        SaveManager.SaveUserSettings();
    }
    private static UserSettings _CurrentUserSettings;
    public static UserSettings CurrentUserSettings
    {
        get
        {
            _CurrentUserSettings ??= SaveManager.GetUserSettings();
            _CurrentUserSettings ??= new();
            return _CurrentUserSettings;
        }
        set
        {
            _CurrentUserSettings = value;
            onChangeUser.Invoke(AppSettingsSaveManager.CurrentAppSettings.currentUser);
        }
    }
    [SerializeField] private UnityEvent<string> _onChangeUser;
    public static System.Action<string> onChangeUser = delegate { };
}
