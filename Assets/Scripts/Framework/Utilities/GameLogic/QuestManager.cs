using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public static class QuestManager
    {
        static public Dictionary<QuestType, Quest<QuestType>> quests;
        public static void AddQuest(this QuestType requireTypes, int[] requires)
        {
            quests.Add(requireTypes, new Quest<QuestType>()
            {
                requireTypes = requireTypes,
                requires = requires
            });
        }
        public static void AddListenerOnProgress(this QuestType requireTypes, Callback<int> onProgress)
        {
            quests[requireTypes].OnProgress += onProgress;
        }
        public static void RemoveListenerOnProgress(this QuestType requireTypes, Callback<int> onProgress)
        {
            quests[requireTypes].OnProgress -= onProgress;
        }
        public static void SetProgress(this QuestType requireTypes, int progress)
        {
            quests[requireTypes].Progress = progress;
        }
    }

}

