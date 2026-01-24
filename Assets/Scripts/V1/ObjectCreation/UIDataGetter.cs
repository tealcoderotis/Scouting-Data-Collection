using TMPro;
using UnityEngine.UI;
using UnityEngine;

[System.Serializable]
public abstract class TypeUIDataGetter<ObjectType, SettingsType> where ObjectType : ScoutingObject
                                                                 where SettingsType : ScoutingObject.ScoutingObjectSettings
{
    public string TypeIdentifier { get => prefab == null ? "INVALID" : prefab.ObjectTypeIdentifier; }
    public ObjectType prefab;
    [SerializeField] private TMP_InputField objectName;
    [SerializeField] private TMP_Dropdown sectionName;
    [SerializeField] private TMP_InputField indexInSection;

    public virtual void SetDataFromUI(SettingsType settings)
    {
        settings.objectName = objectName.text;
        settings.sectionName = sectionName.options[sectionName.value].text;
        settings.indexInSection = int.Parse(indexInSection.text);
    }
    public virtual void SetUIFromData(SettingsType settings)
    {
        objectName.text = settings.objectName;
        sectionName.options[sectionName.value].text = settings.sectionName;
        indexInSection.text = settings.indexInSection.ToString();
    }
}
[System.Serializable]
public class IntUIDataGetter : TypeUIDataGetter<ScoutingIntObject, ScoutingIntObject.ScoutingIntSettings>
{
    [SerializeField] private TMP_InputField defaultValue;
    [SerializeField] private Toggle hasMinValue;
    [SerializeField] private TMP_InputField minValue;
    [SerializeField] private Toggle hasMaxValue;
    [SerializeField] private TMP_InputField maxValue;

    public override void SetDataFromUI(ScoutingIntObject.ScoutingIntSettings settings)
    {
        base.SetDataFromUI(settings);
        settings.defaultValue = int.Parse(defaultValue.text);
        settings.hasMinValue = hasMinValue.isOn;
        settings.minValue = int.Parse(minValue.text);
        settings.hasMaxValue = hasMaxValue.isOn;
        settings.maxValue = int.Parse(maxValue.text);
    }
    public override void SetUIFromData(ScoutingIntObject.ScoutingIntSettings settings)
    {
        base.SetUIFromData(settings);
        defaultValue.text = settings.defaultValue.ToString();
        hasMinValue.isOn = settings.hasMinValue;
        minValue.text = settings.minValue.ToString();
        hasMaxValue.isOn = settings.hasMaxValue;
        maxValue.text = settings.maxValue.ToString();
    }
}
[System.Serializable]
public class BoolUIDataGetter : TypeUIDataGetter<ScoutingBoolObject, ScoutingBoolObject.ScoutingBoolSettings>
{
    [SerializeField] private Toggle defaultValue;

    public override void SetDataFromUI(ScoutingBoolObject.ScoutingBoolSettings settings)
    {
        base.SetDataFromUI(settings);
        settings.defaultValue = defaultValue.isOn;
    }
    public override void SetUIFromData(ScoutingBoolObject.ScoutingBoolSettings settings)
    {
        base.SetUIFromData(settings);
        defaultValue.isOn = settings.defaultValue;
    }
}
[System.Serializable]
public class DropdownUIDataGetter : TypeUIDataGetter<ScoutingDropdownObject, ScoutingDropdownObject.ScoutingDropdownSettings>
{
    [SerializeField] DropdownCreate dropdown;

    public override void SetDataFromUI(ScoutingDropdownObject.ScoutingDropdownSettings settings)
    {
        base.SetDataFromUI(settings);
        settings.dropdownOptions.Clear();
        for (int i = 0; i < dropdown.DropdownOptions.Count; i++)
            settings.dropdownOptions.Add(new(dropdown.DropdownOptions[i].NameInput.text, "NO_LOGIC"));
    }
    public override void SetUIFromData(ScoutingDropdownObject.ScoutingDropdownSettings settings)
    {
        base.SetUIFromData(settings);
        for (int i = 0; i < settings.dropdownOptions.Count; i++)
            dropdown.CreateDropdownOption(settings.dropdownOptions[i].optionName, -1);
        dropdown.ForceUpdateOptionCreatore();
    }
}
[System.Serializable]
public class TextUIDataGetter : TypeUIDataGetter<ScoutingTextObject, ScoutingTextObject.ScoutingTextSettings>
{
    [SerializeField] private TMP_InputField placeholderText;
    [SerializeField] private TMP_InputField minimumHeight;
    // it's either truncate or autoexpand so for now logic is using bool. Use dropdown to give user more info
    [SerializeField] private TMP_InputField autoExpand;

    public override void SetDataFromUI(ScoutingTextObject.ScoutingTextSettings settings)
    {
        base.SetDataFromUI(settings);
        settings.placeholderText = placeholderText.text;
        settings.minimumHeight = float.Parse(minimumHeight.text);
        settings.maximiumHeight = float.Parse(autoExpand.text);
    }
    public override void SetUIFromData(ScoutingTextObject.ScoutingTextSettings settings)
    {
        base.SetUIFromData(settings);
        placeholderText.text = settings.placeholderText;
        minimumHeight.text = settings.minimumHeight.ToString();
        autoExpand.text = settings.maximiumHeight.ToString();
    }
}