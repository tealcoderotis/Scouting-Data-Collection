using UnityEngine;
using UnityEngine.UI;

// In order to allow lists that include every type of link
public abstract class DependeeUI : MonoBehaviour
{
    public System.Action<bool> onEventCall = delegate { };
    public abstract void ForceUpdate();
}
// general class that needs to be set up, but allows use without requiring a UI element
public abstract class DependeeUI<ConditionType> : DependeeUI
{
    protected abstract ConditionType GetCurrentValue { get; }
    protected abstract bool GetEnabled(ConditionType input);
    protected void Listener(ConditionType value) => onEventCall(GetEnabled(value));
    protected UnityEngine.Events.UnityAction<ConditionType> UnityAction;
    protected virtual void Awake() => UnityAction = new(Listener);

    public override void ForceUpdate() => Listener(GetCurrentValue);
}
// automatically sets everything up, but requires UI element, 
public abstract class DependeeUI<UnityEventSource, ConditionType> : DependeeUI<ConditionType> where UnityEventSource : Selectable
{
    protected UnityEventSource EventSource { get; private set; }
    protected abstract UnityEngine.Events.UnityEvent<ConditionType> UnityEvent { get; }
    protected override void Awake()
    {
        base.Awake();
        if (TryGetComponent(out UnityEventSource source)) EventSource = source;
        else
        {
            Debug.LogWarning($"{gameObject.name} did not have the componenet. Auto-adding it");
            EventSource = gameObject.AddComponent<UnityEventSource>();
        }
        UnityEvent.AddListener(UnityAction);
        ForceUpdate();
    }
}
