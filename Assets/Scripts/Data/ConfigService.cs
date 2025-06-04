using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Zenject;

public class ConfigService
{
    public GameConfig gameConfig { get; private set; }
    private const string GAMECONFIG_JSON_NAME = "gameconfig";

    public ConfigService()
    {
        LoadConfigFromResources();
    }

    private void LoadConfigFromResources()
    {
        TextAsset textAsset = Resources.Load<TextAsset>(GAMECONFIG_JSON_NAME);
        if (textAsset != null)
        {
            gameConfig = JsonUtility.FromJson<GameConfig>(textAsset.text);
        }
        else
        {
            Debug.LogError($"Config file not found in Resources at path: {GAMECONFIG_JSON_NAME}");
        }
    }
}