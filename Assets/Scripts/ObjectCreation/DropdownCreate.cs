using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DropdownCreate : UILink<int>
{
    protected override int GetCurrentValue => DropdownOptions.Count;
    // if not active in hierarchy, dropdown isn't selected and thus this doesn't matter.
    protected override bool GetEnabled(int input) => input >= 2 || !gameObject.activeInHierarchy;

    [SerializeField] private UILinkTarget saveButton;
    [SerializeField] private Scrollbar scrollbar;
    [SerializeField] private GameObject prefab;
    [Space]
    private DropdownOption dropdownOptionCreator;
    [field: SerializeField] public List<DropdownOption> DropdownOptions { get; private set; } = new();


    protected override void Awake()
    {
        base.Awake();

        dropdownOptionCreator = new(Instantiate(prefab, transform));
        dropdownOptionCreator.IndexInput.interactable = false;
        dropdownOptionCreator.IndexDrag.interactable = false;
        dropdownOptionCreator.DeleteOption.interactable = false;
        dropdownOptionCreator.NameInput.onEndEdit.AddListener((text) =>
        {
            if (text.Length == 0) return;
            CreateDropdownOption(text, -1);
            ForceUpdateOptionCreatore();
        });
    }
    private void OnEnable()
    {
        ForceUpdate();
    }
    private void OnDisable()
    {
        for (int i = DropdownOptions.Count - 1; i >= 0; i--)
            RemoveDropdownOption(i);
        ForceUpdate();
    }


    public void CreateDropdownOption(string initialText, int indexTextOffset)
    {
        DropdownOption dropdownOption = new(Instantiate(prefab, transform));
        DropdownOptions.Add(dropdownOption);

        dropdownOption.IndexInput.onValidateInput += (input, charIndex, addedChar) => char.IsNumber(addedChar) ? addedChar : '\0';
        dropdownOption.IndexInput.onValueChanged.AddListener(indexStr =>
        {
            if (indexStr.Length == 0) indexStr = "0";
            else indexStr = $"{System.Math.Clamp(int.Parse(indexStr), 0, transform.childCount - 2)}";
            dropdownOption.IndexInput.SetTextWithoutNotify(indexStr);
        });
        dropdownOption.IndexInput.onEndEdit.AddListener(newIndexStr =>
        {
            int newIndex = int.Parse(newIndexStr);

            dropdownOption.IndexInput.transform.parent.SetSiblingIndex(newIndex);
            DropdownOptions.Remove(dropdownOption);
            DropdownOptions.Insert(newIndex, dropdownOption);

            for (int i = 0; i < DropdownOptions.Count; i++)
                DropdownOptions[i].UpdateIndexInputText();
        });
        dropdownOption.DeleteOption.onClick.AddListener(() =>
        {
            RemoveDropdownOption(dropdownOption.GetSiblingIndex());
            FixInteractability();
        });

        dropdownOption.UpdateIndexInputText(indexTextOffset);
        dropdownOption.NameInput.SetTextWithoutNotify(initialText);
        saveButton.AddLink(dropdownOption.NameInputLink);
        dropdownOption.NameInputLink.ForceUpdate();
        FixInteractability();

        // for some reason, if it's at 0 then it doesnt do the scrollbar.value
        // meaning it stops being at 0 instead of staying at zero. this prevents that.
        if (scrollbar.value == 0) scrollbar.value = float.Epsilon;
        StartCoroutine(CoroutineUtil.DelayAction(() => scrollbar.value = 0, 1));
        ForceUpdate();
    }
    private void RemoveDropdownOption(int index)
    {
        saveButton.RemoveLink(DropdownOptions[index].NameInputLink);
        Destroy(DropdownOptions[index].IndexInput.transform.parent.gameObject);
        for (int i = index; i < DropdownOptions.Count; i++)
            DropdownOptions[i].UpdateIndexInputText(i >= index ? -1 : 0);
        DropdownOptions.RemoveAt(index);

        ForceUpdate();
    }
    public void ForceUpdateOptionCreatore()
    {
        dropdownOptionCreator.NameInput.text = string.Empty;
        dropdownOptionCreator.IndexInput.transform.parent.SetAsLastSibling();
    }

    private void FixInteractability()
    {
        bool shouldBeInteractible = DropdownOptions.Count >= 2;
        for (int i = 0; i < DropdownOptions.Count; i++)
        {
            DropdownOptions[i].IndexInput.interactable = shouldBeInteractible;
            DropdownOptions[i].IndexDrag.interactable = shouldBeInteractible;
            DropdownOptions[i].DeleteOption.interactable = true;
        }
    }


    [System.Serializable]
    public struct DropdownOption
    {
        [field: SerializeField] public TMP_InputField IndexInput { get; private set;  }
        [field: SerializeField] public Button IndexDrag { get; private set; }
        [field: SerializeField] public TMP_InputField NameInput { get; private set; }
        public UILink NameInputLink { get; private set; }
        [field: SerializeField] public Button DeleteOption { get; private set; }

        public DropdownOption(GameObject instance) : this(instance.transform.Find("Index Setter").GetComponent<TMP_InputField>(),
                                                          instance.transform.Find("Mover").GetComponent<Button>(),
                                                          instance.transform.Find("Name Input").GetComponent<TMP_InputField>(),
                                                          instance.transform.Find("Delete Option").GetComponent<Button>())
        { }
        public DropdownOption(TMP_InputField indexInput, Button indexDrag, TMP_InputField nameInput, Button deleteOption)
        {
            IndexInput = indexInput;
            IndexDrag = indexDrag;
            NameInput = nameInput;
            NameInputLink = NameInput.GetComponent<UILink>();
            DeleteOption = deleteOption;
        }

        public readonly int GetSiblingIndex() => IndexInput.transform.parent.GetSiblingIndex();
        public readonly void SetSiblingIndex(int index) => IndexInput.transform.parent.SetSiblingIndex(index);
        // destroy does not run instantly, so if I call this right after destroying it gives innacurate results
        public readonly void UpdateIndexInputText(int offset = 0) =>
            IndexInput.SetTextWithoutNotify((GetSiblingIndex() + offset).ToString());
    }
}
