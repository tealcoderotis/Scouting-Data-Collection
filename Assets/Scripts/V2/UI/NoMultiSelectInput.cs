using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_InputField))]
public class NoMultiSelectInput : MonoBehaviour
{
    TMP_InputField inputField;
    string prevText = string.Empty;
    private void Awake()
    {
        inputField = GetComponent<TMP_InputField>();
        inputField.onFocusSelectAll = false;
    }
    private void Update()
    {
        if (inputField.text.Length < prevText.Length) inputField.text = prevText;
        else prevText = inputField.text;
        if (inputField.isFocused && inputField.selectionAnchorPosition != inputField.selectionFocusPosition)
        {
            inputField.caretPosition = inputField.selectionFocusPosition;
        }
    }
}
