using Battle.Logic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIMain : MonoBehaviour
{
    public Toggle isAtk;
    public Text tips;
    public GameObject headinfoPrefab;
    public System.Action<bool ,SoldierType> onSoliderSelect;

    private List<HeadInfo> recyleHeadinfo = new List<HeadInfo>();
    private Queue<HeadInfo> pools = new Queue<HeadInfo>();
    private Dictionary<uint,HeadInfo> headinfos = new Dictionary<uint, HeadInfo>();
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

    public void ShowFrameCount(int count)
    {
        tips.text = "Frame:" + count;
    }

    public void UpdateHeadInfos(List<SoldierData> datas,BattleView view)
    {
        for (int i = 0; i < datas.Count; i++)
        {
            uint key = datas[i].key;
            if (!headinfos.ContainsKey(key))
            {
                GameObject head = GameObject.Instantiate<GameObject>(headinfoPrefab,headinfoPrefab.transform.parent);
                HeadInfo info = new HeadInfo(head.transform);
                info.Init(datas[i]);
                headinfos.Add(key, info);
            }
            HeadInfo temp = headinfos[key];
            GameObject target = view.allSoldiers[key];
            if (target.activeInHierarchy)
            {
               Vector3 screenPos= Camera.main.WorldToScreenPoint(target.transform.position+Vector3.up*2);
                temp.UpdatePos(screenPos, datas[i].hp);
            }
            else
            {
                temp.Receyle();
            }

        }
    }
}
