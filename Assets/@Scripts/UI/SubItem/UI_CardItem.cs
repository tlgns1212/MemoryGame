using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_CardItem : UI_Base
{
    #region Enum
    #endregion

    public int _cardId;
    int _index;
    public Data.CardData _cardData;
    private bool _isFront;
    public bool IsFront
    {
        get { return _isFront; }
        set
        {
            _isFront = value;
            StartCoroutine(RotateCard());
        }
    }
    public bool _isClickable;
    Image _image;
    Action<int> _parAction;
    private void Awake()
    {
        Init();
    }
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        _isFront = false;
        _isClickable = true;
        _image = gameObject.GetComponent<Image>();

        // Refresh();

        gameObject.BindEvent(OnClickCard);

        return true;
    }

    public void SetInfo(int index, int backType, int backNum, int cardType, int cardNum, Action<int> callback)
    {
        _index = index;
        _cardId = backType * 10000 + backNum * 1000 + cardType * 100 + cardNum;
        _cardData = Managers.Data.CardDic[_cardId];
        _parAction = callback;

        Refresh();
    }

    void OnClickCard()
    {
        if (!_isClickable || IsFront)
            return;
        IsFront = !IsFront;
    }

    public void MatchCard(bool isSuccess)
    {
        if (isSuccess)
        {
            _isClickable = false;
        }
        else
        {
            IsFront = false;
        }
    }

    public IEnumerator RotateCard()
    {
        _isClickable = false;
        // Managers.Sound.PlayFlipCard();
        if (_isFront)
        {
            for (float i = 0f; i <= 180f; i += 10f)
            {
                transform.rotation = Quaternion.Euler(0f, i, 0f);
                if (i == 90f)
                {
                    _image.sprite = Managers.Resource.Load<Sprite>(_cardData.frontCard);
                }
                yield return new WaitForSeconds(0.01f);
            }
            _parAction.Invoke(_index);
        }
        else if (!_isFront)
        {
            for (float i = 180f; i >= 0f; i -= 10f)
            {
                transform.rotation = Quaternion.Euler(0f, i, 0f);
                if (i == 90f)
                {
                    _image.sprite = Managers.Resource.Load<Sprite>(_cardData.backCard);
                }
                yield return new WaitForSeconds(0.01f);
            }
        }
        _isClickable = true;
    }


    public void Refresh()
    {
    }

}
