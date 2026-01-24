using TMPro;
using UnityEngine.Events;

public class InputFieldLink : UILink<TMP_InputField, string>
{
    protected override UnityEvent<string> ValueSource => uiInstance.onValueChanged;
    public override string GetValue() => uiInstance.text;
    public override void SetValue(string value) => uiInstance.SetTextWithoutNotify(value);
}