using System;

namespace ThingalinkObject
{
    public class ListHead
    {
        public delegate void ListMethod(ListMember item);
        public delegate bool TestMethod(ListMember item, object passParam);

        protected int ManagedCount = 0;
        public int Count => ManagedCount;

        public ListMember First;
        public ListMember Last;

        public ListHead()
        {
        }
        public ListHead(ListHead copy)
        {
            ManagedCount = copy.Count;
            First = copy.First;
            Last = copy.Last;

        }

        public virtual ListMember At(int index)
        {
            if (index > Count)
                return null;

            int idx = 0;
            var selection = First;

            while (selection != null)
            {
                if (idx == index)
                    return selection;

                selection = selection.Next;
                idx++;
            }
            return null;
        }

        public void MakeFirst(ListMember item)
        {
            First = item;
            item?.SetPrevious(null);
            while(item?.Next != null)
            {
                item = item.Next;
            }
            Last = item;
        }
        public void Append(ListHead list)
        {
            if (Last == null)
            {
                First = list.First;
            }
            else
            {
                Last.SetNext(list.First);
            }
            Last = list.Last;
            ManagedCount += list.Count;
        }
        public virtual void InsertFirst(object value)
        {
            ListMember thing = value as ListMember;
            if (thing == null)
            {
                thing = new ListMember(value);
            }
            else
            {
                //may have been member of another list
                thing.SetPrevious(null);
                thing.SetNext(First);
            }


            ManagedCount++;

            First = thing;
            if (Last == null)
            {
                Last = thing;
            }
        }

        public void AddFirst(object value)
        {
            ListMember thing = value as ListMember;
            if (thing == null)
            {
                thing = new ListMember(value);
            }

            thing.SetPrevious(null);
            thing.SetNext(First);

            if (First == null)
            {
                Last = thing;
            }
            else
            {
                First.SetPrevious(thing);
            }

            ManagedCount++;

            First = thing;
        }
        public virtual ListMember Add(object value, ListMember insertAfter = null)
        {
            ListMember thing = new ListMember(value);

            ManagedCount++;

            if (First == null)
            {
                First = thing;
                Last = thing;
            }
            else if (Last == null)
            {
                First = thing;
                Last = thing;
            }
            else if (insertAfter == null || insertAfter == Last)
            {
                thing.SetPrevious(Last);
                Last.SetNext(thing);
                Last = thing;
            }
            else
            {
                insertAfter.Next?.SetPrevious(thing);
                thing.SetNext(insertAfter.Next);
                insertAfter.SetNext(thing);
            }

            return thing;
        }
        public void Iterate(ListMethod action)
        {
            IterateFrom(First, action);
        }
        public ListMember IterateFind(TestMethod action, object passParam, bool tillTrue = true)
        {
            return IterateFind(First, action, passParam, tillTrue);
        }

        public ListMember IterateFind(ListMember first, TestMethod action, object passParam, bool tillTrue = true)
        {
            var selection = first;
            while (selection != null)
            {

                if (action?.Invoke(selection, passParam) == tillTrue)
                {
                    return selection;
                }

                selection = selection.Next;
            }
            return null;
        }

        public virtual void DeleteObject(object value)
        {
            var item = IterateFind(IsItem, value);
            if (item != null)
                DeleteItem(item);
        }
        public bool IsItem(ListMember item, object passParam)
        {
            return item.Object == passParam;
        }
        public virtual void DeleteItem(ListMember item)
        {
            if (item == First)
            {
                First = item.Next;
            }
            if (item == Last)
            {
                Last = item.Previous;
            }
            item.Next?.SetPrevious(item.Previous);
            item.Previous?.SetNext(item.Next);
            ManagedCount--;
        }

        public void IterateFrom(ListMember first, ListMethod action)
        {
            var selection = first;
            while (selection != null)
            {
                action?.Invoke(selection);

                selection = selection.Next;
            }
        }
        public void IterateReverseFrom(ListMember last, ListMethod action)
        {
            var selection = Last;
            while (selection != null)
            {
                action?.Invoke(selection);
                selection = selection.Previous;
            }
        }
        public void IterateUIBreather(ListMethod action, Action alsoEach)
        {
            var selection = First;
            while (selection != null)
            {
                action?.Invoke(selection);
                selection = selection.Next;

                //expected pass Application.DoEvents();  or anything
                alsoEach?.Invoke();
            }
        }

        internal void DeleteFirstN(int lop)
        {
            var selection = First;
            int count = 0;
            while (lop > count)
            {
                DeleteItem(selection);

                selection = selection.Next;
                count++;
            }
            First = selection;
        }
    }
}
