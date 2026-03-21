using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ScoutingCycleAccuracyObject : ScoutingObject<int, ScoutingCycleAccuracyObject.ScoutingCycleSettings>
{
    private GameObject objectPrefab;
    public override string ObjectTypeIdentifier => nameof(ScoutingCycleAccuracyObject);

    protected override int Value => GetValue();
    Transform cycleUI;
    Button addCycleButton;
    TextMeshProUGUI lengthLabel;

    protected override void Awake()
    {
        objectPrefab = Resources.Load<GameObject>("Prefabs/Cycle Accuracy Item");
        base.Awake();
        cycleUI = transform.Find("Cycles");
        addCycleButton = transform.Find("Bottom UI/Add Cycle Button").gameObject.GetComponent<Button>();
        addCycleButton.onClick.AddListener(() => CreateCycle());
        lengthLabel = transform.Find("Bottom UI/Length Label").gameObject.GetComponent<TextMeshProUGUI>();
        lengthLabel.text = "Length: 0";
    }

    public override void ResetValues()
    {
        for (int i = cycleUI.transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(cycleUI.transform.GetChild(i).gameObject);
        }
        lengthLabel.text = "Length: 0";
        StartCoroutine(LayoutUpdate());
        base.ResetValues();
    }

    public int GetValue()
    {
        return cycleUI.transform.childCount;
    }

    public override List<MatchData> GetCycleAccuracy()
    {
        List<MatchData> cycles = new();
        for (int i = 0; i < cycleUI.transform.childCount; i++)
        {
            MatchData matchData = new();
            matchData.uniqueData.Add(new MatchData.ArbritraryData("cycle_type", typeof(string), objectName));
            matchData.uniqueData.Add(new MatchData.ArbritraryData("cycle_number", typeof(int), i.ToString()));
            matchData.uniqueData.Add(new MatchData.ArbritraryData("cycle_accuracy", typeof(string), cycleUI.transform.GetChild(i).gameObject.GetComponent<CycleItem>().GetValue()));
            cycles.Add(matchData);
        }
        return cycles;
    }

    private void CreateCycle()
    {
        GameObject cycle = Instantiate(objectPrefab);
        cycle.transform.SetParent(cycleUI, false);
        cycle.GetComponent<CycleItem>().CreateButtons(Settings.options);
        cycle.GetComponent<CycleItem>().AddParentScoutingObejct(this);
        lengthLabel.text = $"Length: {cycleUI.transform.childCount}";
        for (int i = 0; i < cycleUI.transform.childCount; i++)
        {
            cycleUI.transform.GetChild(i).gameObject.GetComponent<CycleItem>().SetLabel(i + 1);
        }
        StartCoroutine(LayoutUpdate());
    }

    private System.Collections.IEnumerator LayoutUpdate()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(cycleUI.gameObject.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(gameObject.GetComponent<RectTransform>());
        gameObject.GetComponent<LayoutElement>().enabled = false;
        yield return new WaitForEndOfFrame();
        gameObject.gameObject.GetComponent<LayoutElement>().enabled = true;
    }

    [System.Serializable]
    public class ScoutingCycleSettings : ScoutingObjectSettings
    {
        [SerializeField] public int options = 5;
    }

    public void ItemDelete()
    {
        lengthLabel.text = $"Length: {cycleUI.transform.childCount}";
        for (int i = 0; i < cycleUI.transform.childCount; i++)
        {
            cycleUI.transform.GetChild(i).gameObject.GetComponent<CycleItem>().SetLabel(i + 1);
        }
        StartCoroutine(LayoutUpdate());
    }
}
