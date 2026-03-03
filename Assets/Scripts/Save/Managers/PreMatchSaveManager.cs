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
        scoutingTeamNumber.onValueChanged.AddListener(_ => SavePreMatchData());
        username.onValueChanged.AddListener(_ => SavePreMatchData());
        scoutingIndex.onValueChanged.AddListener(_ => SavePreMatchData());
        rawMatchNumber.onValueChanged.AddListener(_ => SavePreMatchData());
        compLevelDisplay.onValueChanged.AddListener(_ => SavePreMatchData());
        setNumberDisplay.onValueChanged.AddListener(_ => SavePreMatchData());
        matchNumberDisplay.onValueChanged.AddListener(_ => SavePreMatchData());
        scoutedTeamNumber.onValueChanged.AddListener(_ => SavePreMatchData());
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
            scoutingTeamNumber.text = data.scoutingTeam;
            username.text = data.scoutingUser;
            scoutedTeamNumber.text = data.scoutedTeam;
            compLevelDisplay.value = data.compLevel;
            rawMatchNumber.text = data.rawMatchNumber;
            setNumberDisplay.text = data.setNumber;
            matchNumberDisplay.text = data.matchNumber;
            scoutingIndex.value = data.scoutingIndex;
            ScoutingCore.CurrentGlobalMatchIndex = int.Parse(data.rawMatchNumber) - 1;
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