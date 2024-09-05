using System;

namespace Thingalink
{
    public class LoopSet
    {
        public ListMember On;

        public int Index;
        public int Spin;
        public ListHead List;

        public LoopSet(ListHead list, int spin)
        {
            List = list;
            Spin = spin;
        }
        public void Reset(ListHead list, int spin)
        {
            List = list;
            Spin = spin;
            Index = 0;
        }

        public void Loop(ListHead.ListMethod action)
        {
            if (On == null)
                On = List.First;
            Index = 0;
            while (On != null && Index < Spin)
            {
                action?.Invoke(On);
                Index++;

                On = On.Next;
            }
        }
        public void LoopAll(ListHead.ListMethod action, Action breather)
        {
            On = null;
            Loop(action);
            breather?.Invoke();
            while (On != null)
            {
                Loop(action);
                breather?.Invoke();
            }
        }
    }
}
