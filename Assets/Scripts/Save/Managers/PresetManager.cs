using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PresetManager : MonoBehaviour
{
    public static PresetManager Instance;

    [SerializeField] private ObjectCreation objectCreation;
    [SerializeField] private TextMeshProUGUI scouterNamePreset;
    [SerializeField] private Transform scouterContent;
    [SerializeField] private TMP_Dropdown presetSelectorDropdown;

    [SerializeField] private string[] presetNames;

    private void Awake()
    {
        //Instance = this;

        //presetNames = SaveManager.GetSavePresets();
        //presetSelectorDropdown.options.Clear();
        //for (int i = 0; i < presetNames.Length; i++)
        //    presetSelectorDropdown.options.Add(new(GetPresetName(presetNames[i])));
        //presetSelectorDropdown.options.Add(new("Create new preset"));

        //presetSelectorDropdown.onValueChanged.AddListener(LoadPreset);
        //if (presetNames.Length > 0)
        //{
        //    presetSelectorDropdown.value = presetNames.Length - 1;
        //    // does not call event itself as the dropdown is disabled when this runs
        //    LoadPreset(presetNames.Length - 1);
        //}
    }

    public string GetPresetName(string filePath) => filePath[(filePath.LastIndexOf('\\') + 1)..];

    public void SavePreset()
    {
        List<SaveManager.PresetData> scouterObjects = new();
        for (int section = 1; section < scouterContent.childCount; section++)
        {
            for (int scoutingData = 0; scoutingData < scouterContent.GetChild(section).childCount; scoutingData++)
            {
                ScoutingObject scoutingObject = scouterContent.GetChild(section).GetChild(scoutingData).GetComponent<ScoutingObject>();
                scouterObjects.Add(new(scoutingObject));
            }
        }
        SaveManager.SaveScoutingPreset(scouterNamePreset.text, scouterObjects);
    }

    public void LoadLastPreset() => LoadPreset(GetPresetName(presetNames[^1]));
    public void LoadPreset(int presetIndex)
    {
        if (presetIndex < 0 || presetIndex >= presetNames.Length) LoadPreset(new SaveManager.PresetData[] { });
        else LoadPreset(GetPresetName(presetNames[presetIndex]));
    }
    public void LoadPreset(string presetName) => LoadPreset(SaveManager.LoadScoutingPreset(presetName));
    public  void LoadPreset(SaveManager.PresetData[] presetObjects)
    {
        // 1 is non-editable and ignored
        for (int sectionIndex = 1; sectionIndex < scouterContent.childCount; sectionIndex++)
        {
            for (int objIndex = scouterContent.GetChild(sectionIndex).childCount - 1; objIndex >= 0; objIndex--)
                Destroy(scouterContent.GetChild(sectionIndex).GetChild(objIndex).gameObject);
        }
        for (int i = 0; i < presetObjects.Length; i++)
        {
            objectCreation.ImportObjectSettings(presetObjects[i].identifier, presetObjects[i].json, true);
        }
    }
}
