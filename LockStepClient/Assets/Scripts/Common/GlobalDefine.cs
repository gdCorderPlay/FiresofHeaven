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
    public const int Login = 10001;//登录
    public const int Match = 10002;//匹配对手
    public const int MatchResond = 10003;//匹配结果


}

public class StaticDef
{
    public static int UID;

    public static int playerMode;//分配的类型 0：红方 1：蓝方

}