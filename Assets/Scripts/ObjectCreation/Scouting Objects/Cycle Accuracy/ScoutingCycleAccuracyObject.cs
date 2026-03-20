using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ScoutingCycleAccuracyObject : ScoutingObject<string, ScoutingCycleAccuracyObject.ScoutingCycleSettings>
{
    private GameObject objectPrefab;
    public override string ObjectTypeIdentifier => nameof(ScoutingCycleAccuracyObject);

    protected override string Value => GetValue();
    Transform cycleUI;
    Button addCycleButton;

    protected override void Awake()
    {
        objectPrefab = Resources.Load<GameObject>("Prefabs/Cycle Accuracy Item");
        base.Awake();
        cycleUI = transform.Find("Cycles");
        addCycleButton = transform.Find("Bottom UI/Add Cycle Button").gameObject.GetComponent<Button>();
        addCycleButton.onClick.AddListener(() => CreateCycle());
    }

    public override void ResetValues()
    {
        for (int i = cycleUI.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(cycleUI.transform.GetChild(i).gameObject);
        }
        base.ResetValues();
    }

    public string GetValue()
    {
        return "";
    }

    private void CreateCycle()
    {
        GameObject cycle = Instantiate(objectPrefab);
        cycle.transform.SetParent(cycleUI, false);
        cycle.GetComponent<CycleItem>().CreateButtons(Settings.options);
    }

    [System.Serializable]
    public class ScoutingCycleSettings : ScoutingObjectSettings
    {
        [SerializeField] public int options = 5;
    }
}
