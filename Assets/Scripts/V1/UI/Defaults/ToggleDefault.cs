using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class ToggleDefault : MonoBehaviour
{
    private Toggle toggle;
    [SerializeField] private bool defaultValue;
    private void Awake() => toggle = GetComponent<Toggle>();
    private void OnEnable() => toggle.isOn = defaultValue;
}
