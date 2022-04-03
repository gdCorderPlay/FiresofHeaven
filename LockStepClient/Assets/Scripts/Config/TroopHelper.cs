//////////////////////////////////////////////////////////////////////////////////////
// The MIT License(MIT)
// Copyright(c) 2018 lycoder

// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
//CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

//////////////////////////////////////////////////////////////////////////////////////
using System.Collections.Generic;
using UnityEngine;

namespace Battle.Logic
{
    public static class TroopHelper
    {
        private static Dictionary<SoldierType, SoldierConfig> configs;  
        public static void Init()
        {
            configs = new Dictionary<SoldierType, SoldierConfig>();
            TextAsset config = Resources.Load<TextAsset>("Config/SoldierConfig");
            SoldierConfigs _config = JsonUtility.FromJson<SoldierConfigs>(config.text);
            for (int i = 0; i < _config.configs.Length; i++)
            {
                configs.Add(_config.configs[i].type, _config.configs[i]);
            }
        }
        public static uint scale = 1;
        public static uint GetTroopAtkDis(SoldierType type)
        {
            uint dis = 1;
            if (configs.ContainsKey(type))
            {
                dis = configs[type].atkRange;
            }
            dis = dis * scale;
            dis = dis * dis;
            return dis;
        }
        public static uint GetTroopRange(SoldierType type)
        {
            uint dis = 2;
            if (configs.ContainsKey(type))
            {
                dis = configs[type].range;
            }
            dis = dis * scale;
            dis = dis * dis;
            return dis;
        }
        public static uint GetTroopAtkPrepareTime(SoldierType type)
        {
            uint prepareTime = 2;
            if (configs.ContainsKey(type))
            {
                prepareTime = configs[type].preTime;
            }
            return prepareTime;
        }
        public static uint GetTroopAtkCDTime(SoldierType type)
        {
            uint cd = 2;
            if (configs.ContainsKey(type))
            {
                cd = configs[type].norAtkCD;
            }
            return cd;
        }
        public static float GetSpeed(SoldierType type)
        {
            return 0.5f;
        }

        public static string GetBTree(SoldierType type)
        {
            string tree = "";
            if (configs.ContainsKey(type))
            {
                tree = configs[type].BTree;
            }
            return tree;
        }
        public static int GetMaxHp(SoldierType type)
        {
            int hp = 0;
            if (configs.ContainsKey(type))
            {
                hp = configs[type].hp;
            }
            return hp;
        }
    }
}

