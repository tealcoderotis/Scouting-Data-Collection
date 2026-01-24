using UnityEngine.Events;
using UnityEngine.UI;

public class ToggleDependee : DependeeUI<Toggle, bool>
{
    protected override UnityEvent<bool> UnityEvent => EventSource.onValueChanged;
    protected override bool GetCurrentValue => EventSource.isOn;
    protected override bool GetEnabled(bool input) => input;
}
