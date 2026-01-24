using TMPro;
using UnityEngine.Events;

public class DropdownLink : UILink<TMP_Dropdown, int>
{
    protected override UnityEvent<int> ValueSource => uiInstance.onValueChanged;
    public override int GetValue() => uiInstance.value;
    public override void SetValue(int value) => uiInstance.SetValueWithoutNotify(value);
}
