using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSingleton : Singleton<GameSingleton>
{
    protected GameSingleton()
    {
    }
    public int currentLevel;
    public LevelButtonMode currentMode = LevelButtonMode.LoadLevelMode;
}
