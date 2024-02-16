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
    Data.CardData _cardData;
    public bool _isFront;
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
        print(_cardId);
        print(_cardData.frontCard);
        _parAction = callback;

        Refresh();
    }

    void OnClickCard(){
        if (!_isClickable)
            return;
        StartCoroutine(RotateCard());
    }

    public void MatchCard(bool isSuccess){
        if(isSuccess){
            _isClickable = false;
        }
        else{
            _isFront = true;
            StartCoroutine(RotateCard());
        }
    }

    public IEnumerator RotateCard(){
        _isClickable = false;
        if(!_isFront)
        {
            for(float i = 0f; i <= 180f; i += 10f){
                transform.rotation = Quaternion.Euler(0f, i, 0f);
                if(i == 90f)
                {
                    _image.sprite = Managers.Resource.Load<Sprite>(_cardData.frontCard);
                }
                yield return new WaitForSeconds(0.01f);
            }
            _parAction.Invoke(_index);
        }
        else if(_isFront)
        {
            for(float i = 180f; i >= 0f; i -= 10f){
                transform.rotation = Quaternion.Euler(0f, i, 0f);
                if(i == 90f)
                {
                    _image.sprite = Managers.Resource.Load<Sprite>(_cardData.backCard);
                }
                yield return new WaitForSeconds(0.01f);
            }
        }
        _isFront = !_isFront;
        _isClickable = true;
    }


    public void Refresh()
    {
    }

}
