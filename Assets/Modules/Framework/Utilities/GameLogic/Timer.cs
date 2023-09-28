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
        private int triggerInterval_Sec = 1; public int TriggerInterval_Sec { get { return triggerInterval_Sec; } set { triggerInterval_Sec = Mathf.Clamp(value, 1, int.MaxValue); } }
        private long beginPoint; public long BeginPoint
        {
            get { return beginPoint; }
            set
            {
                beginPoint = value;
                elapse = 0;
                MarkedPoint = value;
            }
        }
        private long markedPoint; public long MarkedPoint { 
            get { return markedPoint; } 
            set { markedPoint = value; } 
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
                    if (ResidalTime_Sec - 1 == 0)
                    {
                        OnTrigger?.Invoke();
                    }
                }
            }
        }
        public long ElaspedTime_Tick { get { return DateTime.UtcNow.Ticks - beginPoint; } }
        public long ResidalTime_Sec { get { return ElaspedTime_Tick.ToSecond() % triggerInterval_Sec; } }
        public long RemainTime_Sec { get { return Math.Clamp(TriggerInterval_Sec - ResidalTime_Sec, 0, TriggerInterval_Sec); } }
        public int TriggersFromBegin { get { return ElaspedTime_Tick.ToSecond() / triggerInterval_Sec; } }
        public int TriggersFromMark { get { return TriggersFromBegin - ((markedPoint - beginPoint).ToSecond() / triggerInterval_Sec); } }

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
        public void Begin(long? tick = null)
        {
            if (tick.HasValue)
            {
                BeginPoint = tick.Value;
            }
            else
            {
                BeginPoint = DateTime.UtcNow.Ticks;
            }
        }
        public void Mark()
        {
            MarkedPoint = DateTime.UtcNow.Ticks;
        }
    }
}

