using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_InputField))]
public class InputFieldDefault : MonoBehaviour
{
    private TMP_InputField inputField;
    [SerializeField] private string defaultValue;
    private void Awake() => inputField = GetComponent<TMP_InputField>();
    private void OnEnable() => inputField.text = defaultValue;
}
