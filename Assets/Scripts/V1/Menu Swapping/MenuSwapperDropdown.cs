using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Dropdown))]
public class MenuSwapperDropdown : MonoBehaviour
{
    private TMP_Dropdown dropdown;
    [SerializeField] private MenuSwapper target;

    private void Awake()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        dropdown.onValueChanged.AddListener((index) => target.ChangeMenu(index));
    }
    private void Start()
    {
        UpdateDropdown();
    }
    public void UpdateDropdown()
    {
        if (target == null)
        {
            Debug.Log($"Target MenuSwapper on {name} is null");
            return;
        }
        dropdown.options.Clear();
        for (int i = 0; i < target.transform.childCount; i++)
            dropdown.options.Add(new(target.transform.GetChild(i).name));
    }
}
