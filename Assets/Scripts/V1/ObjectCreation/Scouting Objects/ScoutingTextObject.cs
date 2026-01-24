using TMPro;
using UnityEngine;

public class ScoutingTextObject : ScoutingObject<string, ScoutingTextObject.ScoutingTextSettings>
{
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
    }
    protected override void Start()
    {
        base.Start();
        UpdateSize();
        if (Settings.autoExpands) inputField.onValueChanged.AddListener(AutoUpdateSize);
    }
    public override void ResetValues()
    {
        inputField.text = string.Empty;
    }

    // AddListener doesn't accept it if it does not have a string paramater
    protected void AutoUpdateSize(string _)
    {
        inputField.ForceLabelUpdate();
        UpdateSize();
    }
    protected void UpdateSize()
    {
        Vector2 size = InputRectTransform.sizeDelta;
        if (!Settings.autoExpands)
        {
            size.y = Settings.minimumHeight;
        }
        else if (Settings.maximiumHeight > Settings.minimumHeight)
        {
            size.y = Mathf.Clamp(inputField.preferredHeight, Settings.minimumHeight, Settings.maximiumHeight);
        }
        else
        {
            size.y = Mathf.Max(Settings.minimumHeight, inputField.preferredHeight);
        }
        InputRectTransform.sizeDelta = size;
        size.y += 80;
        RectTransform.sizeDelta = size;
    }


    [System.Serializable]
    public class ScoutingTextSettings : ScoutingObjectSettings
    {
        public string placeholderText;
        public float minimumHeight;
        public bool autoExpands;
        public float maximiumHeight;
    }
}
