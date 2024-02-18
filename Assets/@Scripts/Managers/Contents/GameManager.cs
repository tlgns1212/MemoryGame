using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using static Define;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System.Net;
using UnityEngine.Networking;



[Serializable]
public class GameData
{
    public int level = 1;
    public int totalScore = 0;
    public int combo = 0;
    public float time = 0;
    public float startTime = 0;
    public List<HistoryData> historyDatas = new List<HistoryData>();
}
[Serializable]
public class HistoryData
{
    public float time;
    public string description;
}
public class GameManager
{
    #region GameData
    public GameData _gameData = new GameData();
    public int Level
    {
        get { return _gameData.level; }
        set { _gameData.level = value; }
    }
    public int TotalScore
    {
        get { return _gameData.totalScore; }
        set
        {
            _gameData.totalScore = value;
            OnResourcesChanged?.Invoke();
        }
    }
    public int Combo
    {
        get { return _gameData.combo; }
        set { _gameData.combo = value; }
    }
    public float Time
    {
        get { return _gameData.time; }
        set { _gameData.time = value; }
    }
    public float StartTime
    {
        get { return _gameData.startTime; }
        set { _gameData.startTime = value; }
    }
    public List<HistoryData> HistoryDatas
    {
        get { return _gameData.historyDatas; }
    }
    private UI_GameScene _gameSceneUI;
    public UI_GameScene GameSceneUI
    {
        get { return _gameSceneUI; }
        set { _gameSceneUI = value; }
    }

    // public int Ruby
    // {
    //     get { return _gameData.Ruby; }
    //     set
    //     {
    //         _gameData.Ruby = value;
    //         // TODO 재화 습득시 저장이 아니라 일정 시간마다 저장으로 하기
    //         SaveGame();
    //         OnResourcesChanged?.Invoke();
    //     }
    // }
    #region Action
    public event Action OnResourcesChanged;
    #endregion

    #region Option
    public bool _effectSoundOn = true;
    public bool EffectSoundOn
    {
        get { return _effectSoundOn; }
        set { _effectSoundOn = value; }
    }

    public bool BGMOn
    {
        get { return BGMOn; }
        set
        {
            if (BGMOn == value)
                return;
            BGMOn = value;
            if (BGMOn == false)
            {
                Managers.Sound.Stop(Define.Sound.Bgm);
            }
            else
            {
                string name = "Bgm_Lobby";
                if (Managers.Scene.CurrentScene.SceneType == Define.Scene.GameScene)
                    name = "Bgm_Game";

                Managers.Sound.Play(Define.Sound.Bgm, name);
            }
        }
    }

    #endregion
    #endregion

    // public void RefreshGame()
    // {
    //     _path = Application.persistentDataPath + "/" + UserName + ".json";
    //     _gameData = new GameData();
    //     SaveGame();
    // }

    // public void Init()
    // {
    //     _path = Application.persistentDataPath + "/" + UserName + ".json";

    //     if (File.Exists(_path))
    //     {
    //         string fileStr = File.ReadAllText(_path);
    //         GameData data = JsonConvert.DeserializeObject<GameData>(fileStr);
    //         if (data != null)
    //             _gameData = data;
    //     }

    //     SaveGame();
    // }

    // #region Save&Load
    // public string _path = "";
    // public void SaveGame()
    // {
    //     _path = Application.persistentDataPath + "/" + UserName + ".json";
    //     string jsonStr = JsonConvert.SerializeObject(_gameData);
    //     File.WriteAllText(_path, jsonStr);
    //     Debug.Log($"Save Game Completed : {_path}");
    // }

    // public bool LoadGame()
    // {
    //     _path = Application.persistentDataPath + "/" + UserName + ".json";

    //     if (File.Exists(_path) == false)
    //     {
    //         Init();
    //         return false;
    //     }

    //     string fileStr = File.ReadAllText(_path);
    //     GameData data = JsonConvert.DeserializeObject<GameData>(fileStr);
    //     if (data != null)
    //         _gameData = data;

    //     Debug.Log($"Save Game Loaded : {_path}");
    //     return true;
    // }

    // public void ClearContinueData()
    // {
    //     // TODO Clear 후 저장
    // }

    // #endregion

}
