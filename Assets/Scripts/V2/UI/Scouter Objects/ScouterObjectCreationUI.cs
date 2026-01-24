using TMPro;
using UnityEngine;

public abstract class ScouterObjectCreationUI : MonoBehaviour
{
    [SerializeField] protected TMP_Dropdown sectionSelect;
    [SerializeField] protected TMP_InputField sectionIndex;
    [SerializeField] protected TMP_InputField objectName;
    [SerializeField] protected TMP_InputField objectID;
    [SerializeField] protected TMP_Dropdown typeSelect;


    public void ResetBaseUI()
    {
        sectionSelect.value = 0;
        objectName.text = string.Empty;
        objectID.text = string.Empty;
        typeSelect.value = 0;
    }
    public abstract void ResetSpecificUI();
    public abstract void LoadValues(object obj);
    public abstract void ApplyValues(object obj);
    public abstract ScoutingObject CreateInstance();
}
public abstract class ScouterObjectCreationUI<T> : ScouterObjectCreationUI where T : ScoutingObject
{
    [Space]
    [SerializeField] protected T prefab;

    public override void LoadValues(object obj)
    {
        if (obj.GetType() != typeof(T)) return;
        LoadValues((T)obj);
    }
    public override void ApplyValues(object obj)
    {
        if (obj.GetType() != typeof(T)) return;
        ApplyValues((T)obj);
    }
    public virtual void LoadValues(T obj)
    {
        for (int i = 0; i < sectionSelect.options.Count; ++i)
        {
            if (sectionSelect.options[i].text == obj.GetBaseSettings().sectionName)
            {
                sectionSelect.value = i;
                break;
            }
        }
        sectionIndex.text = obj.transform.GetSiblingIndex().ToString();
        objectName.text = obj.GetBaseSettings().objectName;
        objectID.text = obj.GetBaseSettings().objectID == ScouterObjectCreator.Instance.GetID(objectName.text, true) ? string.Empty : obj.GetBaseSettings().objectID;
        for (int i = 0; i < ScouterObjectCreator.TypeIndexes.Length; ++i)
        {
            if (obj.GetType() == ScouterObjectCreator.TypeIndexes[i])
            {
                typeSelect.value = i;
                break;
            }
        }
    }
    public TSettings GetAsSettings<TSettings>() where TSettings : ScoutingObject.ScoutingObjectSettings, new()
    {
        TSettings setting = new()
        {
            objectName = objectName.text,
            sectionName = sectionSelect.options[sectionSelect.value].text,
            indexInSection = sectionIndex.text.Length == 0 ? -1 : int.Parse(sectionIndex.text)
        };
        setting.objectID = (objectID.text.Length == 0 ? ScouterObjectCreator.Instance.GetID(setting.objectName, true) : objectID.text);
        return setting;
    }
    public abstract void ApplyValues(T obj);

    public override ScoutingObject CreateInstance()
    {
        T instance = Instantiate(prefab);
        ApplyValues(instance);
        return instance;
    }
}