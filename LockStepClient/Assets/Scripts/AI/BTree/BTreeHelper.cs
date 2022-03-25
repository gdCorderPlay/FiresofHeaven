using Battle.Logic.AI.BTree;
using BTreeFrame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeHelper
{
    public static Dictionary<string, TreeConfig> configs;

    public static TreeConfig CreateTreeConfig(string treeName)
    {
        if (configs == null)
        {
            configs = new Dictionary<string, TreeConfig>();
            BTreeNodeFactory.Init();
             
        }
        if (!configs.ContainsKey(treeName))
        {
            TextAsset config = Resources.Load<TextAsset>("Config/"+ treeName);
            TreeConfig _config = JsonUtility.FromJson<TreeConfig>(config.text);
            configs.Add(treeName, _config);
        }
        return configs[treeName];
    }

}
