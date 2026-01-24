using UnityEngine;

public class MatchSaveManager : MonoBehaviour
{
    public static MatchSaveManager Instance;
    public void Awake() => Instance = this;

    [SerializeField] OutMatchData externalMatchDataInfo;
    [SerializeField] private Transform scouterContent;

    public void SaveMatch(bool resetValues = false)
    {
        MatchData matchData = new();
        externalMatchDataInfo.UpdateMatchSaveManagerValues(matchData);
        for (int section = 1; section < scouterContent.childCount; section++)
        {
            for (int scoutingData = 0; scoutingData < scouterContent.GetChild(section).childCount; scoutingData++)
            {
                ScoutingObject scoutingObject = scouterContent.GetChild(section).GetChild(scoutingData).GetComponent<ScoutingObject>();
                matchData.uniqueData.Add(scoutingObject.GetMatchData());
                if (resetValues) scoutingObject.ResetValues();
            }
        }
        SaveManager.SaveMatchData(matchData);
    }
    public void UpdateOutMatch() => externalMatchDataInfo.AutoUpdate();
}
