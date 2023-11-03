using Framework;
using System;
using UnityEngine;

namespace Framework
{
    public class PDataTime : PDataBlock<PDataTime>
    {
        public static long Hour = 3600;
        public static long Day = 84400;
        public static long FirstOpenGameTime { get { return Instance.firstOpenGameTime; } set { Instance.firstOpenGameTime = value; } } public long firstOpenGameTime;
        public static long OpenGameTime { get { return Instance.openGameTime; } set { Instance.openGameTime = value; } } public long openGameTime;
        /// <summary>/// total time first open to now/// </summary>
        public static long TotalTime { get { return Instance.totalTime; } set { Instance.totalTime = value; } } public long totalTime;
        /// <summary>sum of playtime/// </summary>
        public static long TotalTimePlay { get { return Instance.totalTimePlay; } set { Instance.totalTimePlay = value; } } public long totalTimePlay;
        protected override void Init()
        {
            base.Init();
            Instance.firstOpenGameTime = Instance.firstOpenGameTime == 0 ? DateTime.UtcNow.Ticks : Instance.firstOpenGameTime;
        }
    }
}