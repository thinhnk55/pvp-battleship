using Framework;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardCollection : CardCollectionBase<BoardInfo>
{
    private void Awake()
    {
        UpdateUIs();
    }
    public override void UpdateUIs()
    {
        List<BoardInfo> infos = new List<BoardInfo>();
        for (int i = 0; i < GameData.ListBoard.Count; i++)
        {
            BoardInfo info = GameData.ListBoard[i];
            info.Id = i;
            info.row = CoreGame.Instance.player.row;
            info.column = CoreGame.Instance.player.column;
            infos.Add(info);
        }
        BuildUIs(infos);
    }
    public override void BuildUIs(List<BoardInfo> infos)
    {
        base.BuildUIs(infos);
    }

    public override void ModifyUIAt(int i, BoardInfo info)
    {
        base.ModifyUIAt(i, info);
    }
}
