namespace ThingalinkObject
{
    public sealed class ListMember : Speck
    {
        public delegate bool Condition(ListMember thing, object passParam);
        public delegate void PassItem(ListMember thing, object param);

        public ListMember() : this(null)
        {

        }

        public ListMember(object obj) : base(obj)
        {
        }

        private ListMember _Previous;
        private ListMember _Next;
        public ListMember Previous => _Previous;
        public ListMember Next => _Next;

        public void SetValue(object obj)
        {
            Value = obj;
        }

        public void SetNext(ListMember next)
        {
            _Next = next;
        }
        public void SetPrevious(ListMember previous)
        {
            _Previous = previous;
        }


        public void Nextecute(PassItem method, object passParam)
        {
            method.Invoke(this, passParam);
            Next?.Nextecute(method, passParam);
        }
        public void Nextifcute(PassItem method, Condition condition, object passParam)
        {
            method.Invoke(this, passParam);

            if (condition(this, passParam))
                Next?.Nextecute(method, passParam);
        }
        public void Iftecute(PassItem method, Condition condition, object passParam)
        {
            if (condition(this, passParam))
                method.Invoke(this, passParam);

            Next?.Iftecute(method, condition, passParam);
        }
        public bool Ifterminate(Condition condition, object passParam)
        {
            if (condition(this, passParam))
            {
                return true;
            }
            else if (Next == null)
            {
                return false;
            }
            else
            {
                return Next.Ifterminate(condition, passParam);
            }

        }
        public void Ifterminate(PassItem method, Condition condition, object passParam)
        {
            if (condition(this, passParam))
            {
                method.Invoke(this, passParam);
            }
            else
            {
                Next?.Ifterminate(method, condition, passParam);
            }
        }
        public void Iftogo(PassItem method, Condition condition, object passParam)
        {
            if (condition(this, passParam))
            {
                method.Invoke(this, passParam);
                Next?.Nextecute(method, passParam);
            }
        }
    }
}
