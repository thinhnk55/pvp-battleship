using System.Threading.Tasks;
using UnityEngine;

namespace Framework
{
    public class AsyncTaskTracker
    {
        Task[] tasks;
        AsyncOperation[] asynsOp;
        float progress; public float Progress { get { CalcProgress(); return progress; } }

        public AsyncTaskTracker(Task[] tasks, AsyncOperation[] asynsOp)
        {
            this.tasks = tasks;
            this.asynsOp = asynsOp;
        }

        public void CalcProgress()
        {
            float p = 0;
            if (!tasks.IsNullOrEmpty())
                for (int i = 0; i < tasks.Length; i++)
                {
                    PDebug.Log(i);
                    if (tasks[i] != null && tasks[i].IsCompletedSuccessfully)
                    {
                        p++;
                    }
                }
            for (int i = 0; i < asynsOp.Length; i++)
            {
                p += asynsOp[i].progress;
            }
            progress = p / (tasks.Length + asynsOp.Length);
        }
    }
}
