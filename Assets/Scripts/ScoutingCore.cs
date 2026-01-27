using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoutingCore : MonoBehaviour
{
    public static ScoutingCore Instance;
    private void Awake()
    {
        Instance = this;
        SaveManager.LoadEventData();
    }
    private void OnApplicationPause() => SaveManager.SaveEventData();
    [SerializeField] private TMP_InputField eventOverrideCode;
    public void UpdateCurrent()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            StartCoroutine(FeedbackManager.Instance.DoFeedback("Internet not available - Cannot do an API call"));
        }
        else
        {
            FeedbackManager.Instance.Feedback = "Waiting for API result...";
            FeedbackManager.Instance.FeedbackEnabled = true;
            StartCoroutine(CoroutineUtil.DelayAction(ErrorHandledDisableInput, StartCoroutine(APIData.SetToCurrentEvent(this, eventOverrideCode.text))));
        }

        void ErrorHandledDisableInput()
        {
            if (CurrentEvent == null || CurrentEventMatches == null)
            {
                FeedbackManager.Instance.Feedback = "Error occured during requests. Anything null has been set to a new instance";
                CurrentEvent ??= new();
                CurrentEventMatches ??= new APIData.SimpleMatch[0];
                StartCoroutine(CoroutineUtil.DelayAction(() => FeedbackManager.Instance.FeedbackEnabled = false, new WaitForSeconds(FeedbackManager.Instance.DefaultFeedbackDuration)));
            }
            else FeedbackManager.Instance.FeedbackEnabled = false;
        }
    }

    public enum ScoutingPosition { Blue1, Blue2, Blue3, Red1, Red2, Red3};
    // qualification, quarter-final, semi-final, final
    public enum CompLevels { qm, qf, sf, f }

    [Space]
    [SerializeField] private CoreEventData eventData = new();

    public static CoreEventData EventData { get => Instance.eventData; set => Instance.eventData = value; }
    public static bool IsTesting { get => EventData.isTesting; set => EventData.isTesting = value; }
    public static APIData.SimpleEvent CurrentEvent { get => EventData.currentEvent; set => EventData.currentEvent = value; }
    public static APIData.SimpleMatch[] CurrentEventMatches { get => EventData.currentEventMatches; set => EventData.currentEventMatches = value; }
    public static int CurrentGlobalMatchIndex { get => EventData.currentGlobalMatchIndex; set => EventData.currentGlobalMatchIndex = value; }
    public static APIData.SimpleMatch CurrentExpectedMatch => CurrentEventMatches[CurrentGlobalMatchIndex];


    [System.Serializable]
    public class CoreEventData
    {
        public bool isTesting;
        public APIData.SimpleEvent currentEvent = new();
        public APIData.SimpleMatch[] currentEventMatches = new APIData.SimpleMatch[] { };
        public int currentGlobalMatchIndex;
    }

    public static MatchSorter matchSorter = new();
    public class MatchSorter : Comparer<APIData.SimpleMatch>
    {
        public override int Compare(APIData.SimpleMatch x, APIData.SimpleMatch y)
        {
            if (x.CompLevel != y.CompLevel)
                return System.Math.Sign(x.CompLevel - y.CompLevel);
            else if (x.set_number != y.set_number)
                return System.Math.Sign(x.set_number - y.set_number);
            else if (x.match_number != y.match_number)
                return System.Math.Sign(x.match_number - y.match_number);
            else return 0;
        }
    }
}
