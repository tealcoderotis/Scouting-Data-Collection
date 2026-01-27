using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_InputField))]
public class SiblingInput : MonoBehaviour
{
    TMP_InputField inputField;
    int ChildCount => Scouter.Instance.content.transform.GetChild(Scouter.Instance.ObjectCreation.SubsetValue - 1).childCount;
    private void Awake()
    {
        inputField = GetComponent<TMP_InputField>();
        inputField.onEndEdit.AddListener((str) =>
        {
            if (str == "")
            {
                DefaultText();
                return;
            }
            inputField.SetTextWithoutNotify(System.Math.Clamp(int.Parse(str), 0, ChildCount).ToString());
        });
        Scouter.Instance.ObjectCreation.OnChangeSubset.AddListener(_ => DefaultText());
    }
    private void OnEnable()
    {
        Scouter.Instance.ObjectCreation.SubsetValue = 0;
        DefaultText();
    }
    private void DefaultText()
    {
        inputField.SetTextWithoutNotify(Scouter.Instance.ObjectCreation.SubsetValue == 0 ? "0" : ChildCount.ToString());
    }
}
