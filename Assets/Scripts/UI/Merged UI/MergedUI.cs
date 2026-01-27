using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class MergedUI<Graphic, SubmitEvent, EventValue> : MonoBehaviour where Graphic : Selectable
                                                                                 where SubmitEvent : UnityEvent<EventValue>
{
    protected Graphic thisGraphic;
    [SerializeField] protected Graphic otherGraphic;
    protected abstract EventValue Value(Graphic source);
    protected abstract void SetWithoutNotify(Graphic source, EventValue value);
    protected abstract SubmitEvent GetEvent(Graphic source);

    private void Awake()
    {
        thisGraphic = GetComponent<Graphic>();
    }
    private void Start()
    {
        GetEvent(thisGraphic).AddListener(value => SetWithoutNotify(otherGraphic, value));
        GetEvent(otherGraphic).AddListener(value => SetWithoutNotify(thisGraphic, value));
    }
}
