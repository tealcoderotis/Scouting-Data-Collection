using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollingInputField : TMP_InputField
{
    private ScrollRect ScrollRect => Scouter.Instance.ContentScroll;

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left || eventData.dragging) return;
        ActivateInputField();
    }
    public override void OnSelect(BaseEventData eventData) { }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        ScrollRect.OnBeginDrag(eventData);
    }
    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
        ScrollRect.OnDrag(eventData);
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        ScrollRect.velocity = Vector2.zero;
    }
    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        ScrollRect.OnEndDrag(eventData);
    }
}
