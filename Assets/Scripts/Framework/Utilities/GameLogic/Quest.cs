using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public enum QuestType
    {
        SHIP_DESTROY,
        AVATAR,
        WIN_COUNT,
        WIN_STREAK,
        SHIP_DESTROY_CONSECUTIVE,
        SHIP_0_DESTROY,
        SHIP_1_DESTROY,
        SHIP_2_DESTROY,
        SHIP_3_DESTROY,
        PERFECT_GAME,
        ALIVE_1_SHIP,
        GEM_USED,
        AVATAR_FRAME,

        LUCKY_SHOT_COUNT,
        DESTROY_SHIP_CONSECUTIVE_3

    }
    public class Quest<T>
    {
        public int[] requires;
        public T requireTypes;
        public Callback<int> OnProgress;
        [SerializeField] private int progress; public int Progress
        {
            get { return progress; }
            set
            {
                if (value>progress)
                {
                    progress = value;
                    OnProgress?.Invoke(value);
                } 
            } 
        }


    }
}