using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FileSelectReferences : MonoBehaviour
{
    FileInfo file;
    [SerializeField] private Toggle select;
    [SerializeField] private TextMeshProUGUI nameDisplay;
    [SerializeField] private TextMeshProUGUI timestampDisplay;

    public Toggle.ToggleEvent OnSelect => select.onValueChanged;
    private void Awake()
    {
        select.onValueChanged.AddListener(isOn => select.interactable = !isOn);
    }
    public void SetFile(FileInfo file)
    {
        this.file = file;
        UpdateDisplay();
    }
    public void UpdateDisplay()
    {
        nameDisplay.text = file.Name;
        timestampDisplay.text = file.LastAccessTime.ToString();
    }

    public void SetSelected(bool selected) => select.isOn = selected;
    public string GetName() => nameDisplay.text;
    public DateTime GetTimeStamp() => file.LastAccessTime;

    private void OnEnable()
    {
        if (file != null) UpdateDisplay();
        select.isOn = GetName() == AppSettingsSaveManager.Instance.CurrentAppSettings.presetName;
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
    }
}