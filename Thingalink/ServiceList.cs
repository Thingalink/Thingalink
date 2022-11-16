using System;

namespace Thingalink
{
    //already a bool  Exists for features that might exist
    // please don't ever derive from this and fail to initialize the instances - being null will have the meaning
    public class NotNull
    {

    }

    public class ServiceAgent : NotNull
    {
        public virtual bool Do()
        {
            return false;
        }
    }

    public class ActionAgent : NotNull
    {
        public Action Action;
        public ActionAgent(Action action)
        {
            Action = action;
        }

        public virtual bool Do()
        {
            if (Action == null)
                return false;

            Action?.Invoke();
            return true;
        }
    }

    public class ServiceList : ListHead
    {
        public delegate void ServiceMethod(ServiceAgent service);

        public ServiceList()
        {
        }
        public ServiceList(ServiceList copy) : base(copy)
        {
        }

        public ServiceAgent CastItem(ListMember item)
        {
            return (ServiceAgent)item.Object;
        }

        public virtual void Do()
        {
            Iterate(Do);
        }
        public virtual void Do(ListMember item)
        {
            CastItem(item).Do();
        }
    }
}
