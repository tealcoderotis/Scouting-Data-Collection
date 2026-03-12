using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public static DialogManager Instance;
    [SerializeField] private GameObject DialogModal;
    [SerializeField] private GameObject AccuracyDialog;
    [SerializeField] private GameObject SelectDialog;
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        DialogModal.GetComponent<Button>().onClick.AddListener(() => HideDialog());
    }

    public void ShowAccuracyDialog()
    {
        AccuracyDialog.SetActive(true);
        DialogModal.SetActive(true);
    }

    public void ShowSelectDialog(MultiSelectObject scoutingObject)
    {
        SelectDialog.SetActive(true);
        DialogModal.SetActive(true);
    }


    public void HideDialog()
    {
        DialogModal.SetActive(false);
    }
}
