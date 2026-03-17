using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogManager : MonoBehaviour
{
    private GameObject checkboxPrefab;
    public static DialogManager Instance;
    [SerializeField] private GameObject DialogModal;
    [SerializeField] private GameObject SelectDialog;
    private GameObject CheckBoxContainer;
    private Button CloseButton;
    private MultiSelectObject currentObject;

    void Awake()
    {
        checkboxPrefab = Resources.Load<GameObject>("Prefabs/Checkbox");
        Instance = this;
        CheckBoxContainer = SelectDialog.transform.Find("CheckContainer").gameObject;
        Button CloseButton = SelectDialog.transform.Find("Close").gameObject.GetComponent<Button>();
        CloseButton.onClick.AddListener(() => HideDialog());
    }

    public void ShowSelectDialog(MultiSelectObject scoutingObject)
    {
        for (int i = CheckBoxContainer.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(CheckBoxContainer.transform.GetChild(i).gameObject);
        }
        foreach (MultiSelectObject.MultiSelectItem item in scoutingObject.Items)
        {
            CreateCheckboxInSelectDialog(item.optionName, item.selected);
        }
        SelectDialog.SetActive(true);
        DialogModal.SetActive(true);
        currentObject = scoutingObject;
    }

    private void CreateCheckboxInSelectDialog(string text, bool selected)
    {
        GameObject check = Instantiate(checkboxPrefab);
        check.GetComponentInChildren<TextMeshProUGUI>().text = text;
        check.GetComponent<Toggle>().isOn = selected;
        check.transform.SetParent(CheckBoxContainer.transform, false);
    }

    public void HideDialog()
    {
        List<MultiSelectObject.MultiSelectItem> items = new List<MultiSelectObject.MultiSelectItem>();
        items.AddRange(currentObject.Items);
        for (int i = 0; i < items.Count; i++)
        {
            bool selected = CheckBoxContainer.transform.GetChild(i).gameObject.GetComponent<Toggle>().isOn;
            MultiSelectObject.MultiSelectItem temp = items[i];
            temp.selected = selected;
            items[i] = temp;
        }
        currentObject.SetValues(items);
        DialogModal.SetActive(false);
    }
}
