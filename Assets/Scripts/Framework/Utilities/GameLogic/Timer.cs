using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Framework
{
    public class Timer<T> : Singleton<Timer<T>>
    {
        private int triggerIntervalInSecond; public int TriggerIntervalInSecond { get { return triggerIntervalInSecond; } set { triggerIntervalInSecond = Mathf.Clamp(value, 1, int.MaxValue); } }
        private long lastTime; public long LastTime
        {
            get { return lastTime; }
            set
            {
                lastTime = value;
                elapse = 0;
                TimePoint = value;
            }
        }

        private float elapse; public float ELaspe
        {
            get { return elapse; }
            set
            {
                elapse = value;
                if (elapse >= 1)
                {
                    elapse -= 1;
                    if (RemainTimeInsecond - 1 == 0)
                    {
                        OnTrigger?.Invoke();
                    }
                }

            }
        }
        private long timePoint; public long TimePoint { get { return timePoint; } set { timePoint = value; } }
        public long ElaspedTime { get { return DateTime.UtcNow.Ticks - lastTime; } }
        public long ResidalTimeInSecond { get { return ElaspedTime.ToSecond() % triggerIntervalInSecond; } }
        public long RemainTimeInsecond { get { return Math.Clamp(TriggerIntervalInSecond - ResidalTimeInSecond, 0, TriggerIntervalInSecond); } }
        public int TriggerCountTotal { get { return ElaspedTime.ToSecond() / triggerIntervalInSecond; } }
        public int TriggerCountFromTimePoint { get { return TriggerCountTotal - ((timePoint - lastTime).ToSecond() / triggerIntervalInSecond); } }
        public Callback OnTrigger;
        public Callback OnElapse;

        public void Init(Callback onTrigger, Callback onElapse){
            OnTrigger = onTrigger;
            OnElapse = onElapse;
        }
        public void Elasping()
        {
            ELaspe += Time.deltaTime;
            OnElapse?.Invoke();
        }
    }
}

