using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class ScoutingDropdownObject : ScoutingObject<string, ScoutingDropdownObject.ScoutingDropdownSettings>
{
    public override string ObjectTypeIdentifier => nameof(ScoutingDropdownObject);

    TMP_Dropdown dropdown;
    protected override string Value => Settings.dropdownOptions[dropdown.value].optionID;
    protected List<string> GetDropdownOptions
    {
        get
        {
            List<string> options = new();
            for (int i = 0; i < Settings.dropdownOptions.Count; i++)
                options.Add(Settings.dropdownOptions[i].optionName);
            return options;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        dropdown = transform.Find("Dropdown").GetComponent<TMP_Dropdown>();
    }
    public override void ResetValues()
    {
        dropdown.ClearOptions();
        dropdown.AddOptions(GetDropdownOptions);
        base.ResetValues();
    }


    [System.Serializable]
    public class ScoutingDropdownSettings : ScoutingObjectSettings
    {
        [Tooltip("Only used to store information for dropdown")]
        public List<DropdownOptionData> dropdownOptions;

        [System.Serializable]
        public struct DropdownOptionData
        {
            public string optionName;
            public string optionID;

            public DropdownOptionData(string optionName, string optionID)
            {
                this.optionName = optionName;
                this.optionID = optionID;
            }
        }
    }
}
