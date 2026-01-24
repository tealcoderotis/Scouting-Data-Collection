using System;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.UI;

public class MenuButtons : MonoBehaviour
{
    private class ButtonReference
    {
        public Button button;
        public TextMeshProUGUI text;
    }
    [Serializable]
    public class ButtonInfo
    {
        public bool interactible;
        public string displayText;
        public UnityEvent _event;
    }
    [Serializable]
    public class MenuInfo
    {
        [HideInInspector] public GameObject gameObject;
        public ButtonInfo[] buttons = new ButtonInfo[BUTTON_COUNT];
    }
    
    [SerializeField] private Transform menusParent;
    private MenuSwapper menuSwapper;
    private const int MENU_COUNT = 7;
    [SerializeField] MenuInfo[] menus = new MenuInfo[MENU_COUNT];
    [SerializeField] private Transform buttonParent;
    private const int BUTTON_COUNT = 5;
    private readonly ButtonReference[] buttons = new ButtonReference[BUTTON_COUNT];

    private void Start()
    {
        Assert.IsTrue(menusParent != null && menusParent.childCount == MENU_COUNT, $"Menus: {menusParent.childCount} != {MENU_COUNT}");
        Assert.IsTrue(buttonParent != null && buttonParent.childCount == BUTTON_COUNT, $"Buttons: {buttonParent.childCount} != {BUTTON_COUNT}");

        menuSwapper = menusParent.GetComponent<MenuSwapper>();
        for (int i = 0; i < BUTTON_COUNT; ++i)
        {
            buttons[i] = new()
            {
                button = buttonParent.GetChild(i).GetComponent<Button>()
            };
            buttons[i].text = buttons[i].button.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        }

        SetMenu(0);
    }

    public void SetMenu(string menuName)
    {
        Transform targetMenu = menuSwapper.transform.Find(menuName);
        if (targetMenu == null) Debug.LogWarning($"Called SetMenu on {name} with invalid name: {menuName}");
        else SetMenu(targetMenu.GetSiblingIndex());
    }
    public void SetMenu(int index)
    {
        if (index < 0 || index >= MENU_COUNT)
        {
            return;
        }

        menuSwapper.ChangeMenu(index);

        for (int i = 0; i < BUTTON_COUNT; ++i)
        {
            buttons[i].button.onClick.RemoveAllListeners();
            buttons[i].button.interactable = menus[index].buttons[i].interactible;
            buttons[i].button.onClick.AddListener(menus[index].buttons[i]._event.Invoke);
            buttons[i].text.text = menus[index].buttons[i].displayText;
        }
    }
}