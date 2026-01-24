using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DropdownOptionReference : MonoBehaviour
{
    [field: SerializeField] public TMP_InputField IndexInput { get; private set; }
    [field: SerializeField] public Button IndexDragger { get; private set; }
    [field: SerializeField] public TMP_InputField OptionName { get; private set; }
    [field: SerializeField] public TMP_InputField OptionID { get; private set; }
    [field: SerializeField] public Button DeleteOption { get; private set; }
}
