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
        if (targetCharacters.Contains(ch) != includedCharacters) return char.MinValue;
        text = text.Insert(pos, ch.ToString());
        ++pos;
        return ch;
    }
}
