using Framework;
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;
[Serializable]
public enum AchievementType
{
    DESTROY_SHIP,
    AVATAR_COLLECTOR,
    WIN,
    WIN_STREAK_MAX,
    DESTROY_ACCUMULATIVE,
    DESTROY_SHIP_1,
    DESTROY_SHIP_2,
    DESTROY_SHIP_3,
    DESTROY_SHIP_4,
    PERFECT_GAME,
    WIN_WITH_1_SHIP,
    SPEND_GEM,
    FRAME_COLLECTOR,
}
[Serializable]
public struct AchievementUnit
{
    public int Task;
    public PConsumableType RewardType;
    public int RewardAmount;
    public string Description;
}
[Serializable]
public struct AchievementInfo
{
    public int Id;
    public string Title;
    public int Obtained;
    public Callback onClick;
    [SerializeField] private int progress; public int Progress { get { return progress; } set { progress = value; } }
    public AchievementUnit[] AchivementUnits;

    public static AchievementInfo FromJson(JSONNode node, int id)
    {
        List<AchievementUnit> achievementUnits = new List<AchievementUnit>();
        JSONArray jsonUnits = (JSONArray)node;
        for (int i = 0; i < jsonUnits.Count; i++)
        {
            achievementUnits.Add(new AchievementUnit()
            {
                Task = int.Parse(jsonUnits[i]["require"]),
                RewardAmount = int.Parse(jsonUnits[i]["reward"]),
                Description = GetDescription((AchievementType)id, int.Parse(jsonUnits[i]["require"])),
                //RewardType = (GoodType)int.Parse(node["reward_type"]),
            });
        }

        AchievementInfo info = new AchievementInfo()
        {
            Id = id,
            Title = GameConfig.AchievementName.GetLoop(id),
            AchivementUnits = achievementUnits.ToArray(),
        };
        return info;
    }

    public override string ToString()
    {
        string s = Id +"_" + Title + "_" + Progress;
        return s;
    }

    static string GetDescription(AchievementType achievementType, int amount = -1)
    {
        switch (achievementType)
        {
            case AchievementType.DESTROY_SHIP:
                return $"Destroy {amount} ships";
            case AchievementType.AVATAR_COLLECTOR:
                return $"Collect {amount} avatars";
            case AchievementType.WIN:
                return $"Win 100 battles";
            case AchievementType.WIN_STREAK_MAX:
                return $"{amount}-game winning streak";
            case AchievementType.DESTROY_ACCUMULATIVE:
                return $"destroy {amount} ships consecutively in battle";
            case AchievementType.DESTROY_SHIP_1:
                return $"Destroy {amount} single-deck ships";
            case AchievementType.DESTROY_SHIP_2:
                return $"Destroy {amount} two-deck ships";
            case AchievementType.DESTROY_SHIP_3:
                return $"Destroy {amount} three-deck ships";
            case AchievementType.DESTROY_SHIP_4:
                return $"Destroy {amount} four-deck ships";
            case AchievementType.PERFECT_GAME:
                return $"Win {amount} battles without losing any ships";
            case AchievementType.WIN_WITH_1_SHIP:
                return $"Win {amount} battles when you have only one ship left that hasn't been destroyed";
            case AchievementType.SPEND_GEM:
                return $"Spend {amount} gem";
            case AchievementType.FRAME_COLLECTOR:
                return $"Collect {amount} frames";
            default:
                return "";
        }
    }

}

public class AchievementCard : CardBase<AchievementInfo>
{
    [HideInInspector] public int Id;
    public Image BG;
    [SerializeField] protected Image Icon ;
    [SerializeField] protected TextMeshProUGUI Title;
    [SerializeField] protected TextMeshProUGUI Description;
    [SerializeField] protected TextMeshProUGUI RewardAmount;
    [SerializeField] protected Slider Progress;
    [SerializeField] protected TextMeshProUGUI TextProgress;
    [SerializeField] protected TextMeshProUGUI TextObtain;
    public override void BuildUI(AchievementInfo info)
    {
        base.BuildUI(info);
        Id = info.Id;
        int completed = 0;
        if (info.Title == null)
            return;
        for (int i = 0; i < info.AchivementUnits.Length; i++)
        {
            if (info.Progress >=info.AchivementUnits[i].Task)
            {
                completed = i + 1;
            }
            else
                break;
        }

        //show unobtained info
        if (Icon != null)
            Icon.sprite = SpriteFactory.Achievements.GetClamp(info.Id).sprites[info.Obtained];
        if (Title)
            Title.text = info.Title;
        if (Description)
            Description.text = info.AchivementUnits.GetClamp(info.Obtained).Description;
        if (RewardAmount != null)
            RewardAmount.text = "x " + TransactionCard.GetStringNumber(info.AchivementUnits.GetClamp(info.Obtained).RewardAmount);
        if (Progress != null)
        {
            Progress.maxValue = info.AchivementUnits.GetClamp(info.Obtained).Task;
            Progress.value = info.Progress;
        }
        if (TextProgress)
        {
            TextProgress.text = info.Progress.ToString() + "/" + info.AchivementUnits.GetClamp(info.Obtained).Task.ToString();
        }
        if (TextObtain)
        {
            if (info.Obtained >= info.AchivementUnits.Length)
            {
                TextObtain.text = "Completed";
                Progress.gameObject.SetActive(false);
            }
            else
            {
                if (info.Progress < info.AchivementUnits.GetClamp(info.Obtained).Task)
                {
                    TextObtain.text = "";
                    Progress.gameObject.SetActive(true);
                }
                else{
                    TextObtain.text = "Obtain";
                    Progress.gameObject.SetActive(false);
                }
            }

        }
        OnClick = info.onClick;
        if (Button)
        {
            Button.onClick.RemoveAllListeners();
            Button.onClick.AddListener(() =>
            {
                OnClicked(info);
                if (Collection)
                    Collection.SelectedCard = this;
            });
        }
        if (OnClick == null || Button.onClick == null)
        {
            gameObject.SetChildrenRecursively<Image>((img) =>
            {
                img.color = Color.gray;
            });
        }    
    }

    protected override void OnClicked(AchievementInfo info)
    {
        OnClick?.Invoke();
    }
}
