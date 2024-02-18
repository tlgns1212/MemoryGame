using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameScene : UI_Scene
{
    #region Enum
    enum GameObjects
    {
        StageGroup,
        StartGroup,
        EndGroup,
        UI_GameSpaceItem,
    }

    enum Buttons
    {
        StartButton,
        BackButton,
        RestartButton,
        ReplayButton,
    }

    enum Texts
    {
        ScoreText,
        TimerText,
        CurrentScoreValueText,
        BestScoreValueText,
        StartBestScoreValueText,
        HistoryContent,
    }

    enum Images
    {
    }
    #endregion

    UI_GameSpaceItem gameSpaceItem;
    private int _bestScore;
    public int BestScore
    {
        get { return _bestScore; }
        set
        {
            _bestScore = value;
            GetText((int)Texts.BestScoreValueText).text = _bestScore.ToString();
            GetText((int)Texts.StartBestScoreValueText).text = _bestScore.ToString();
            PlayerPrefs.SetInt("BestScore", _bestScore);
        }
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Managers.Game.GameSceneUI = this;
        Managers.Game.OnResourcesChanged += Refresh;

        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        BindImage(typeof(Images));

        GetButton((int)Buttons.StartButton).gameObject.BindEvent(OnClickStartButton);
        GetButton((int)Buttons.BackButton).gameObject.BindEvent(OnClickBackButton);
        GetButton((int)Buttons.ReplayButton).gameObject.BindEvent(OnClickReplayButton);
        GetButton((int)Buttons.RestartButton).gameObject.BindEvent(OnClickRestartButton);


        if (PlayerPrefs.HasKey("BestScore"))
        {
            BestScore = PlayerPrefs.GetInt("BestScore");
        }
        else
        {
            BestScore = 0;
        }


        GetObject((int)GameObjects.StageGroup).SetActive(false);
        GetObject((int)GameObjects.EndGroup).SetActive(false);

        gameSpaceItem = GetObject((int)GameObjects.UI_GameSpaceItem).GetOrAddComponent<UI_GameSpaceItem>();

        Refresh();

        return true;
    }

    private void Awake()
    {
        Init();
    }

    private void Refresh()
    {
        GetText((int)Texts.ScoreText).text = Managers.Game.TotalScore.ToString();
    }

    void OnClickStartButton()
    {
        GetObject((int)GameObjects.StartGroup).SetActive(false);
        GetObject((int)GameObjects.StageGroup).SetActive(true);

        Managers.Game.HistoryDatas.Clear();
        Managers.Game.StartTime = Time.realtimeSinceStartup;
        Managers.Game.TotalScore = 0;
        StartGame();
    }
    void OnClickBackButton()
    {
        GetObject((int)GameObjects.StageGroup).SetActive(false);
        GetObject((int)GameObjects.EndGroup).SetActive(true);
        EndGame();
    }
    void OnClickReplayButton()
    {
        // GetObject((int)GameObjects.EndGroup).SetActive(false);
        // GetObject((int)GameObjects.StageGroup).SetActive(true);

    }
    void OnClickRestartButton()
    {
        GetObject((int)GameObjects.EndGroup).SetActive(false);
        GetObject((int)GameObjects.StageGroup).SetActive(true);

        Managers.Game.HistoryDatas.Clear();
        Managers.Game.StartTime = Time.realtimeSinceStartup;
        Managers.Game.TotalScore = 0;
        StartGame();
    }

    IEnumerator TimeTicking()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            Managers.Game.Time--;
            TimeSpan timeSpan = TimeSpan.FromSeconds(Managers.Game.Time);
            string formattedTime = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
            GetText((int)Texts.TimerText).text = formattedTime;
            if (Managers.Game.Time <= 0)
            {
                EndGame();
                break;
            }
        }
    }

    Coroutine _timerCo;

    public void StartGame(int level = 1)
    {
        gameSpaceItem.SetInfo(level);
        // Managers.Game.TotalScore = 0;
        Managers.Game.Combo = 0;
        TimeSpan timeSpan = TimeSpan.FromSeconds(Managers.Game.Time);
        string formattedTime = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
        GetText((int)Texts.TimerText).text = formattedTime;
        if (_timerCo != null)
            StopCoroutine(_timerCo);
        _timerCo = StartCoroutine(TimeTicking());
    }

    public void EndGame()
    {
        if (BestScore < Managers.Game.TotalScore)
        {
            BestScore = Managers.Game.TotalScore;
        }
        GetText((int)Texts.CurrentScoreValueText).text = Managers.Game.TotalScore.ToString();

        GetObject((int)GameObjects.EndGroup).SetActive(true);
        InitEndGroup();
        GetObject((int)GameObjects.StageGroup).SetActive(false);
    }

    void InitEndGroup()
    {
        string historyString = "";

        foreach (HistoryData hData in Managers.Game.HistoryDatas)
        {
            string lineString = "";
            print(hData.time);
            TimeSpan timeSpan = TimeSpan.FromSeconds(hData.time);
            lineString += string.Format("{0:D2}분 {1:D2}초에 ", timeSpan.Minutes, timeSpan.Seconds);
            lineString += hData.description + "\n";
            historyString += lineString;
        }
        GetText((int)Texts.HistoryContent).text = historyString;
    }

    // private void OnDestroy()
    // {
    //     if (Managers.Game != null)
    //         Managers.Game.OnResourcesChanged -= Refresh;
    // }
}
