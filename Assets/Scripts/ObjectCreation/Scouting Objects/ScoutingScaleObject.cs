using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ScoutingScaleObject : ScoutingObject<string, ScoutingScaleObject.ScoutingScaleSettings>
{
    public override string ObjectTypeIdentifier => nameof(ScoutingScaleObject);

    protected override string Value => GetValue();

    Button oneButton;
    Button twoButton;
    Button threeButton;
    Button fourButton;
    Button fiveButton;
    Image oneButtonImage;
    Image twoButtonImage;
    Image threeButtonImage;
    Image fourButtonImage;
    Image fiveButtonImage;
    Image[] buttonImages;

    int currentValue = -1;

    protected override void Awake()
    {
        base.Awake();
        Transform valueUI = transform.Find("Value_UI");
        oneButton = valueUI.Find("1").GetComponent<Button>();
        oneButton.onClick.AddListener(() => ToggleValue(1));
        twoButton = valueUI.Find("2").GetComponent<Button>();
        twoButton.onClick.AddListener(() => ToggleValue(2));
        threeButton = valueUI.Find("3").GetComponent<Button>();
        threeButton.onClick.AddListener(() => ToggleValue(3));
        fourButton = valueUI.Find("4").GetComponent<Button>();
        fourButton.onClick.AddListener(() => ToggleValue(4));
        fiveButton = valueUI.Find("5").GetComponent<Button>();
        fiveButton.onClick.AddListener(() => ToggleValue(5));
        oneButtonImage = valueUI.Find("1").GetComponent<Image>();
        twoButtonImage = valueUI.Find("2").GetComponent<Image>();
        threeButtonImage = valueUI.Find("3").GetComponent<Image>();
        fourButtonImage = valueUI.Find("4").GetComponent<Image>();
        fiveButtonImage = valueUI.Find("5").GetComponent<Image>();
        buttonImages = new Image[] {oneButtonImage, twoButtonImage, threeButtonImage, fourButtonImage, fiveButtonImage};
    }
    public override void ResetValues()
    {
        base.ResetValues();
        SetValue(-1);
    }

    public string GetValue()
    {
        if (currentValue != -1)
        {
            return currentValue.ToString();
        }
        else
        {
            return "";
        }
    }

    private void ToggleValue(int value)
    {
        if (currentValue == value)
        {
            SetValue(-1);
        }
        else
        {
            SetValue(value);
        }
    }

    private void SetValue(int value)
    {
        currentValue = value;
        foreach (Image buttonImage in buttonImages) {
            buttonImage.color = new Color(1f, 1f, 1f, 1f);
        }
        switch (value) {
            case 1:
                buttonImages[0].color = new Color(0f, 1f, 0f, 1f);
                break;
            case 2:
                buttonImages[1].color = new Color(0f, 1f, 0f, 1f);
                break;
            case 3:
                buttonImages[2].color = new Color(0f, 1f, 0f, 1f);
                break;
            case 4:
                buttonImages[3].color = new Color(0f, 1f, 0f, 1f);
                break;
            case 5:
                buttonImages[4].color = new Color(0f, 1f, 0f, 1f);
                break;
        }
    }

    [System.Serializable]
    public class ScoutingScaleSettings : ScoutingObjectSettings
    {

    }
}
