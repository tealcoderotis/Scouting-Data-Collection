using System;
using TMPro;
using UnityEngine;

public class ScouterObjectCreator : MonoBehaviour
{
    private static ScouterObjectCreator _Instance;
    public static ScouterObjectCreator Instance
    {
        get
        {
            if (_Instance != null) return _Instance;
            return _Instance = FindAnyObjectByType<ScouterObjectCreator>(FindObjectsInactive.Include);
        }
    }

    public static readonly Type[] TypeIndexes = new Type[4]
    {
        typeof(ScoutingBoolObject), typeof(ScoutingDropdownObject), typeof(ScoutingIntObject), typeof(ScoutingTextObject)
    };
    public static readonly Type[] TypeSettings = new Type[4]
    {
        typeof(ScoutingBoolObject.ScoutingBoolSettings), typeof(ScoutingDropdownObject.ScoutingDropdownSettings),
        typeof(ScoutingIntObject.ScoutingIntSettings), typeof(ScoutingTextObject.ScoutingTextSettings)
    };
    [SerializeField] private MenuButtons mainMenuSwapper;
    [SerializeField] private MenuSwapper creatorMenuSwapper;
    [Space]
    [SerializeField] private ScouterObjectCreationUI[] menus;
    private int _CurrentUIIndex = 0;
    public int CurrentUIIndex
    {
        get
        {
            return _CurrentUIIndex;
        }
        set
        {
            _CurrentUIIndex = value;
            menus[_CurrentUIIndex].ResetSpecificUI();
        }
    }

    private ScoutingObject currentObject;
    [SerializeField] private TMP_Dropdown sectionSelector;
    private Transform sectionTransform;
    [SerializeField] private TMP_InputField sectionIndexInput;
    private TextMeshProUGUI indexInputPlaceholder;
    private int sectionIndex;
    [SerializeField] private Transform scouterContent;
    [SerializeField] private TMP_Dropdown typeSelect;

    private void Awake()
    {
        indexInputPlaceholder = sectionIndexInput.transform.Find("Text Area").Find("Placeholder").GetComponent<TextMeshProUGUI>();

        sectionTransform = scouterContent.Find(sectionSelector.options[sectionSelector.value].text);
        sectionSelector.onValueChanged.AddListener(SetIndexToLast);
        SetIndexToLast(CurrentUIIndex);
        sectionIndexInput.onEndEdit.AddListener(str =>
        {
            sectionIndex = str.Length == 0 ? 0 : int.Parse(str);
            if (sectionIndex < 0) sectionIndex = 0;
            else if (sectionIndex > sectionTransform.childCount) sectionIndex = sectionTransform.childCount;
            else return;

            sectionIndexInput.SetTextWithoutNotify(sectionIndex.ToString());
        });

        void SetIndexToLast(int menuIndex)
        {
            sectionTransform = scouterContent.Find(sectionSelector.options[menuIndex].text);
            sectionIndex = sectionTransform.childCount;
            sectionIndexInput.SetTextWithoutNotify(sectionIndex.ToString());
            indexInputPlaceholder.text = sectionIndex.ToString();
        }
    }
    private void OnEnable()
    {
        if (currentObject == null) menus[CurrentUIIndex].ResetBaseUI();
        else menus[CurrentUIIndex].LoadValues(currentObject);
        typeSelect.interactable = currentObject == null;
    }
    private void OnDisable()
    {
        currentObject = null;
    }
    public void SetCurrentObject(ScoutingObject scoutingObject)
    {
        currentObject = scoutingObject;
    }
    public void SwapMenu(ScoutingObject scoutingObject)
    {
        int index = 0;
        for (; index < TypeIndexes.Length; index++)
        {
            if (TypeIndexes[index] == scoutingObject.GetType())
            {
                break;
            }
        }

        CurrentUIIndex = index;
        mainMenuSwapper.SetMenu("Editing Menu");
    }
    public void Save()
    {
        if (currentObject == null) currentObject = menus[CurrentUIIndex].CreateInstance();
        else menus[CurrentUIIndex].ApplyValues(currentObject);
        currentObject.transform.SetParent(sectionTransform, false);
        currentObject.transform.SetSiblingIndex(sectionIndex);
        currentObject = null;
    }

    public void Clear()
    {
        currentObject = null;
        typeSelect.interactable = true;
        menus[CurrentUIIndex].ResetBaseUI();
        menus[CurrentUIIndex].ResetSpecificUI();
    }

    public string GetID(string name, bool includeSeection)
    {
        return includeSeection ? $"{sectionSelector.options[sectionSelector.value].text}_{name.Replace(" ", "_")}" : name.Replace(" ", "_");
    }
    public void ListenForSection(NameLinkedIDInput input) => sectionSelector.onValueChanged.AddListener(input.UpdateID);
    public void StopListenForSection(NameLinkedIDInput input) => sectionSelector.onValueChanged.RemoveListener(input.UpdateID);

    public void PreviousMenu()
    {
        mainMenuSwapper.SetMenu(gameObject.activeInHierarchy ? 0 : 2);
    }

    public void ImportScoutingObject(string typeName, string json)
    {
        for (int i = 0; i < TypeIndexes.Length; ++i)
        {
            if (TypeIndexes[i].Name != typeName) continue;
            ScoutingObject @object = menus[i].CreateInstance();
            @object.SetSettingsFromJson(json);
            @object.transform.SetParent(scouterContent.Find(@object.GetBaseSettings().sectionName));
            @object.transform.localScale = Vector3.one;
            return;
        }
    }
}