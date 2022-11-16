using System;
using System.Threading;
using Thingalink;

namespace CobbleApp
{
    public class WaitMule : ReadyMule //waiting for user and or the action. 
    {
        protected Action ready;

        public WaitMule(Action chug) : base(chug)
        {
        }

        protected override void EndFlag()
        {
            IsDone = true;

            //Status.Log("Loader Finished");
            ready?.Invoke();
        }

        public void Ready(Action callback)
        {
            if (Done)
                callback.Invoke();
            else
                ready = callback;

        }
    }

    /// <summary>
    /// AutoStarting
    /// </summary>
    public class ReadyMule : TaskMule
    {
        public ReadyMule(Action chug) : base(chug)
        {
            StartThread();
        }
    }

    public class TaskMule : ThreadMule
    {
        protected bool IsDone;
        public bool Done => IsDone;

        public TaskMule(Action chug) : base(chug)
        {

        }

        protected virtual void EndFlag()
        {
            IsDone = true;
        }

        public override void StartThread()
        {
            IsDone = false;
            Thread = new Thread(ChugThread);
            Thread.Start();
        }

        protected virtual void ChugThread()
        {
            Chug?.Invoke();
            EndFlag();
        }
    }

    //loop or sequence
    //void return
    public class ThreadMule : ServiceAgent 
    {
        protected Thread Thread;
        protected Action Chug;

        public ThreadMule(Action chug)
        {
            Chug = chug;
        }

        public override bool Do()
        {
            if (Thread == null)
            {
                StartThread();
                return true;
            }
            return false;
        }

        public virtual void StartThread()
        {
            Thread = new Thread(Chug.Invoke);
            Thread.Start();
        }

        public void Cancel()
        {
            Thread.Abort();
            Thread = null;
        }
    }
}
