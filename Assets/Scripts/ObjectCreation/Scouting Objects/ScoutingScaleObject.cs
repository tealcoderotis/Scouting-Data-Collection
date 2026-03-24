using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ScoutingScaleObject : ScoutingObject<string, ScoutingScaleObject.ScoutingScaleSettings>
{
    private GameObject buttonPrefab;
    public override string ObjectTypeIdentifier => nameof(ScoutingScaleObject);

    protected override string Value => GetValue();
    Transform valueUI;
    List<GameObject> buttons = new List<GameObject>();

    int currentValue = -1;

    protected override void Awake()
    {
        buttonPrefab = Resources.Load<GameObject>("Prefabs/Button");
        base.Awake();
        valueUI = transform.Find("Value_UI");
        for (int i = 0; i < Settings.options; i++)
        {
            CreateButton(i * (100 / (Settings.options - 1)));
        }
    }

    public override void ResetValues()
    {
        base.ResetValues();
        SetValue(-1);
    }

    private void CreateButton(int number)
    {
        GameObject btn = Instantiate(buttonPrefab);
        btn.transform.SetParent(valueUI, false);

        btn.GetComponentInChildren<TextMeshProUGUI>().text = number.ToString() + "%";

        btn.GetComponent<Button>().onClick.AddListener(() => ToggleValue(number));
        buttons.Add(btn);
    }

    public string GetValue()
    {
        if (currentValue != -1)
        {
            return currentValue.ToString();
        }
        else
        {
            return "";
        }
    }

    private void ToggleValue(int value)
    {
        if (currentValue == value)
        {
            SetValue(-1);
        }
        else
        {
            SetValue(value);
        }
    }

    private void SetValue(int value)
    {
        currentValue = value;
        for (int i = 0; i < buttons.Count(); i++) {
            if (i * (100 / (buttons.Count() - 1)) == value)
            {
                buttons[i].GetComponent<Image>().color = new Color(0f, 1f, 0f, 1f);
            }
            else
            {
                buttons[i].GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            }
        }
    }

    [System.Serializable]
    public class ScoutingScaleSettings : ScoutingObjectSettings
    {
        [SerializeField] public int options = 5;
    }
}
