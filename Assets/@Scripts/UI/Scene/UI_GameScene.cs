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
        UI_GameSpaceItem,
    }

    enum Buttons
    {
        StartButton,
        BackButton
    }

    enum Texts
    {
        ScoreText,
        TimerText
    }

    enum Images
    {
        StartGroup,
    }
    #endregion

    UI_GameSpaceItem gameSpaceItem;

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

        GetObject((int)GameObjects.StageGroup).SetActive(false);

        gameSpaceItem = GetObject((int)GameObjects.UI_GameSpaceItem).GetOrAddComponent<UI_GameSpaceItem>();

        Refresh();

        return true;
    }

    private void Awake()
    {
        Init();
    }

    private void Refresh(){
        GetText((int)Texts.ScoreText).text = Managers.Game.TotalScore.ToString();
    }

    void OnClickStartButton(){
        GetImage((int)Images.StartGroup).gameObject.SetActive(false);
        GetObject((int)GameObjects.StageGroup).SetActive(true);

        StartGame();
    }

    IEnumerator TimeTicking(){
        while(true){
            yield return new WaitForSeconds(1f);
            Managers.Game.Time--;
            TimeSpan timeSpan = TimeSpan.FromSeconds(Managers.Game.Time);
            string formattedTime = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
            GetText((int)Texts.TimerText).text = formattedTime;
            if(Managers.Game.Time <= 0){
                EndGame();
                break;
            }
        }
    }

    public void StartGame(int level = 1){
        gameSpaceItem.SetInfo(level);
        Managers.Game.TotalScore = 0;
        Managers.Game.Combo = 0;
        StartCoroutine(TimeTicking());
    }

    public void EndGame(){
        GetImage((int)Images.StartGroup).gameObject.SetActive(true);
        GetObject((int)GameObjects.StageGroup).SetActive(false);
    }

    private void OnDestroy()
    {
        if (Managers.Game != null)
            Managers.Game.OnResourcesChanged -= Refresh;
    }
}
