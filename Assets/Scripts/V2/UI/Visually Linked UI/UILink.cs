using UnityEngine;
using UnityEngine.UI;

public abstract class UILink<UIType, UIValue> : MonoBehaviour where UIType : Selectable
{
    [SerializeField] protected UILink<UIType, UIValue> other;
    protected UIType uiInstance;
    protected abstract UnityEngine.Events.UnityEvent<UIValue> ValueSource { get; }
    public abstract UIValue GetValue();
    private void SetOthersValue(UIValue value) => other.SetValue(value);
    public abstract void SetValue(UIValue value);

    private bool initialized = false;
    public void Initialize()
    {
        if (initialized) return;
        initialized = true;
        if (!TryGetComponent(out uiInstance) || !other.Matches(this))
        {
            Debug.Log($"{name} ({gameObject.GetInstanceID()} did not match {other.name} ({other.gameObject.GetInstanceID()}");
            Destroy(this);
            return;
        }
        ValueSource.AddListener(SetOthersValue);
        if (other.GetInstance() != null) SetValue(other.GetValue());
    }
    protected virtual void Awake()
    {
        Initialize();
    }
    public UIType GetInstance() => uiInstance;

    public bool Matches(UILink<UIType, UIValue> other) => this.other == other;
}
