using Battle.Logic.AI.BTree;
using BTreeFrame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Battle.Logic
{
    public class BattleLogic
    {
        public BattleData battleData;
        public BattleView battleView;

        Dictionary<uint, BTreeRoot> allTrees;
        public BattleLogic()
        {
            battleData = new BattleData();
            battleView = new BattleView();
            allTrees = new Dictionary<uint, BTreeRoot>();
        }

        /// <summary>
        /// 渲染帧更新
        /// </summary>
        /// <param name="deltaTime"></param>
        public void OnRenderUpdate(float deltaTime)
        {
            battleView.UpdateSoldierInfo(battleData.mAtcSoldierList);
            battleView.UpdateSoldierInfo(battleData.mDefSoldierList);
        }
        /// <summary>
        /// 逻辑帧更新
        /// </summary>
        /// <param name="frameIndex"></param>
        public void OnStepUpdate(int frameIndex)
        {

            DoSoldierLogic();
            CalBattleResults();
        }

        BTreeOutputData m_Output = new BTreeOutputData();
        BTreeInputData m_Input = new BTreeInputData();
        private void DoSoldierLogic()
        {
            for (int i = 0; i < battleData.mAtcSoldierList.Count; i++)
            {
                var _atkTroop = battleData.mAtcSoldierList[i];
                m_Input.SetData(_atkTroop, battleData);
                m_Output.SetData(m_Input);
               
                BTreeTemplateData _output = m_Output as BTreeTemplateData;
                allTrees[_atkTroop.key].Tick(m_Input, ref _output);
                m_Output = _output as BTreeOutputData;
                battleData.mAtcSoldierList[i] = m_Output.soldier;

            }
            for (int i = 0; i < battleData.mDefSoldierList.Count; i++)
            {
                var _defTroop = battleData.mDefSoldierList[i];
                m_Input.SetData(_defTroop, battleData);
                m_Output.SetData(m_Input);
               
                BTreeTemplateData _output = m_Output as BTreeTemplateData;
                allTrees[_defTroop.key].Tick(m_Input, ref _output);
                m_Output = _output as BTreeOutputData;
                battleData.mDefSoldierList[i] = m_Output.soldier;
            }
        }

        /// <summary>
        /// 结算战斗结果
        /// </summary>
        private void CalBattleResults()
        {
            for (int i = 0; i < battleData.mAtcSoldierList.Count; i++)
            {
                SoldierData soldier = battleData.mAtcSoldierList[i];
                if (soldier.isAtk)
                {
                    if (battleData.mAllSoldierDic.ContainsKey(soldier.targetKey))
                    {
                        battleData.mAllSoldierDic[soldier.targetKey].hp -= 10;

                    }
                    soldier.isAtk = false;
                }
                
            }
            for (int i = 0; i < battleData.mDefSoldierList.Count; i++)
            {
                SoldierData soldier = battleData.mDefSoldierList[i];
                if (soldier.isAtk)
                {
                    if (battleData.mAllSoldierDic.ContainsKey(soldier.targetKey))
                    {
                        battleData.mAllSoldierDic[soldier.targetKey].hp -= 10;
                    }   
                    soldier.isAtk = false;
                }
            }
        }
        public void CreateSoldier(SoldierType soldierType, float x, float y,bool isAtk)
        {
            SoldierData soldier = new SoldierData();
            soldier.key = UIDHelper.GetUID();
            soldier.type = soldierType;
            soldier.x = x;
            soldier.y = y;
            soldier.isAtkTroop = isAtk;
            soldier.hp = TroopHelper.GetMaxHp(soldierType);
            soldier.ghostTime = 60;
            soldier.dirX = 1;
            battleData.AddSoldier(soldier);
            battleView.CreateSoldier(soldier);
            BTreeRoot bTree = new BTreeRoot(TroopHelper.GetBTree(soldierType));
            allTrees.Add(soldier.key, bTree);
            
        }


    }
}

