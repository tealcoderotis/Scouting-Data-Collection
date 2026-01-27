using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "TextMeshPro/Input Validator")]
public class GeneralInputValidator : TMPro.TMP_InputValidator
{
    [Tooltip("if true: targetCharacters is whats allowed; if false: targetCharacters is whats excluded")]
    [SerializeField] private bool includedCharacters;
    [SerializeField] private char[] targetCharacters;

    public override char Validate(ref string text, ref int pos, char ch)
    {
        // if returns did work, what would I even do here? I WANT to return nothing. Just don't change...
        if (targetCharacters.Contains(ch) != includedCharacters) return char.MinValue;
        Debug.Log($"\"{text}\" should be edited at index {pos} w/ '{ch}'");
        text = text.Insert(pos, ch.ToString());
        pos += 1;

        // THIS DOESN'T WORK???
        return ch;
    }
}
