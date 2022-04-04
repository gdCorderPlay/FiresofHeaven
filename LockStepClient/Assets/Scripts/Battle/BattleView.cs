using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Battle.Logic
{
    public class BattleView
    {
        public Dictionary<uint, GameObject> allSoldiers;
        public BattleView()
        {
            allSoldiers = new Dictionary<uint, GameObject>();
        }

        public void CreateSoldier(SoldierData data)
        {
            GameObject soldier = CreateSoldier(data.type);
            allSoldiers.Add(data.key,soldier);
            soldier.transform.position = new Vector3(data.x.AsFloat(), 0.45f, data.y.AsFloat());
            
        }

        public void UpdateSoldierInfo(List<SoldierData> soldiers)
        {
            for (int i = 0; i < soldiers.Count; i++)
            {
                UpdateSoldierInfo(soldiers[i]);
            }
        }
        void UpdateSoldierInfo(SoldierData soldier)
        {
            allSoldiers[soldier.key].transform.position= new Vector3(soldier.x.AsFloat(), 0.45f, soldier.y.AsFloat());
            allSoldiers[soldier.key].transform.rotation = Quaternion.LookRotation(new Vector3(-soldier.dirX.AsFloat(), 0, -soldier.dirY.AsFloat()), Vector3.up);
            allSoldiers[soldier.key].SetActive(soldier.ghostTime>0);
            allSoldiers[soldier.key].transform.GetComponent<Animator>().SetInteger("state", soldier.state);
        }
        private GameObject CreateSoldier(SoldierType soldierType)
        {
            switch (soldierType)
            {
                case SoldierType.Infantry:
                    return GameObject.Instantiate(Resources.Load("Prefabs/actor/ifrit")) as GameObject;
                case SoldierType.Cavalry:
                    return GameObject.Instantiate(Resources.Load("Prefabs/actor/leopard")) as GameObject;
                case SoldierType.Bowman:
                    return GameObject.Instantiate(Resources.Load("Prefabs/actor/ba")) as GameObject;
                case SoldierType.Enchanter:
                    return GameObject.Instantiate(Resources.Load("Prefabs/actor/scorpion")) as GameObject;

            }
            return new GameObject();
        }
    }
}

