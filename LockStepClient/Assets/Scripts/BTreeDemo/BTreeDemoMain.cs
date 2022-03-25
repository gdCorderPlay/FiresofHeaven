using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Battle.Logic;
public class BTreeDemoMain : MonoBehaviour
{
    public UIMain UI;
    private BattleLogic battle;
    public List<SoldierData> soldiersAtk;
    public List<SoldierData> soldiersDef;
    void Start()
    {
        
        Application.targetFrameRate = 30;
        TroopHelper.Init();
        UI = GameObject.FindObjectOfType<UIMain>();
        UI.onSoliderSelect = OnSoilderCreateClick;
        battle = new BattleLogic();
        soldiersAtk = battle.battleData.mAtcSoldierList;
        soldiersDef = battle.battleData.mDefSoldierList;
    }

    // Update is called once per frame
    void Update()
    {
        battle.OnRenderUpdate(Time.deltaTime);
    }
    int frame;
    private void FixedUpdate()
    {
        battle.OnStepUpdate(frame++);
    }
    void OnSoilderCreateClick(bool isAtk,SoldierType soldierType)
    {
        if (HitGround(out Vector3 hit))
        {
            battle.CreateSoldier(soldierType, hit.x, hit.z, isAtk);
        }

    }
    private bool HitGround( out Vector3 pos)
    {
        pos = Vector3.zero;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit,Camera.main.farClipPlane, LayerMask.GetMask("Ground")))
        {
            pos = hit.point;
            return true;
        }
        return false;
    }
}
