using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public abstract class ScoutingObject : MonoBehaviour
{
    private Button settingsButton, deleteButton, nullifyButton;
    private TMPro.TextMeshProUGUI valueLabel;
    protected bool nullified = false;
    public abstract string ObjectTypeIdentifier { get; }

    protected virtual void Awake()
    {
        settingsButton = transform.Find("Settings Button").GetComponent<Button>();
        deleteButton = transform.Find("Delete Button").GetComponent<Button>();
        nullifyButton = transform.Find("Nullify Button").GetComponent<Button>();
        nullifyButton.onClick.AddListener(Nullify);
        valueLabel = transform.Find("Label").GetComponent<TMPro.TextMeshProUGUI>();
        Scouter.Instance.onEnterScouter += SetEditorButtons;
        settingsButton.onClick.AddListener(() => Scouter.Instance.ObjectCreation.EnterScoutingObjectEditor(this));
        deleteButton.onClick.AddListener(() => Destroy(gameObject));
    }
    private void OnDestroy() => Scouter.Instance.onEnterScouter -= SetEditorButtons;
    private void SetEditorButtons(bool isEditing)
    {
        settingsButton.gameObject.SetActive(isEditing);
        deleteButton.gameObject.SetActive(isEditing);
        nullifyButton.gameObject.SetActive(!isEditing);
    }

    private void Nullify() {
        nullified = !nullified;
        if (nullified) {
            valueLabel.fontStyle = FontStyles.Strikethrough;
        }
        else {
            valueLabel.fontStyle = FontStyles.Normal;
        }
    }
    
    public virtual void ResetValues() {
        nullified = false;
        valueLabel.fontStyle = FontStyles.Normal;
    }

    public bool Nullified {
        get {
            return nullified;
        }
    }

    public abstract MatchData.ArbritraryData GetMatchData();
    public abstract ScoutingObjectSettings GetBaseSettings();
    public abstract object GetSettings();
    public abstract void SetSettingsFromJson(string json);
    [System.Serializable]
    public abstract class ScoutingObjectSettings
    {
        public string objectIdentifier;
        public string objectName;
        public string sectionName;
        public int indexInSection;

        // these are used just for changing the type specific settings of a scouting object
        public void Import(string json) => JsonUtility.FromJsonOverwrite(json, this);
        public string Export() => JsonUtility.ToJson(this);
    }
}
[System.Serializable]
public abstract class ScoutingObject<EventType, SettingsType> : ScoutingObject where SettingsType : ScoutingObject.ScoutingObjectSettings
{
    [Tooltip("Uses it's own object in order to prevent problems with using json (especially overwrite) with the whole object")]
    [SerializeField] protected SettingsType Settings;
    protected abstract EventType Value { get; }
    public override MatchData.ArbritraryData GetMatchData() => new(Settings.objectIdentifier, typeof(EventType), Value.ToString());
    public override ScoutingObjectSettings GetBaseSettings() => Settings;
    public override object GetSettings() => Settings;
    public override void SetSettingsFromJson(string json)
    {
        JsonUtility.FromJsonOverwrite(json, Settings);
    }

    protected override void Awake()
    {
        base.Awake();
    }
    protected virtual void OnEnable()
    {
        Settings.indexInSection = transform.GetSiblingIndex();
        ResetValues();
    }
    protected virtual void Start()
    {
        transform.Find("Label").GetComponent<TMPro.TextMeshProUGUI>().text = Settings.objectName;
    }
}