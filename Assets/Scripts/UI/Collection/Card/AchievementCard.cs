using Framework;
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
[Serializable]
public enum AchievementType
{
    ENVOVY_OF_WAR,
    COLLECTOR,
    EXPERIENCER,
    DOMINATOR,
    PAYLOAD_POWERHOUSE,
    PREDATOR,
    FIGHTER,
    EXAMINATOR,
    UNSTOPABLE,
    TACTICAL_GENIUS,
    LIFE_IS_FRAGILE,
    SHOPAHOLIC,

}
[Serializable]
public struct AchievementUnit
{
    public int Task;
    public PResourceType RewardType;
    public int RewardAmount;
    public string Description;
}
[Serializable]
public struct AchievementInfo
{
    public int Id;
    public string Title;
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

        string title = jsonUnits[0]["name"];

        AchievementInfo info = new AchievementInfo()
        {
            Id = id,
            Title = title.Substring(0, title.Length - 1),
            AchivementUnits = achievementUnits.ToArray(),
            Progress = GameData.Player.Achievement[id],
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
            case AchievementType.ENVOVY_OF_WAR:
                return $"Destroy {amount} ships";
            case AchievementType.COLLECTOR:
                return $"Collect {amount} avatars";
            case AchievementType.EXPERIENCER:
                return $"Win 100 battles";
            case AchievementType.DOMINATOR:
                return $"{amount}-game winning streak";
            case AchievementType.PAYLOAD_POWERHOUSE:
                return $"destroy {amount} ships consecutively in battle";
            case AchievementType.PREDATOR:
                return $"Destroy {amount} single-deck ships";
            case AchievementType.FIGHTER:
                return $"Destroy {amount} two-deck ships";
            case AchievementType.EXAMINATOR:
                return $"Destroy {amount} three-deck ships";
            case AchievementType.UNSTOPABLE:
                return $"Destroy {amount} four-deck ships";
            case AchievementType.TACTICAL_GENIUS:
                return $"Win {amount} battles without losing any ships";
            case AchievementType.LIFE_IS_FRAGILE:
                return $"Win {amount} battles when you have only one ship left that hasn't been destroyed";
            case AchievementType.SHOPAHOLIC:
                return $"Spend {amount} gem";
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

        int obtain = Mathf.Clamp(GameData.Player.AchievementObtained[info.Id], 0, 4 );

        //show unobtained info
        if (Icon != null)
            Icon.sprite = SpriteFactory.Achievements[info.Id].sprites[obtain];
        if (Title)
            Title.text = info.Title;
        if (Description)
            Description.text = info.AchivementUnits[obtain].Description;
        if (RewardAmount != null)
            RewardAmount.text = "x " + info.AchivementUnits[obtain].RewardAmount.ToString();
        if (Progress != null)
        {
            Progress.maxValue = info.AchivementUnits[obtain].Task;
            Progress.value = info.Progress;
        }
        if (TextProgress)
        {
            TextProgress.text = info.Progress.ToString() + "/" + info.AchivementUnits[obtain].Task.ToString();
        }
        OnClick = info.onClick;
        if (Button)
        {
            Button.onClick.RemoveAllListeners();
            Button.onClick.AddListener(() =>
            {
                if (Collection)
                    Collection.SelectedCard = this;
                OnClicked(info);
            });
        }
        if (OnClick == null && Button.onClick == null)
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
