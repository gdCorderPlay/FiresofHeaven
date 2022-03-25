using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Debugger

{
    public static void LogError(object msg)
    {
        Debug.LogError(msg);
    }
    public static void Log(object msg)
    {
        Debug.Log(msg);
    }
    public static void Log_Btree(object msg)
    {
        //Debug.Log(msg);
    }
    public static void LogWarning(object msg)
    {
        Debug.LogWarning( msg);
    }
    

}


