using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateStatDataClass
{
    [MenuItem("Assets/FSM/StatData")]
    public static StatData CreateStatData()
    {
        StatData asset = ScriptableObject.CreateInstance<StatData>();
        AssetDatabase.CreateAsset(asset, "Assets/Data/StatData.asset");
        AssetDatabase.SaveAssets();
        return asset;
    }

    [MenuItem("Assets/FSM/PlayerStatData")]
    public static StatData CreatePlayerStatData()
    {
        StatData asset = ScriptableObject.CreateInstance<PlayerStatData>();
        AssetDatabase.CreateAsset(asset, "Assets/Data/PlayerStatData.asset");
        AssetDatabase.SaveAssets();
        return asset;
    }

    [MenuItem("Assets/FSM/AIStatData")]
    public static StatData CreateAIStatData()
    {
        StatData asset = ScriptableObject.CreateInstance<AIStatData>();
        AssetDatabase.CreateAsset(asset, "Assets/Data/AIStatData.asset");
        AssetDatabase.SaveAssets();
        return asset;
    }
}