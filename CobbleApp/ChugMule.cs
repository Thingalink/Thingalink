using System;
using System.Threading;

namespace CobbleApp
{
    public class ChugMule// : ServiceAgent 
    {
        public static ChugMule Instance;

        Thread Thread;
        Action Chug;

        private bool Go;
        public bool Hold;

        public ChugMule(Action work)
        {
            Chug = work;

            Go = true;

            if (Instance == null)
                Instance = this;

        }

        public bool Do()
        {
            if (Thread != null)
            {
                Cancel();
                return false;//not alot of reason to read this result false for why are you calling it again - reset is the legit reason
            }
            Start();
            return true;
        }

        public void Start()
        {
            Thread = new Thread(ChugThread);
            Thread.Start();
        }

        public void End()
        {
            Go = false;
        }
        public virtual void Ending()
        {
        }
        public void Cancel()
        {
            Pause();
            Thread?.Abort();
            Thread = null;
        }

        public void Pause()
        {
            Hold = true;
        }

        public void Resume()
        {
            if (Thread == null)
            {
                Start();
            }
            Hold = false;
        }

        protected void ChugThread()
        {
            while (true)// (Chuging)
            {
                if (CheckHold())
                {
                    //eat less process while paused
                    Thread.Sleep(100);
                    continue;
                }

                if (!Go)
                {
                    Ending();
                    return;
                }

            //    try
            //    {
            //        if (Enabled.Value)

                        Chug.Invoke();

            //        Status.Instance.Tick();
            //    }
            //    }
            //    catch (Exception e)
            //{
            //    Status.Log(e.Message);
            //    Status.Update();
            //}

        }
        }
        public virtual bool CheckHold()
        {
            return Hold;
        }
    }
}
