using System.Collections;
using TMPro;
using UnityEngine;

public class FeedbackManager : MonoBehaviour
{
    public static FeedbackManager Instance;
    private void Awake() => Instance = this;

    [SerializeField] private GameObject inputPrevention;
    [SerializeField] private TextMeshProUGUI feedbackDisplay;
    [SerializeField] private float defaultDuration;

    public bool FeedbackEnabled { get => inputPrevention.activeSelf; set => inputPrevention.SetActive(value); }
    public string Feedback { get => feedbackDisplay.text; set => feedbackDisplay.text = value; }
    public float DefaultFeedbackDuration => defaultDuration;
    public IEnumerator DoFeedback(string feedback)
    {
        yield return DoFeedback(feedback, new WaitForSeconds(defaultDuration));
    }
    public IEnumerator DoFeedback(string feedback, object delay = null, System.Action onCloseFeedback = null)
    {
        feedbackDisplay.text = feedback;
        inputPrevention.SetActive(true);

        yield return delay;

        onCloseFeedback?.Invoke();
        inputPrevention.SetActive(false);
    }
}
