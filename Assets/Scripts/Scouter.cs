using UnityEngine;
using UnityEngine.UI;

public class Scouter : MonoBehaviour
{
    public static Scouter Instance
    {
        get
        {
            if (_Instance != null) return _Instance;
            _Instance = GameObject.Find("Canvas").transform.Find("Menus").transform.Find("Scouter").GetComponent<Scouter>();
            return _Instance;
        }
    }
    public static Scouter _Instance;

    public RectTransform content;
    [field: SerializeField] public ScrollRect ContentScroll { get; private set; }
    [SerializeField] private MenuSwapper menuSwapper;
    [SerializeField] private Button enterObjectCreation;
    [field: SerializeField] public ObjectCreation ObjectCreation { get; private set; }
    public System.Action<bool> onEnterScouter = delegate { };
    protected bool isEditing;


    private void Awake()
    {
        _Instance = this;
    }
    public void EnableScouter(bool isEditing)
    {
        this.isEditing = isEditing;
        enterObjectCreation.interactable = isEditing;
        menuSwapper.ChangeMenu(name);
        onEnterScouter(isEditing);

        // for some reason sets it as a negative value?
        content.offsetMax = new(-(isEditing ? 220 : 120), 0);
    }

    public void Save()
    {
        MatchSaveManager.Instance.SaveMatch(true);
        ContentScroll.verticalNormalizedPosition = 1;
    }
}
