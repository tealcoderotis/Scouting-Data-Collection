using TMPro;
using UnityEngine;

public class ObjectCreation : MonoBehaviour
{
    [SerializeField] MenuSwapper editorMenuSwapper;
    [SerializeField] private Transform scoutingMenu;
    [Space]
    [SerializeField] IntUIDataGetter intGetter;
    [SerializeField] BoolUIDataGetter boolGetter;
    [SerializeField] DropdownUIDataGetter dropdownGetter;
    [SerializeField] TextUIDataGetter textGetter;
    [Header("Data Source")]
    [SerializeField] TMP_Dropdown subsetDropdown;
    [SerializeField] TMP_Dropdown typeDropdown;

    private ScoutingObject currentObject;
    public int SubsetValue { get => subsetDropdown.value; set => subsetDropdown.@value = value; }
    public TMP_Dropdown.DropdownEvent OnChangeSubset { get => subsetDropdown.onValueChanged; set => subsetDropdown.onValueChanged = value; }

    // Creates new object
    public void ImportObjectSettings(string typeIdentifier, string jsonData, bool setParent = false)
    {
        // can't use ScoutingObject<> as <> require a type, and it could be different things
        Object _object;
        if (intGetter.TypeIdentifier == typeIdentifier) _object = intGetter.prefab;
        else if (boolGetter.TypeIdentifier == typeIdentifier) _object = boolGetter.prefab;
        else if (dropdownGetter.TypeIdentifier == typeIdentifier) _object = dropdownGetter.prefab;
        else if (textGetter.TypeIdentifier == typeIdentifier) _object = textGetter.prefab;
        else
        {
            Debug.LogError($"{typeIdentifier} was invalid");
            return;
        }
        ImportObjectSettings(Instantiate(_object), jsonData, setParent);
    }
    // edits existing object
    public void ImportObjectSettings(Object _object, string jsonData, bool setParent = false)
    {
        if (_object == null) Debug.Log($"_object was null, could not import settings from json data");
        else
        {
            ScoutingObject scoutingObj = ((ScoutingObject)_object);
            scoutingObj.SetSettingsFromJson(jsonData);
            if (setParent)
                scoutingObj.transform.SetParent(scoutingMenu.Find(scoutingObj.GetBaseSettings().sectionName));
            scoutingObj.transform.localScale = Vector3.one;
        }
    }

    public void EnterScoutingObjectEditor() => EnterScoutingObjectEditor(null);
    public void EnterScoutingObjectEditor(ScoutingObject scoutingObject)
    {
        currentObject = scoutingObject;
        editorMenuSwapper.ChangeMenu(name);

        if (currentObject == null)
        {
            typeDropdown.interactable = true;
            return;
        }

        subsetDropdown.value = currentObject.transform.parent.GetSiblingIndex();
        typeDropdown.interactable = false;
        if (currentObject is ScoutingIntObject) typeDropdown.value = 1;
        else if (currentObject is ScoutingBoolObject) typeDropdown.value = 2;
        else if (currentObject is ScoutingDropdownObject) typeDropdown.value = 3;
        else if (currentObject is ScoutingTextObject) typeDropdown.value = 4;

        switch (typeDropdown.value)
        {
            case 1:
                intGetter.SetUIFromData((ScoutingIntObject.ScoutingIntSettings)currentObject.GetSettings());
                break;
            case 2:
                boolGetter.SetUIFromData((ScoutingBoolObject.ScoutingBoolSettings)currentObject.GetSettings());
                break;
            case 3:
                dropdownGetter.SetUIFromData((ScoutingDropdownObject.ScoutingDropdownSettings)currentObject.GetSettings());
                break;
            case 4:
                textGetter.SetUIFromData((ScoutingTextObject.ScoutingTextSettings)currentObject.GetSettings());
                break;
        }
    }
    public void SaveScoutingObject()
    {
        if (currentObject != null) Destroy(currentObject.gameObject);
        switch (typeDropdown.value)
        {
            // 0 is the select type option
            case 1:
                currentObject = Instantiate(intGetter.prefab, scoutingMenu.Find(subsetDropdown.options[subsetDropdown.value].text));
                intGetter.SetDataFromUI((ScoutingIntObject.ScoutingIntSettings)currentObject.GetSettings());
                break;
            case 2:
                currentObject = Instantiate(boolGetter.prefab, scoutingMenu.Find(subsetDropdown.options[subsetDropdown.value].text));
                boolGetter.SetDataFromUI((ScoutingBoolObject.ScoutingBoolSettings)currentObject.GetSettings());
                break;
            case 3:
                currentObject = Instantiate(dropdownGetter.prefab, scoutingMenu.Find(subsetDropdown.options[subsetDropdown.value].text));
                dropdownGetter.SetDataFromUI((ScoutingDropdownObject.ScoutingDropdownSettings)currentObject.GetSettings());
                break;
            case 4:
                currentObject = Instantiate(textGetter.prefab, scoutingMenu.Find(subsetDropdown.options[subsetDropdown.value].text));
                textGetter.SetDataFromUI((ScoutingTextObject.ScoutingTextSettings)currentObject.GetSettings());
                break;
            default: return;
        }
        currentObject.transform.SetSiblingIndex(currentObject.GetBaseSettings().indexInSection);
        editorMenuSwapper.ChangeMenu(0);
    }
}
