using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_GameSpaceItem : UI_Base
{
    #region Enum
    #endregion

    List<UI_CardItem> cards;
    int _selectedIndex;
    int _selectedNum;
    int _successCount;
    int _currentLevel;

    private void Awake()
    {
        Init();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        // Refresh();

        return true;
    }

    public void SetInfo(int stageLevel)
    {
        _currentLevel = stageLevel;
        Data.StageData sData = Managers.Data.StageDic[stageLevel];

        _selectedNum = 0;
        _selectedIndex = 0;
        _successCount = 0;
        Managers.Game.Time = sData.totalTime;
        Managers.Game.Level = sData.level;
        int totalSize = sData.horSize * sData.verSize;
        cards = new List<UI_CardItem>();
        List<int> cardNums = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };
        cardNums.Shuffle();

        for(int i = 0; i < totalSize / 2; i++){
            UI_CardItem card1 = Managers.UI.MakeSubItem<UI_CardItem>(transform);
            int randNum = cardNums[i];
            card1.SetInfo(i*2,1, 0, 1, randNum, (int index)=>{
                if(_selectedNum == 0){
                    _selectedIndex = index;
                    _selectedNum = cards[index]._cardId;
                }
                else{
                    bool isSuccess = _selectedNum == cards[index]._cardId;

                    cards[_selectedIndex].MatchCard(isSuccess);
                    cards[index].MatchCard(isSuccess);

                    if(isSuccess){
                        Managers.Game.Combo += 1;
                        Managers.Game.TotalScore += Managers.Game.Combo * sData.score;
                        if(++_successCount == totalSize/2){
                            GameOver(true);
                        }
                    }
                    else{
                        Managers.Game.Combo = 0;
                    }

                    _selectedIndex = 0;
                    _selectedNum = 0;
                }
            });
            UI_CardItem card2 = Managers.UI.MakeSubItem<UI_CardItem>(transform);
            card2.SetInfo(i*2+1,1, 0, 1, randNum, (int index)=>{
                if(_selectedNum == 0){
                    _selectedIndex = index;
                    _selectedNum = cards[index]._cardId;
                }
                else{
                    bool isSuccess = _selectedNum == cards[index]._cardId;

                    cards[_selectedIndex].MatchCard(isSuccess);
                    cards[index].MatchCard(isSuccess);

                    if(isSuccess){
                        Managers.Game.Combo += 1;
                        Managers.Game.TotalScore += Managers.Game.Combo * sData.score;
                        if(++_successCount == totalSize/2){
                            GameOver(true);
                        }
                    }
                    else{
                        Managers.Game.Combo = 0;
                    }

                    _selectedIndex = 0;
                    _selectedNum = 0;
                }
            });
            cards.Add(card1);
            cards.Add(card2);
        }

        gameObject.GetComponent<GridLayoutGroup>().constraintCount = sData.verSize;

        // cards.Shuffle();
        for(int i = 0; i < cards.Count; i++){
            cards[i].transform.SetSiblingIndex(i);
        }

        Refresh();
    }

    public void GameOver(bool isWin){
        if(isWin){
            foreach(UI_CardItem card in cards){
                Destroy(card);
            }
            cards = null;
            Managers.Game.GameSceneUI.StartGame(_currentLevel+1);
        }
        else{
            Managers.Game.GameSceneUI.EndGame();
        }
    }

    public void Refresh()
    {
    }


}
