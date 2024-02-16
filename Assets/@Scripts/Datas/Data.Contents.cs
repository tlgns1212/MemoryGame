using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{

    #region  StageData
    public class StageData
    {
        public int level;
        public int totalTime;
        public int score;
        public int verSize;
        public int horSize;
    }

    [Serializable]
    public class StageDataLoader : ILoader<int, StageData>
    {
        public List<StageData> stages = new List<StageData>();
        public Dictionary<int, StageData> MakeDict()
        {
            Dictionary<int, StageData> dict = new Dictionary<int, StageData>();
            foreach (StageData stage in stages)
                dict.Add(stage.level, stage);
            return dict;
        }
    }
    #endregion

    #region  CardData
    public class CardData
    {
        public int id;
        public string frontCard;
        public string backCard;
    }

    [Serializable]
    public class CardDataLoader : ILoader<int, CardData>
    {
        public List<CardData> cards = new List<CardData>();
        public Dictionary<int, CardData> MakeDict()
        {
            Dictionary<int, CardData> dict = new Dictionary<int, CardData>();
            foreach (CardData card in cards)
                dict.Add(card.id, card);
            return dict;
        }
    }
    #endregion

}
