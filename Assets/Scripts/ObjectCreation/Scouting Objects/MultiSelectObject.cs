using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class MultiSelectObject : ScoutingObject<string, MultiSelectObject.MultiSelectObjectSettings>
{
    public override string ObjectTypeIdentifier => nameof(MultiSelectObject);

    Button button;
    protected override string Value => GetSelectedValues();
    private List<MultiSelectItem> items = new List<MultiSelectItem>();
    public MultiSelectItem[] Items 
    {
        get {
            return items.ToArray();
        }
    }

    protected override void Awake()
    {
        base.Awake();
        foreach (MultiSelectObjectSettings.MultiSelectOptionData option in Settings.multiSelectOptions)
        {
            items.Add(new MultiSelectItem(option.optionName, option.optionID, false));
        }
        button = transform.Find("Choose Button").GetComponent<Button>();
        button.onClick.AddListener(() => DialogManager.Instance.ShowSelectDialog(this));
    }
    public override void ResetValues()
    {
        for (int i = 0; i < items.Count; i++)
        {
            MultiSelectItem temp = items[i];
            temp.selected = false;
            items[i] = temp;
        }
        button.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "None";
        base.ResetValues();
    }

    public void SetValues(List<MultiSelectItem> newItems)
    {
        items = newItems;
        string buttonContent = GetSelectedValues();
        if (buttonContent != "")
        {
            button.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = buttonContent;
        }
        else
        {
            button.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "None";
        }
    }

    private string GetSelectedValues()
    {
        string val = "";
        foreach (MultiSelectItem item in items)
        {
            if (item.selected)
            {
                string text = item.optionID;
                val += text + ", ";
            }
        }
        if (val.Length > 0)
        {
            return val.Substring(0, val.Length - 2);
        }
        else
        {
            return "";
        }
    }


    [System.Serializable]
    public class MultiSelectObjectSettings : ScoutingObjectSettings
    {
        [Tooltip("Only used to store information for dropdown")]
        public List<MultiSelectOptionData> multiSelectOptions;

        [System.Serializable]
        public struct MultiSelectOptionData
        {
            public string optionName;
            public string optionID;

            public MultiSelectOptionData(string optionName, string optionID)
            {
                this.optionName = optionName;
                this.optionID = optionID;
            }
        }
    }

    public struct MultiSelectItem
        {
            public string optionName;
            public string optionID;
            public bool selected;

            public MultiSelectItem(string optionName, string optionID, bool selected)
            {
                this.optionName = optionName;
                this.optionID = optionID;
                this.selected = selected;
            }
        }
}
