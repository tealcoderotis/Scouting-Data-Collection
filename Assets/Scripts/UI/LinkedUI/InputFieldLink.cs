using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class InputFieldLink : UILink<TMP_InputField, string>
{
    [SerializeField] private List<string> invalidInputs;
    protected override UnityEvent<string> UnityEvent => EventSource.onValueChanged;
    protected override string GetCurrentValue => EventSource.text;
    protected override bool GetEnabled(string input) => !invalidInputs.Contains(input);
}