using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_GameSpaceItem : UI_Base
{
    #region Enum
    enum Texts
    {
        ComboText
    }
    #endregion

    List<UI_CardItem> cards = new List<UI_CardItem>();
    Data.StageData _sData;
    int _selectedIndex;
    int _selectedNum;
    int _successCount;
    int _currentLevel;
    int Combo
    {
        get { return Managers.Game.Combo; }
        set
        {
            Managers.Game.Combo = value;
            if (Managers.Game.Combo != 0)
                ComboToast();
        }
    }

    private void Awake()
    {
        Init();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindText(typeof(Texts));

        // Refresh();

        return true;
    }

    public void SetInfo(int stageLevel)
    {
        _currentLevel = stageLevel;
        Managers.Game.HistoryDatas.Add(new HistoryData()
        {
            time = Time.realtimeSinceStartup - Managers.Game.StartTime,
            description = string.Format("{0} 스테이지 시작", _currentLevel)
        });
        _sData = Managers.Data.StageDic[stageLevel];
        GetText((int)Texts.ComboText).alpha = 0;

        _selectedNum = 0;
        _selectedIndex = 0;
        _successCount = 0;
        Managers.Game.Time = _sData.totalTime;
        Managers.Game.Level = _sData.level;
        int totalSize = _sData.horSize * _sData.verSize;
        foreach (UI_CardItem card in cards)
        {
            Destroy(card.gameObject);
        }
        cards = new List<UI_CardItem>();
        List<int> cardNums = new List<int>();
        for (int cardNum = 0; cardNum < 52; cardNum++)
        {
            cardNums.Add(cardNum);
        }
        cardNums.Shuffle();

        for (int i = 0; i < totalSize / 2; i++)
        {
            UI_CardItem card1 = Managers.UI.MakeSubItem<UI_CardItem>(transform);
            UI_CardItem card2 = Managers.UI.MakeSubItem<UI_CardItem>(transform);
            int cardNum = cardNums[i] % 13 + 1;
            int cardType = cardNums[i] / 13 + 1;

            card1.SetInfo(i * 2, 1, 0, cardType, cardNum, HandleCardFlippedAction);
            card2.SetInfo(i * 2 + 1, 1, 0, cardType, cardNum, HandleCardFlippedAction);
            cards.Add(card1);
            cards.Add(card2);
        }

        gameObject.GetComponent<GridLayoutGroup>().constraintCount = _sData.verSize;

        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].transform.SetSiblingIndex(i);
        }

        Refresh();
    }

    void HandleCardFlippedAction(int index)
    {
        Managers.Game.HistoryDatas.Add(new HistoryData()
        {
            time = Time.realtimeSinceStartup - Managers.Game.StartTime,
            description = string.Format("{0}를 뒤집음", cards[index]._cardData.frontCard)
        });
        int totalSize = _sData.horSize * _sData.verSize;
        if (_selectedNum == 0)
        {
            _selectedIndex = index;
            _selectedNum = cards[index]._cardId;
        }
        else
        {
            bool isSuccess = _selectedNum == cards[index]._cardId;

            cards[_selectedIndex].MatchCard(isSuccess);
            cards[index].MatchCard(isSuccess);

            if (isSuccess)
            {
                Combo += 1;
                Managers.Game.TotalScore += Combo * _sData.score;
                Managers.Game.HistoryDatas.Add(new HistoryData()
                {
                    time = Time.realtimeSinceStartup - Managers.Game.StartTime,
                    description = string.Format("{0}짝이 맞음", cards[index]._cardData.frontCard)
                });
                if (++_successCount == totalSize / 2)
                {
                    StartCoroutine(GameOver(true));
                }
            }
            else
            {
                Combo = 0;
                Managers.Game.HistoryDatas.Add(new HistoryData()
                {
                    time = Time.realtimeSinceStartup - Managers.Game.StartTime,
                    description = string.Format("{0}와 {1}짝이 맞지 않음", cards[_selectedIndex]._cardData.frontCard, cards[index]._cardData.frontCard)
                });
            }

            _selectedIndex = 0;
            _selectedNum = 0;
        }
    }

    IEnumerator GameOver(bool isWin)
    {
        yield return new WaitForSeconds(1f);
        if (isWin)
        {
            Managers.Game.GameSceneUI.StartGame(_currentLevel + 1);
        }
        else
        {
            Managers.Game.HistoryDatas.Add(new HistoryData()
            {
                time = Time.realtimeSinceStartup - Managers.Game.StartTime,
                description = string.Format("게임 종료")
            });
            Managers.Game.GameSceneUI.EndGame();
        }
    }

    void ComboToast()
    {
        TMPro.TMP_Text text = GetText((int)Texts.ComboText);
        text.text = string.Format("{0} Combo!", Combo);
        text.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(text.transform.DOScale(1.5f, 0.5f))
        .Join(text.GetComponent<RectTransform>().DOAnchorPosY(200f, 0.5f))
        .Join(text.DOFade(1, 0.5f))
        .Append(text.DOFade(0, 1f));
    }

    public void Refresh()
    {
    }


}
