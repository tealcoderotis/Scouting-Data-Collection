using System;
using System.Collections.Generic;
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
    protected bool isCycleable => Settings.cycleable;

    protected DateTime lastPressTime;
    protected bool firstCycle = true;
    protected List<TimeSpan> cycleTimes;

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

        if (isCycleable)
        {
            lastPressTime = DateTime.Now;
            firstCycle = true;
            cycleTimes = new();
        }
    }
    public override void ResetValues()
    {
        SetCurrentValue(Settings.defaultValue);
        base.ResetValues();
        if (isCycleable)
        {
            lastPressTime = DateTime.Now;
            firstCycle = true;
            cycleTimes = new();
        }
    }

    public void ParseTextInput(string input) => SetCurrentValue(input == "" ? Settings.defaultValue : int.Parse(input));

    public void DecrementValue() => DeCycle();
    public void IncrementValue() => Cycle();
    public void Cycle()
    {
        if (isCycleable)
        {
            DateTime currentTime = DateTime.Now;
            if (!firstCycle)
            {
                Debug.Log("Cycle time: " + (currentTime - lastPressTime).TotalSeconds + " s");
                cycleTimes.Add(currentTime - lastPressTime);
            }
            else
            {
                firstCycle = false;
            }
            lastPressTime = currentTime;
        }
        ChangeCurrentValue(1);
    }
    public void DeCycle()
    {
        if (isCycleable)
        {
            DateTime currentTime = DateTime.Now;
            if (cycleTimes.Count > 0)
            {
                cycleTimes.RemoveAt(cycleTimes.Count - 1);
            }
            lastPressTime = currentTime;
        }
        ChangeCurrentValue(-1);
    }
    public void ChangeCurrentValue(string change) => SetCurrentValue(currentValue + int.Parse(change));
    public void ChangeCurrentValue(int change) => SetCurrentValue(currentValue + change);
    public void SetCurrentValue(string value) => SetCurrentValue(int.Parse(value));
    public void SetCurrentValue(int value)
    {
        currentValue = System.Math.Clamp(value, ActiveMinValue, ActiveMaxValue);
        valueDisplay.SetTextWithoutNotify(currentValue.ToString());
    }
    public override List<MatchData> GetCycles()
    {
        if (isCycleable)
        {
            List<MatchData> cycles = new();
            for (int i = 0; i < cycleTimes.Count; i++)
            {
                MatchData matchData = new();
                matchData.uniqueData.Add(new MatchData.ArbritraryData("cycle_type", typeof(string), objectName));
                matchData.uniqueData.Add(new MatchData.ArbritraryData("cycle_number", typeof(int), i.ToString()));
                matchData.uniqueData.Add(new MatchData.ArbritraryData("cycle_time", typeof(System.DateTime), cycleTimes[i].TotalMilliseconds.ToString()));
                cycles.Add(matchData);
            }
            return cycles;
        }
        else
        {
            return null;
        }
    }


    [System.Serializable]
    public class ScoutingIntSettings : ScoutingObjectSettings
    {
        [SerializeField] public int defaultValue;
        [SerializeField] public bool hasMinValue;
        [SerializeField] public int minValue;
        [SerializeField] public bool hasMaxValue;
        [SerializeField] public int maxValue;
        [SerializeField] public bool cycleable;
    }
}
