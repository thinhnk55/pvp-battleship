using Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public struct QuestInfo
{
    public int Id;
    public string Description;
    public int Progress;
    public int Require;
    public int Reward;
    public Callback<QuestInfo> OnCollect;
    public Callback<QuestInfo> OnChange;
}
public class QuestCard : CardBase<QuestInfo>
{
    [SerializeField] TextMeshProUGUI reward;
    [SerializeField] TextMeshProUGUI des;
    [SerializeField] Slider progressBar;
    [SerializeField] TextMeshProUGUI progress;
    [SerializeField] Button collect;
    [SerializeField] Button change;

    protected override void OnClicked(QuestInfo info)
    {
        throw new System.NotImplementedException();
    }

    public override void BuildUI(QuestInfo info)
    {
        base.BuildUI(info);
        reward?.SetText(info.Reward.ToString());
        des?.SetText(info.Description);
        progress?.SetText(info.Progress.ToString()+"/"+info.Require.ToString());
        collect?.onClick.AddListener(() =>
        {
            info.OnCollect?.Invoke(info);
        });
        change?.onClick.AddListener(() =>
        {
            info.OnChange?.Invoke(info);
        });
        if (progressBar)
        {
            progressBar.maxValue = info.Require;
            progressBar.value = info.Progress;
        }
    }
}
