using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    private void Awake() {
        Init();
    }

    protected override void Init()
    {
        base.Init();
        SceneType = Define.Scene.GameScene;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Time.timeScale = 1;


        Managers.UI.ShowSceneUI<UI_GameScene>();
    }

    public override void Clear()
    {

    }
}
