using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;

public class PresetManager : MonoBehaviour
{
    public static PresetManager Instance;

    [SerializeField] private TMP_InputField scouterNamePreset;
    [SerializeField] private Transform scouterContent;

    [Space]
    [SerializeField] private FileSelection fileSelection;
    [SerializeField] private List<FileInfo> presetFiles;
    public List<FileInfo> GetPresets() => presetFiles;

    private void Awake()
    {
        Instance = this;
        LoadPresetPaths();
        fileSelection.ReloadPresets();
    }
    private void Start()
    {
        for (int i = 0; i < presetFiles.Count; ++i)
        {
            if (AppSettingsSaveManager.CurrentAppSettings.presetName != presetFiles[i].Name) continue;
            LoadPreset(presetFiles[i].Name);
            return;
        }
        LoadLastPreset();
    }

    public void LoadPresetPaths()
    {
        string[] presetPaths = SaveManager.GetSavePresets();
        presetFiles = new();

        for (int i = 0; i < presetPaths.Length; i++)
            presetFiles.Add(new(presetPaths[i]));
        presetFiles.OrderBy(p => p.LastWriteTime);
    }


    public void LoadLastPreset()
    {
        if (presetFiles.Count > 0) LoadPreset(presetFiles[^1].Name);
    }
    public void LoadPreset(int presetIndex)
    {
        if (presetIndex < 0 || presetIndex >= presetFiles.Count) LoadPreset(new SaveManager.PresetData[] { });
        else LoadPreset(presetFiles[presetIndex].Name);
    }
    public void LoadPreset(string presetName) => LoadPreset(SaveManager.LoadScoutingPreset(presetName));
    public void LoadPreset(SaveManager.PresetData[] presetObjects)
    {
        // 0 is Pregame: non-editable and thus ignored
        for (int sectionIndex = 1; sectionIndex < scouterContent.childCount; sectionIndex++)
        {
            for (int objIndex = scouterContent.GetChild(sectionIndex).childCount - 1; objIndex >= 0; objIndex--)
            {
                Destroy(scouterContent.GetChild(sectionIndex).GetChild(objIndex).gameObject);
            }
        }
        for (int i = 0; i < presetObjects.Length; i++)
        {
            ScouterObjectCreator.Instance.ImportScoutingObject(presetObjects[i].identifier, presetObjects[i].json);
        }
    }
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
        presetFiles.Insert(0, new FileInfo(Path.Combine(SaveManager.ScoutingPresets, scouterNamePreset.text)));
        AppSettingsSaveManager.CurrentAppSettings.presetName = scouterNamePreset.text;
    }

    public void EnabledIsDuplicate(GameObject gameObj) => gameObj.SetActive(SaveManager.PresetExists(scouterNamePreset.text));
}
