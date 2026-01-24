using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DropdownDependee : DependeeUI<TMP_Dropdown, int>
{
    protected override UnityEvent<int> UnityEvent => EventSource.onValueChanged;
    protected override int GetCurrentValue => EventSource.value;
    protected override bool GetEnabled(int input) => EventSource.value != 0;
}
