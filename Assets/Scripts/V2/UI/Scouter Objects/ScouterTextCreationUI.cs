using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScouterTextCreationUI : ScouterObjectCreationUI<ScoutingTextObject>
{
    [SerializeField] protected TMP_InputField placeholder;
    [SerializeField] protected TMP_InputField minimumHeight;
    [SerializeField] protected Toggle autoExpands;
    [SerializeField] protected TMP_InputField maximiumHeight;

    public override void ResetSpecificUI()
    {
        placeholder.text = string.Empty;
        minimumHeight.text = string.Empty;
        autoExpands.isOn = true;
        maximiumHeight.text = string.Empty;
    }
    public override void LoadValues(ScoutingTextObject obj)
    {
        base.LoadValues(obj);
        placeholder.text = obj.GetFullSettings().placeholderText;
        minimumHeight.text = obj.GetFullSettings().minimumHeight.ToString();
        autoExpands.isOn = obj.GetFullSettings().autoExpands;
        maximiumHeight.text = obj.GetFullSettings().maximiumHeight.ToString();
    }
    public override void ApplyValues(ScoutingTextObject obj)
    {
        ScoutingTextObject.ScoutingTextSettings newSettings = GetAsSettings<ScoutingTextObject.ScoutingTextSettings>();
        newSettings.placeholderText = placeholder.text;
        newSettings.minimumHeight = minimumHeight.text.Length == 0 ? 150 : int.Parse(minimumHeight.text);
        newSettings.autoExpands = autoExpands.isOn;
        newSettings.maximiumHeight = maximiumHeight.text.Length == 0 ? 0 : int.Parse(maximiumHeight.text);
        obj.SetSettings(newSettings);
    }
}
