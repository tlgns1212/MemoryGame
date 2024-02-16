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
    }

    enum Buttons
    {
        StartButton
    }

    enum Texts
    {
    }

    enum Images
    {
        Background,
    }
    #endregion


    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        BindImage(typeof(Images));

        GetButton((int)Buttons.StartButton).gameObject.BindEvent(OnClickStartButton);

        return true;
    }



    private void Awake()
    {
        Init();
    }

    private void Start()
    {
    }

    void OnClickStartButton(){
        GetImage((int)Images.Background).gameObject.SetActive(false);
    }
}
