using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDHelper 
{
    public static uint count;
    public static uint GetUID()
    {
        count++;
        return count;
    }
}
