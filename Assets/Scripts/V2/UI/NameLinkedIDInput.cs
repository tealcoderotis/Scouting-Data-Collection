using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_InputField))]
public class NameLinkedIDInput : MonoBehaviour
{
    TMP_InputField idInputField;
    TextMeshProUGUI placeholder;
    [SerializeField] TMP_InputField nameInputField;
    [SerializeField] private bool includeSection;

    private void Start()
    {
        idInputField = GetComponent<TMP_InputField>();
        placeholder = idInputField.transform.Find("Text Area").Find("Placeholder").GetComponent<TextMeshProUGUI>();
        nameInputField.onValueChanged.AddListener(UpdateID);
        UpdateID(nameInputField.text);
        ScouterObjectCreator.Instance.ListenForSection(this);
    }
    private void OnDestroy()
    {
        nameInputField.onValueChanged.RemoveListener(UpdateID);
        ScouterObjectCreator.Instance.StopListenForSection(this);
    }

    public void UpdateID(int _) => UpdateID(nameInputField.text);
    private void UpdateID(string name)
    {
        placeholder.text = ScouterObjectCreator.Instance.GetID(name, includeSection);
    }
}
