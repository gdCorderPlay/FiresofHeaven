namespace Battle.Logic
{
    [System.Serializable]
    public class SoldierData
    {
        public uint key;
        //坐标(表现层数据，不用是整型)
        public float x;
        public float y;
        public float dirX;
        public float dirY;
        //是否是进攻方
        public bool isAtkTroop;
        //部队状态
        public int state;
        //兵种类型
        public SoldierType type;
        //目标id
        public uint targetKey = 0;
        //血量
        public int hp;

        public uint norAtkCD;
        //攻击中
        public bool isAtk;
        //前置动作
        public bool inPrepose;
        //前置动作时间
        public uint preTime;
        //死亡停留时间
        public uint ghostTime;
        //是否处于战斗状态
        public bool inWar;
        //攻击范围
        public float atkRange;
        //单位体积
        public float spyRange;
    }
    [System.Serializable]
    public class SoldierConfigs
    {
        public SoldierConfig[] configs;
    }
    [System.Serializable]
    public class SoldierConfig
    {
        //兵种类型
        public SoldierType type;
        //血量
        public int hp;

        public uint norAtkCD;
        //前置动作时间
        public uint preTime;
        //死亡停留时间
        public uint ghostTime;
        //攻击范围
        public uint atkRange;
        //单位体积
        public uint range;
        //行为树
        public string BTree;
    }
}

