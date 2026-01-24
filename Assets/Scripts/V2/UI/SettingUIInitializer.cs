using TMPro;
using UnityEngine;

public class SettingUIInitializer : MonoBehaviour
{
    [Header("Linked things")]
    [SerializeField] private InputFieldLink settingUsername;
    [SerializeField] private InputFieldLink matchUsername;
    [Space]
    [SerializeField] private InputFieldLink settingTeamNumber;
    [SerializeField] private InputFieldLink matchTeamNumber;
    [Space]
    [SerializeField] private DropdownLink settingScoutingIndex;
    [SerializeField] private DropdownLink teamScoutingIndex;
    [Header("Fill in")]
    [SerializeField] private TMP_InputField apiKeyInput;
    [SerializeField] private TMP_InputField overrideDate;


    private void Start()
    {
        settingUsername.Initialize();
        settingUsername.SetValue(AppSettingsSaveManager.CurrentAppSettings.currentUser);
        matchUsername.Initialize();
        settingUsername.GetInstance().onEndEdit.AddListener(LoadUser);
        matchUsername.GetInstance().onEndEdit.AddListener(LoadUser);

        settingTeamNumber.Initialize();
        settingTeamNumber.SetValue(AppSettingsSaveManager.CurrentAppSettings.teamNumber.ToString());
        matchTeamNumber.Initialize();

        settingScoutingIndex.Initialize();
        settingScoutingIndex.SetValue(AppSettingsSaveManager.CurrentAppSettings.scoutingPositionIndex);
        teamScoutingIndex.Initialize();
        settingScoutingIndex.GetInstance().onValueChanged.AddListener(UpdateScoutingPositionIndex);
        teamScoutingIndex.GetInstance().onValueChanged.AddListener(UpdateScoutingPositionIndex);


        apiKeyInput.text = AppSettingsSaveManager.CurrentAppSettings.apiKey;
        overrideDate.text = System.DateTime.Now.ToString("MM:dd:yyyy");
    }
    private void UpdateScoutingPositionIndex(int index)
    {
        AppSettingsSaveManager.CurrentAppSettings.scoutingPositionIndex = index;
    }
    private void LoadUser(string username)
    {
        SaveManager.SaveUserSettings();
        AppSettingsSaveManager.CurrentAppSettings.currentUser = username;
        UserSettingsSaveManager.CurrentUserSettings = SaveManager.GetUserSettings();
    }
}