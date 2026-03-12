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
    private List<int> selectedOptions = new List<int>();
    protected List<string> GetDropdownOptions
    {
        get
        {
            List<string> options = new();
            for (int i = 0; i < Settings.multiSelectOptions.Count; i++)
                options.Add(Settings.multiSelectOptions[i].optionName);
            return options;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        button = transform.Find("Choose Button").GetComponent<Button>();
        button.onClick.AddListener(() => DialogManager.Instance.ShowSelectDialog(this));
    }
    public override void ResetValues()
    {
        GetComponentInChildren<Text>();
        button.GetComponentInChildren<TMP_Text>().text = "None";
        selectedOptions = new List<int>();
        base.ResetValues();
    }

    public void SetValues(List<int> selectedOptions)
    {
        this.selectedOptions = selectedOptions;
    }

    private string GetSelectedValues()
    {
        string val = "";
        foreach (int option in selectedOptions)
        {
            string text = GetDropdownOptions[option];
            val += text + ", ";
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
}
