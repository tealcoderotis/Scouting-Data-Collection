using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileSelection : MonoBehaviour
{
    [SerializeField] private FileSelectReferences prefab;
    [SerializeField] private Transform content;
    private readonly List<FileSelectReferences> selectors = new();
    private FileSelectReferences currentlySelected;

    private void OnEnable()
    {
        ReloadPresets();
    }
    public void ReloadPresets()
    {
        List<FileInfo> allFiles = PresetManager.Instance.GetPresets();
        for (int i = 0; i < selectors.Count; ++i)
        {
            selectors[i].SetFile(allFiles[i]);
            selectors[i].gameObject.SetActive(true);
        }
        for (int i = selectors.Count; i < allFiles.Count; ++i)
        {
            selectors.Add(Instantiate(prefab, content));
            selectors[i].SetFile(allFiles[i]);
            int index = i;
            selectors[i].OnSelect.AddListener((isOn) =>
            {
                if (isOn) ChangeSelected(selectors[index]);
            });
        }
        for (int i = allFiles.Count; i < selectors.Count; ++i)
        {
            selectors[i].gameObject.SetActive(false);
        }
    }
    public void ChangeSelected(FileSelectReferences newFile)
    {
        if (currentlySelected != null) currentlySelected.SetSelected(false);
        currentlySelected = newFile;
    }
    public void SubmitSelected()
    {
        if (currentlySelected == null) return;
        AppSettingsSaveManager.CurrentAppSettings.presetName = currentlySelected.GetName();
        PresetManager.Instance.LoadPreset(AppSettingsSaveManager.CurrentAppSettings.presetName);
    }
}