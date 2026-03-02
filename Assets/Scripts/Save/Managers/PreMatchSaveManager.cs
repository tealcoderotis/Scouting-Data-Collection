using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PrematchSaveManager : MonoBehaviour
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
    private void Start()
    {
        LoadPreMatchData();
        scoutingTeamNumber.onEndEdit.AddListener(_ => SavePreMatchData());
        username.onEndEdit.AddListener(_ => SavePreMatchData());
        scoutingIndex.onValueChanged.AddListener(_ => SavePreMatchData());
        rawMatchNumber.onEndEdit.AddListener(_ => SavePreMatchData());
        compLevelDisplay.onValueChanged.AddListener(_ => SavePreMatchData());
        setNumberDisplay.onEndEdit.AddListener(_ => SavePreMatchData());
        matchNumberDisplay.onEndEdit.AddListener(_ => SavePreMatchData());
        scoutedTeamNumber.onEndEdit.AddListener(_ => SavePreMatchData());
    }

    private void SavePreMatchData()
    {
        string scoutingTeam = scoutingTeamNumber.text;
        string scoutingUser = username.text;
        string scoutedTeam = scoutedTeamNumber.text;
        int compLevel = compLevelDisplay.value;
        string rawMatchNumberValue = rawMatchNumber.text;
        string setNumber = setNumberDisplay.text;
        string matchNumber = matchNumberDisplay.text;
        int position = scoutingIndex.value;
        SaveManager.SavePreMatchSettings(new PrematchSaveData(scoutingTeam, scoutingUser, scoutedTeam, compLevel, rawMatchNumberValue,  setNumber, matchNumber, position));
    }

    private void LoadPreMatchData()
    {
        PrematchSaveData? nullableData = SaveManager.LoadPreMatchSettings();
        if (nullableData.HasValue)
        {
            PrematchSaveData data = nullableData.Value;
            scoutingTeamNumber.text = data.scoutingTeam.ToString();
            username.text = data.scoutingUser;
            scoutedTeamNumber.text = data.scoutedTeam.ToString();
            compLevelDisplay.value = data.compLevel;
            rawMatchNumber.text = data.rawMatchNumber.ToString();
            setNumberDisplay.text = data.setNumber.ToString();
            matchNumberDisplay.text = data.matchNumber.ToString();
            scoutingIndex.value = data.scoutingIndex;
        }
    }

    public struct PrematchSaveData
    {
        public string scoutingTeam;
        public string scoutingUser;
        public string scoutedTeam;
        public int compLevel;
        public string rawMatchNumber;
        public string setNumber;
        public string matchNumber;
        public int scoutingIndex;
        public PrematchSaveData(string scoutingTeam, string scoutingUser, string scoutedTeam, int compLevel, string rawMatchNumber, string setNumber, string matchNumber, int scoutingIndex)
        {
            this.scoutingTeam = scoutingTeam;
            this.scoutingUser = scoutingUser;
            this.scoutedTeam = scoutedTeam;
            this.compLevel = compLevel;
            this.rawMatchNumber = rawMatchNumber;
            this.setNumber = setNumber;
            this.matchNumber = matchNumber;
            this.scoutingIndex = scoutingIndex;
        }
    }
}