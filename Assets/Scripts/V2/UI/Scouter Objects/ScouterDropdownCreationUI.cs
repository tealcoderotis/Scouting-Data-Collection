using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScouterDropdownCreationUI : ScouterObjectCreationUI<ScoutingDropdownObject>
{
    public override void ResetSpecificUI()
    {
        InitCreator();
        creator.OptionName.SetTextWithoutNotify(string.Empty);
        for (int i = content.childCount - 1; i >= 1; --i)
        {
            Destroy(content.GetChild(i).gameObject);
        }
        dropdownOptions.Clear();
    }
    public override void LoadValues(ScoutingDropdownObject obj)
    {
        base.LoadValues(obj);
        for (int i = 0; i < obj.GetFullSettings().dropdownOptions.Count; ++i)
        {
            DropdownOptionReference created = Instantiate(optionPrefab, content);
            InitOption(created);
            created.IndexInput.text = i.ToString();
            created.OptionName.text = obj.GetFullSettings().dropdownOptions[i].optionName;
            created.OptionID.text = obj.GetFullSettings().dropdownOptions[i].optionID;
        }
    }
    public override void ApplyValues(ScoutingDropdownObject obj)
    {
        ScoutingDropdownObject.ScoutingDropdownSettings newSettings = GetAsSettings<ScoutingDropdownObject.ScoutingDropdownSettings>();
        newSettings.dropdownOptions = new();
        for (int i = 1; i < content.transform.childCount; ++i)
        {
            string name = dropdownOptions[i - 1].OptionName.text;
            string id = dropdownOptions[i - 1].OptionID.text.Length == 0 ? ScouterObjectCreator.Instance.GetID(name, false) : dropdownOptions[i - 1].OptionID.text;
            newSettings.dropdownOptions.Add(new(name, id));
        }
        obj.SetSettings(newSettings);
    }


    [SerializeField] private Transform content;
    [SerializeField] private DropdownOptionReference optionPrefab;
    private DropdownOptionReference creator;
    private readonly List<DropdownOptionReference> dropdownOptions = new();

    private void Awake()
    {
        InitCreator();
    }

    public void InitCreator()
    {
        if (creator != null) return;
        creator = content.GetChild(0).GetComponent<DropdownOptionReference>();
        creator.IndexInput.interactable = false;
        creator.IndexDragger.interactable = false;
        creator.OptionName.interactable = true;
        creator.OptionID.interactable = false;
        creator.DeleteOption.interactable = true;

        creator.OptionName.onSubmit.AddListener(MakeSibling);
        creator.DeleteOption.onClick.AddListener(() =>
        {
            creator.OptionName.SetTextWithoutNotify("");
            creator.OptionID.text = "";
        });
    }
    protected void MakeSibling(string name)
    {
        DropdownOptionReference newOption = Instantiate(optionPrefab, content);
        InitOption(newOption);
        newOption.OptionName.text = creator.OptionName.text;
        newOption.OptionID.transform.Find("Text Area").Find("Placeholder").GetComponent<TextMeshProUGUI>().text = creator.OptionName.text.Replace(" ", "_");
        newOption.IndexInput.text = (content.childCount - 2).ToString();
        creator.OptionName.text = "";
    }
    public void InitOption(DropdownOptionReference option)
    {
        option.IndexInput.interactable = true;
        option.IndexDragger.interactable = false;
        option.OptionName.interactable = true;
        option.OptionID.interactable = true;
        option.DeleteOption.interactable = true;

        option.IndexInput.onSubmit.AddListener((string index) =>
        {
            int prevIndex = option.transform.GetSiblingIndex();
            int newIndex = int.Parse(index) + 1;
            if (newIndex < 1) newIndex = 1;
            else if (newIndex >= content.childCount) newIndex = content.childCount - 1;
            option.transform.SetSiblingIndex(newIndex);
            if (prevIndex < newIndex) FixMovedOptions(prevIndex, newIndex);
            else FixMovedOptions(newIndex, prevIndex);
        });
        option.DeleteOption.onClick.AddListener(() =>
        {
            int index = option.transform.GetSiblingIndex();
            Destroy(option.gameObject);
            FixMovedOptions(index, transform.childCount);
        });

        dropdownOptions.Add(option);
    }

    protected void FixMovedOptions(int start, int end)
    {
        if (start < 1) start = 1;
        if (end >= content.childCount) end = content.childCount - 1;
        for (int i = start; i <= end; ++i)
        {
            dropdownOptions[i - 1] = content.GetChild(i).GetComponent<DropdownOptionReference>();
            dropdownOptions[i - 1].IndexInput.SetTextWithoutNotify((i - 1).ToString());
        }
    }
}
