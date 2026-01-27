using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ScoutingBoolObject : ScoutingObject<bool, ScoutingBoolObject.ScoutingBoolSettings>
{
    public override string ObjectTypeIdentifier => nameof(ScoutingBoolObject);

    Toggle toggle;
    protected override bool Value => toggle.isOn;
    public override MatchData.ArbritraryData GetMatchData() => new(Settings.objectIdentifier, typeof(bool), (Value ? 1 : 0).ToString());


    protected override void Awake()
    {
        base.Awake();
        toggle = GetComponent<Toggle>();
    }
    public override void ResetValues()
    {
        toggle.isOn = Settings.defaultValue;
    }


    [System.Serializable]
    public class ScoutingBoolSettings : ScoutingObjectSettings
    {
        public bool defaultValue;
    }
}
