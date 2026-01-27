using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TempInputFieldThing : MonoBehaviour
{
    [SerializeField] private TMP_InputField source;
    [SerializeField] private Image targetHolder;
    [SerializeField] private TextMeshProUGUI targetLabel;
    [SerializeField] private TextMeshProUGUI target;
    [SerializeField] private float fadeSpeed;

    private void Start()
    {
        StartCoroutine(Fade());
    }
    private void Update()
    {
        target.text = source.text;
        fadeTarget = target.transform.position.y <= source.transform.position.y ? 1 : 0;
    }

    private float fadeTarget = 0;
    private IEnumerator Fade()
    {
        if (fadeTarget != 0) targetHolder.gameObject.SetActive(true);

        while (true)
        {
            float timedChange = Mathf.Sign(fadeTarget - GetFade()) * fadeSpeed * Time.deltaTime;
            float proximityChange = fadeTarget - GetFade();
            if (Mathf.Abs(proximityChange) < Mathf.Abs(timedChange))
            {
                SetFade(fadeTarget);
                yield return 1;
                continue;
            }
            SetFade(GetFade() + timedChange);
            yield return 1;
        }
        //SetFade(fadeTarget);
        //fade = null;
        //targetHolder.gameObject.SetActive(fadeTarget != 0);


        float GetFade() => targetHolder.color.a;
        void SetFade(float fade)
        {
            SetFadeSingle(targetHolder, fade);
            SetFadeSingle(targetLabel, fade);
            SetFadeSingle(target, fade);
        }
        void SetFadeSingle(Graphic graphic, float fade)
        {
            Color color = graphic.color;
            color.a = fade;
            graphic.color = color;
        }
    }
}