using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public abstract class ScoutingObject : MonoBehaviour
{
    private Button settingsButton, deleteButton;
    public string ObjectTypeIdentifier => GetType().Name;

    protected virtual void Awake()
    {
        settingsButton = transform.Find("Settings Button").GetComponent<Button>();
        deleteButton = transform.Find("Delete Button").GetComponent<Button>();
        Scouter.Instance.onEnterScouter += SetEditorButtons;
        settingsButton.onClick.AddListener(() =>
        {
            ScouterObjectCreator.Instance.SetCurrentObject(this);
            ScouterObjectCreator.Instance.SwapMenu(this);
        });
        deleteButton.onClick.AddListener(() => Destroy(gameObject));
    }
    private void OnDestroy()
    {
        if (Application.exitCancellationToken.IsCancellationRequested) return;
        Scouter.Instance.onEnterScouter -= SetEditorButtons;
    }
    protected virtual void SetEditorButtons(bool isEditing)
    {
        settingsButton.gameObject.SetActive(isEditing);
        deleteButton.gameObject.SetActive(isEditing);
    }
    
    public abstract void ResetValues();

    public abstract MatchData.ArbritraryData GetMatchData();
    public abstract ScoutingObjectSettings GetBaseSettings();
    public abstract object GetSettings();
    public abstract void SetSettingsFromJson(string json);
    [System.Serializable]
    public abstract class ScoutingObjectSettings
    {
        public string objectID;
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
    public override MatchData.ArbritraryData GetMatchData() => new(Settings.objectID, typeof(EventType), Value.ToString());
    public override ScoutingObjectSettings GetBaseSettings() => Settings;
    public override object GetSettings() => Settings;
    public SettingsType GetFullSettings() => Settings;
    public override void SetSettingsFromJson(string json)
    {
        JsonUtility.FromJsonOverwrite(json, Settings);
    }
    public void SetSettings(SettingsType settings)
    {
        Settings = settings;
    }

    protected virtual void Start()
    {
        transform.Find("Label").GetComponent<TMPro.TextMeshProUGUI>().text = Settings.objectName;
    }

    protected override void SetEditorButtons(bool isEditing)
    {
        base.SetEditorButtons(isEditing);
        Settings.indexInSection = transform.GetSiblingIndex();
        ResetValues();
    }
}