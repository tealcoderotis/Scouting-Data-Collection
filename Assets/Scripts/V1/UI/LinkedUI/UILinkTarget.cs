using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Selectable))]
public class UILinkTarget : MonoBehaviour
{
    private Selectable selectable;
    [SerializeField] private List<LinkState> UILinks = new();

    private void Awake()
    {
        selectable = GetComponent<Selectable>();
        for (int i = 0; i < UILinks.Count; i++)
        {
            UILinks[i].updateInteractables += UpdateInteractable;
            UILinks[i].Setup(UILinks[i].link);
        }
        UpdateInteractable();
    }

    public LinkState GetLinkState(int index) => index >= 0 && index < UILinks.Count ? UILinks[index] : null;
    public void AddLinks(UILink[] links)
    {
        for (int i = 0; i < links.Length; i++)
            AddLink(links[i], false);
        UpdateInteractable();
    }
    public void AddLink(UILink link, bool doInteractableUpdate = true)
    {
        UILinks.Add(new(link));
        UILinks[^1].updateInteractables += UpdateInteractable;
        if (doInteractableUpdate) UpdateInteractable();
    }
    public void RemoveLink(UILink link, bool alsoDestroyObject = false)
    {
        LinkState linkState = null;
        for (int i = 0; i < UILinks.Count; i++)
            if (UILinks[i].link == link)
                linkState = UILinks[i];
        if (linkState == null)
        {
            return;
        }


        UILinks.Remove(linkState);
        if (alsoDestroyObject) Destroy(linkState.link.gameObject);
        else linkState.updateInteractables -= UpdateInteractable;
        UpdateInteractable();
    }

    public void UpdateInteractable()
    {
        for (int i = 0; i < UILinks.Count; i++)
        {
            if (!UILinks[i].OverridenState)
            {
                selectable.interactable = false;
                return;
            }
        }
        selectable.interactable = true;
    }


    [System.Serializable]
    public class LinkState
    {
        public UILink link;
        public bool state;
        public bool trueIfInactive;
        public bool? @override = null;
        public bool OverridenState
        {
            get
            {
                if (@override != null) return @override.Value;
                if (trueIfInactive && !link.gameObject.activeInHierarchy) return true;
                return state;
            }
        }
        public System.Action updateInteractables = delegate { };

        public LinkState(UILink link)
        {
            if (!Application.isPlaying) return;
            Setup(link);
        }
        // constructors dont run when the thing first gets loaded
        public void Setup(UILink link)
        {
            this.link = link;
            link.onEventCall += SetState;
        }

        public void SetState(bool state)
        {
            if (this.state == state) return;
            
            this.state = state;
            updateInteractables();
        }
    }
}
