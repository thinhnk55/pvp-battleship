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
    public bool Obtained;
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
    [SerializeField] Image obtainIndicator;

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
        if (!info.Obtained)
        {
            obtainIndicator?.gameObject.SetActive(false);
            collect?.onClick.AddListener(() =>
            {
                if (info.OnCollect != null)
                {
                    info.OnCollect?.Invoke(info);
                }
            });
            change?.onClick.AddListener(() =>
            {
                if (info.OnChange != null)
                {
                    info.OnChange?.Invoke(info);
                }

            });
        }
        else
        {
            if (!change) // not the normal quest
            {
                obtainIndicator?.gameObject.SetActive(true);
                gameObject.SetChildrenRecursively<Image>((img) =>
                {
                    img.color = Color.gray;
                });
                collect.gameObject.SetActive(false);
            }

        }
        if (progressBar)
        {
            progressBar.maxValue = info.Require;
            progressBar.value = info.Progress;
        }
    }
}
