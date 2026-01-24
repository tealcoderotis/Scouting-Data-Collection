using TMPro;

public class MergedInputField : MergedUI<TMP_InputField, TMP_InputField.SubmitEvent, string>
{
    protected override string Value(TMP_InputField source) => thisGraphic.text;
    protected override TMP_InputField.SubmitEvent GetEvent(TMP_InputField source) => source.onSubmit;
    protected override void SetWithoutNotify(TMP_InputField source, string value) => source.SetTextWithoutNotify(value);
}
