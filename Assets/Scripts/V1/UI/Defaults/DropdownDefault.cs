using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Dropdown))]
public class DropdownDefault : MonoBehaviour
{
    private TMP_Dropdown dropdown;
    [SerializeField] private int defaultIndex = 0;
    private void Awake() => dropdown = GetComponent<TMP_Dropdown>();
    private void OnEnable() => dropdown.value = defaultIndex;
}
