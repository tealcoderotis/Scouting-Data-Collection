using TMPro;
using UnityEngine;

// name kinda sucks
[RequireComponent(typeof(TMP_InputField))]
public class NumberInputField : MonoBehaviour
{
    TMP_InputField inputField;
    [SerializeField] string defaultString;
    [SerializeField] bool inheritDefault;

    private void Awake()
    {
        inputField = GetComponent<TMP_InputField>();
        inputField.onEndEdit.AddListener(PreventEmpty);
        if (!inheritDefault)
        {
            inputField.text = defaultString;
        }
    }
    private void PreventEmpty(string input)
    {
        if (input == "") inputField.SetTextWithoutNotify(defaultString);
    }
}
