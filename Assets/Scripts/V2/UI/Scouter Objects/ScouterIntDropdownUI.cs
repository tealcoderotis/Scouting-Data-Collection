using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScouterIntCreationUI : ScouterObjectCreationUI<ScoutingIntObject>
{
    [SerializeField] protected TMP_InputField defaultValue;
    [SerializeField] protected Toggle minValueExists;
    [SerializeField] protected TMP_InputField minValue;
    [SerializeField] protected Toggle maxValueExists;
    [SerializeField] protected TMP_InputField maxValue;

    public override void ResetSpecificUI()
    {
        defaultValue.text = string.Empty;
        minValueExists.isOn = true;
        minValue.text = string.Empty;
        maxValueExists.isOn = false;
        maxValue.text = string.Empty;
    }
    public override void LoadValues(ScoutingIntObject obj)
    {
        base.LoadValues(obj);
        defaultValue.text = obj.GetFullSettings().defaultValue.ToString();
        maxValueExists.isOn = obj.GetFullSettings().hasMinValue;
        minValue.text = obj.GetFullSettings().minValue.ToString();
        maxValueExists.isOn = obj.GetFullSettings().hasMaxValue;
        maxValue.text = obj.GetFullSettings().maxValue.ToString();
    }
    public override void ApplyValues(ScoutingIntObject obj)
    {
        ScoutingIntObject.ScoutingIntSettings newSettings = GetAsSettings<ScoutingIntObject.ScoutingIntSettings>();
        newSettings.defaultValue = defaultValue.text.Length == 0 ? 0 : int.Parse(defaultValue.text);
        newSettings.hasMinValue = minValueExists.isOn;
        newSettings.minValue = minValue.text.Length == 0 ? 0 : int.Parse(minValue.text);
        newSettings.hasMaxValue = maxValueExists.isOn;
        newSettings.maxValue = maxValue.text.Length == 0 ? 0 : int.Parse(maxValue.text);
        obj.SetSettings(newSettings);
    }
}
