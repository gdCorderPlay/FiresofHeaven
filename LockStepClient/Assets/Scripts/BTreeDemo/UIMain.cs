using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIMain : MonoBehaviour
{
    public Toggle isAtk;

    public System.Action<bool ,SoldierType> onSoliderSelect;

    private void Start()
    {
        SoilderIcon[] icons = transform.GetComponentsInChildren<SoilderIcon>();
        for (int i = 0; i < icons.Length; i++)
        {
            icons[i].action = OnSoliderSelect;
        }
    }

    void OnSoliderSelect(SoldierType  soldierType)
    {
        if(onSoliderSelect!=null)
        onSoliderSelect(isAtk.isOn,soldierType);
    }
}
