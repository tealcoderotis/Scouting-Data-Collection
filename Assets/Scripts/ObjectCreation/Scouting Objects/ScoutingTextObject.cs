using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoutingTextObject : ScoutingObject<string, ScoutingTextObject.ScoutingTextSettings>
{
    public override string ObjectTypeIdentifier => nameof(ScoutingTextObject);

    private TMP_InputField inputField;
    private RectTransform RectTransform => transform as RectTransform;
    private RectTransform InputRectTransform => inputField.transform as RectTransform;
    protected override string Value => inputField.text;


    protected override void Awake()
    {
        base.Awake();
        inputField = transform.Find("Input_Field").GetComponent<TMP_InputField>();
        inputField.transform.GetChild(0).Find("Placeholder").GetComponent<TextMeshProUGUI>().text = Settings.placeholderText;
        inputField.lineType = TMP_InputField.LineType.MultiLineSubmit;

        UpdateSize();
        if (Settings.autoExpand) inputField.onValueChanged.AddListener(AutoUpdateSize);
    }
    public override void ResetValues()
    {
        inputField.text = "";
    }

    // AddListener doesn't accept it if it does not have a string paramater
    protected void AutoUpdateSize(string _) => StartCoroutine(CoroutineUtil.DelayAction(UpdateSize, 1));
    protected void UpdateSize()
    {
        Vector2 size = InputRectTransform.sizeDelta;
        size.y = Mathf.Max(Settings.minimumHeight, inputField.preferredHeight);
        InputRectTransform.sizeDelta = size;
        RectTransform.sizeDelta = new Vector2(RectTransform.sizeDelta.x, size.y + 100);

        //LayoutRebuilder.ForceRebuildLayoutImmediate(transform.parent as RectTransform);
    }


    [System.Serializable]
    public class ScoutingTextSettings : ScoutingObjectSettings
    {
        public string placeholderText;
        public float minimumHeight;
        public bool autoExpand;
    }
}
