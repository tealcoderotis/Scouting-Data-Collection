using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PrematchData : MonoBehaviour
{
    [SerializeField] TMP_InputField scoutingTeamNumber;
    [SerializeField] TMP_InputField username;
    [SerializeField] TMP_Dropdown scoutingIndex;
    [Space]
    [SerializeField] TMP_InputField rawMatchNumber;
    [SerializeField] TMP_Dropdown compLevelDisplay;
    [SerializeField] TMP_InputField setNumberDisplay;
    [SerializeField] TMP_InputField matchNumberDisplay;
    [SerializeField] TMP_InputField scoutedTeamNumber;
    int globalMatchNumber;

    private void Start()
    {
        scoutingIndex.value = AppSettingsSaveManager.Instance.CurrentAppSettings.scoutingPositionIndex;
        rawMatchNumber.onEndEdit.AddListener(SetMatchNumber);
        scoutingIndex.onValueChanged.AddListener(_ => SetMatchNumber(globalMatchNumber));
        scoutingIndex.onValueChanged.AddListener(UpdateScoutingPositionIndex);
    }
    private void OnEnable()
    {
        SetMatchNumber(ScoutingCore.EventData?.currentGlobalMatchIndex ?? 0);
    }

    private void UpdateScoutingPositionIndex(int index)
    {
        AppSettingsSaveManager.Instance.CurrentAppSettings.scoutingPositionIndex = index;
    }

    public void UpdateMatchSaveManagerValues(MatchData data)
    {
        data.scoutingTeam = int.Parse(scoutingTeamNumber.text);
        data.scoutingUser = username.text;
        data.scoutedTeam = int.Parse(scoutedTeamNumber.text);
        data.compLevel = System.Enum.Parse<ScoutingCore.CompLevels>(compLevelDisplay.options[compLevelDisplay.value].text);
        data.setNumber = int.Parse(setNumberDisplay.text);
        data.matchNumber = int.Parse(matchNumberDisplay.text);
    }
    public void AutoUpdate() => SetMatchNumber(globalMatchNumber + 1);
    public void SetMatchNumber(string index) => SetMatchNumber(int.Parse(index) - 1);
    public void SetMatchNumber(int index)
    {
        globalMatchNumber = index;
        if (globalMatchNumber >= 0 && globalMatchNumber < ScoutingCore.CurrentEventMatches.Length)
        {
            scoutedTeamNumber.interactable = false;
            APIData.SimpleMatch match = ScoutingCore.CurrentEventMatches[globalMatchNumber];
            compLevelDisplay.value = (int)match.CompLevel;
            setNumberDisplay.text = match.set_number.ToString();
            matchNumberDisplay.text = match.match_number.ToString();
            scoutedTeamNumber.text = match.alliances.GetTeamKey(scoutingIndex.value).ToString().Remove(0, 3);
        }
        else
        {
            matchNumberDisplay.text = (int.Parse(matchNumberDisplay.text) + 1).ToString();
            scoutedTeamNumber.interactable = true;
            scoutedTeamNumber.text = "";
        }
        rawMatchNumber.SetTextWithoutNotify((globalMatchNumber + 1).ToString());
        ScoutingCore.CurrentGlobalMatchIndex = globalMatchNumber;
    }
}
