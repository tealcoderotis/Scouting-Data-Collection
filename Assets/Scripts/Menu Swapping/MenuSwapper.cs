using UnityEngine;

public class MenuSwapper : MonoBehaviour
{
    public void ChangeMenu(int index)
    {
        if (index < 0 || index >= transform.childCount) Debug.LogWarning($"Called ChangeMenu on {name} with out of bound index: {index}");
        else ChangeMenu(transform.GetChild(index));
    }
    public void ChangeMenu(string menuName)
    {
        Transform targetMenu = transform.Find(menuName);
        if (targetMenu == null) Debug.LogWarning($"Called ChangeMenu on {name} with invalid name: {menuName}");
        else ChangeMenu(targetMenu);
    }
    private void ChangeMenu(Transform transform)
    {
        for (int i = 0; i < base.transform.childCount; i++)
            base.transform.GetChild(i).gameObject.SetActive(base.transform.GetChild(i) == transform);
    }
}
