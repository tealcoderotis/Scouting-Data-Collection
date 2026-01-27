using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ScoutingIntObject : ScoutingObject<int, ScoutingIntObject.ScoutingIntSettings>
{
    public override string ObjectTypeIdentifier => nameof(ScoutingIntObject);

    TMP_InputField valueDisplay;
    Button decrementButton;
    Button incrementButton;

    int currentValue;
    protected override int Value => currentValue;
    protected int ActiveMinValue => Settings.hasMinValue ? Settings.minValue : int.MinValue;
    protected int ActiveMaxValue => Settings.hasMaxValue ? Settings.maxValue : int.MaxValue;


    protected override void Awake()
    {
        base.Awake();
        Transform valueUI = transform.Find("Value_UI");
        valueDisplay = valueUI.Find("Input_Field").GetComponent<TMP_InputField>();
        decrementButton = valueUI.Find("Decrement_Button").GetComponent<Button>();
        incrementButton = valueUI.Find("Increment_Button").GetComponent<Button>();

        valueDisplay.onEndEdit.AddListener(ParseTextInput);
        decrementButton.onClick.AddListener(DecrementValue);
        incrementButton.onClick.AddListener(IncrementValue);

    }
    public override void ResetValues()
    {
        SetCurrentValue(Settings.defaultValue);
    }

    public void ParseTextInput(string input) => SetCurrentValue(input == "" ? Settings.defaultValue : int.Parse(input));

    public void DecrementValue() => ChangeCurrentValue(-1);
    public void IncrementValue() => ChangeCurrentValue(1);
    public void ChangeCurrentValue(string change) => SetCurrentValue(currentValue + int.Parse(change));
    public void ChangeCurrentValue(int change) => SetCurrentValue(currentValue + change);
    public void SetCurrentValue(string value) => SetCurrentValue(int.Parse(value));
    public void SetCurrentValue(int value)
    {
        currentValue = System.Math.Clamp(value, ActiveMinValue, ActiveMaxValue);
        valueDisplay.SetTextWithoutNotify(currentValue.ToString());
    }


    [System.Serializable]
    public class ScoutingIntSettings : ScoutingObjectSettings
    {
        [SerializeField] public int defaultValue;
        [SerializeField] public bool hasMinValue;
        [SerializeField] public int minValue;
        [SerializeField] public bool hasMaxValue;
        [SerializeField] public int maxValue;
    }
}
