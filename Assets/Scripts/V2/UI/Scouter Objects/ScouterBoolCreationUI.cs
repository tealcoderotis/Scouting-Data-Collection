using UnityEngine;
using UnityEngine.UI;

public class ScouterBoolCreationUI : ScouterObjectCreationUI<ScoutingBoolObject>
{
    [SerializeField] protected Toggle defaultValueToggle;

    public override void ResetSpecificUI()
    {
        defaultValueToggle.isOn = false;
    }
    public override void LoadValues(ScoutingBoolObject obj)
    {
        base.LoadValues(obj);
        defaultValueToggle.isOn = obj.GetFullSettings().defaultValue;
    }
    public override void ApplyValues(ScoutingBoolObject obj)
    {
        ScoutingBoolObject.ScoutingBoolSettings newSettings = GetAsSettings<ScoutingBoolObject.ScoutingBoolSettings>();
        newSettings.defaultValue = defaultValueToggle.isOn;
        obj.SetSettings(newSettings);
    }
}