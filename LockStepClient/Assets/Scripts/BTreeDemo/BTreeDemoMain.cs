using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Battle.Logic;
using Proto.Data;
using System;
using Google.Protobuf.Collections;

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
        MessageMgr.Instance.AddListener<FrameData>("LockStepLogic", StepLogic);
        Main.Instance.IsReadyForBattle();
    }

    private void StepLogic(FrameData obj)
    {
        CheckRemoteInput(obj.Commands);
        battle.OnStepUpdate(frame++);
    }
    private void CheckRemoteInput(RepeatedField<Command> commands)
    {
        for (int i = 0; i < commands.Count; i++)
        {
            battle.CreateSoldier((SoldierType)commands[i].SoldierType,commands[i].X*0.001f,commands[i].Y*0.001f,commands[i].Mode==0);
        }
    }
    // Update is called once per frame
    void Update()
    {
        battle.OnRenderUpdate(Time.deltaTime);
    }
    int frame;
    private void FixedUpdate()
    {
        //battle.OnStepUpdate(frame++);
    }
    void OnSoilderCreateClick(bool isAtk,SoldierType soldierType)
    {
        if (HitGround(out Vector3 hit))
        {
            Main.Instance.Create(soldierType,hit.x,hit.z);
            //battle.CreateSoldier(soldierType, hit.x, hit.z, isAtk);
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
