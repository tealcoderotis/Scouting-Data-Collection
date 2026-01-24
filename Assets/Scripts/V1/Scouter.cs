using UnityEngine;
using UnityEngine.UI;

public class Scouter : MonoBehaviour
{
    public static Scouter Instance
    {
        get
        {
            if (_Instance != null) return _Instance;
            _Instance = GameObject.Find("Canvas").transform.Find("Menus").transform.Find("Scouting Menu").GetComponent<Scouter>();
            return _Instance;
        }
    }
    private static Scouter _Instance;

    [field: SerializeField] public VerticalLayoutGroup Content { get; private set; }
    [field: SerializeField] public ScrollRect ContentScroll { get; private set; }
    public System.Action<bool> onEnterScouter = delegate { };
    protected bool isEditing;


    private void Awake()
    {
        _Instance = this;
        onEnterScouter += _ =>
        {
            ContentScroll.verticalNormalizedPosition = 1;
        };
    }
    public bool GetIsEditing()
    {
        return isEditing;
    }
    public void SetIsEditing(bool isEditing)
    {
        this.isEditing = isEditing;
        onEnterScouter(isEditing);

        Content.padding.right = isEditing ? 200 : 0;
    }

    public void Save()
    {
        MatchSaveManager.Instance.SaveMatch(true);
        ContentScroll.verticalNormalizedPosition = 1;
        MatchSaveManager.Instance.UpdateOutMatch();
    }
}