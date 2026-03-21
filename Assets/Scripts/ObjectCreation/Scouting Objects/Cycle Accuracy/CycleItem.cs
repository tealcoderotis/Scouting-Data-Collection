using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CycleItem : MonoBehaviour
{
    private GameObject buttonPrefab;
    public string Value => GetValue();
    Transform valueUI;
    Button deleteButton;
    List<GameObject> buttons = new List<GameObject>();
    int currentValue = -1;
    private ScoutingCycleAccuracyObject parentScoutingObject;
    TextMeshProUGUI indexLabel;

    void Awake()
    {
        buttonPrefab = Resources.Load<GameObject>("Prefabs/Button");
        valueUI = transform.Find("Value_UI");
        deleteButton = transform.Find("Delete Button").gameObject.GetComponent<Button>();
        deleteButton.onClick.AddListener(() => Delete());
        indexLabel = transform.Find("Label").gameObject.GetComponent<TextMeshProUGUI>();
    }

    public void CreateButtons(int count)
    {
        for (int i = 1; i <= count; i++)
        {
            CreateButton(i);
        }
    }

    public void SetLabel(int value)
    {
        indexLabel.text = $"Cycle: {value}";
    }

    public void AddParentScoutingObejct(ScoutingCycleAccuracyObject parentScoutingObject)
    {
        this.parentScoutingObject = parentScoutingObject;
    }

    private void CreateButton(int number)
    {
        GameObject btn = Instantiate(buttonPrefab);
        btn.transform.SetParent(valueUI, false);

        btn.GetComponentInChildren<TextMeshProUGUI>().text = number.ToString();

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
            if (i + 1 == value)
            {
                buttons[i].GetComponent<Image>().color = new Color(0f, 1f, 0f, 1f);
            }
            else
            {
                buttons[i].GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            }
        }
    }

    private void Delete()
    {
        DestroyImmediate(gameObject);
        if (parentScoutingObject != null)
        {
            parentScoutingObject.ItemDelete();
        }
    }
}
