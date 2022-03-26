public enum SoldierType
{
    NONE = -1,
    Infantry = 0,       //步兵
    Cavalry = 1,        //骑兵
    Bowman = 2,         //弓兵
    Enchanter = 3,      //魔法师
}
public enum SoldierAnimState
{
    Idle = 0,
    Move = 1,
    Attack = 2,
    Die = 3,
}
public class MessageID
{
    public const int Heat = 100;//心跳
    public const int Login = 10001;//

}

public class StaticDef
{
    public static int UID;

}