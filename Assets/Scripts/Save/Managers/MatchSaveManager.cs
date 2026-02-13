using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MatchSaveManager : MonoBehaviour
{
    public static MatchSaveManager Instance;
    public void Awake() => Instance = this;

    [SerializeField] PrematchData externalMatchDataInfo;
    [SerializeField] private Transform scouterContent;
    [Header("Saved info feedback")]
    [SerializeField] ScrollRect content;
    [SerializeField] TextMeshProUGUI saveVisualizer;

    public void SaveMatch(bool resetValues = false)
    {
        MatchData matchData = new();
        externalMatchDataInfo.UpdateMatchSaveManagerValues(matchData);
        for (int section = 1; section < scouterContent.childCount; section++)
        {
            for (int scoutingData = 0; scoutingData < scouterContent.GetChild(section).childCount; scoutingData++)
            {
                ScoutingObject scoutingObject = scouterContent.GetChild(section).GetChild(scoutingData).GetComponent<ScoutingObject>();
                MatchData.ArbritraryData arbitraryMatchData = scoutingObject.GetMatchData();
                if (!scoutingObject.Nullified) {
                    matchData.uniqueData.Add(arbitraryMatchData);
                }
                else
                {
                    matchData.uniqueData.Add(new MatchData.ArbritraryData(arbitraryMatchData.name, arbitraryMatchData.type, ""));
                }
                if (resetValues) scoutingObject.ResetValues();
            }
        }
        SaveManager.SaveMatchData(matchData);
        StartCoroutine(FeedbackManager.Instance.DoFeedback($"Saving data to {SaveManager.EventSaveString(matchData.EventKey)}"));
    }
}
