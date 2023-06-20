using Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public struct RankInfo
{
    public string Title;
    public Sprite Icon;
    public int Point;

}
public class RankCard : CardBase<RankInfo>
{
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] Image icon;
    [SerializeField] Slider slider;

    protected override void OnClicked(RankInfo info)
    {
        throw new System.NotImplementedException();
    }

    public override void BuildUI(RankInfo info)
    {
        base.BuildUI(info);
        if(title) title.text = info.Title;
        if(icon) icon.sprite = info.Icon;
        if (slider)
        {
            slider.maxValue = GameData.RankConfigs[GameData.Player.Rank].Point;
            slider.value = GameData.Player.Point; 
        }
        if (Button)
        {
            Button.onClick.RemoveAllListeners();
            Button.onClick.AddListener(() =>
            {

            });
        }
    }
}
