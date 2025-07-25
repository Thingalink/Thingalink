using System;
using System.Collections.Generic;

namespace Thingalink
{
    /// <summary>
    /// not trying to replace collections.generic. if you want indexed acess then you have it already
    /// was addicted to linq and Lists<ofType> found awkward clarity and maintenance issues.     
    /// LinkedList<looksgenericbutisstillatype> still needs compatible code that requirs your references to correspeond compatibly.
    /// the old reflection issue just relocated. The point of this class and ListMember is to have consistant item containers.
    /// there are usage patterns solved by having the List handler not need to know the type being passed
    /// the casting still happens but it happens in the object that knows what type is expected in the container rather than the passer
    /// the consuming modules provides the handler for the object so that the list owner need only source data and and deliver it as post office
    /// end result is easy binding code where the metadata is passed into the objects that consume it without the passer needing awareness
    /// yes todo the double isn't always used. there should be a single link option first. yea maybe it is just Ienumberable
    /// </summary>
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

        /// <summary>
        /// the links structure doesn't serialize normal. still need conversion 
        /// </summary>
        /// <returns></returns>
        public List<object> ExportCollection()
        {
            var l = new List<object>();
            First.Nextecute(AddToList, l);
            return l;
        }

        protected void AddToList(ListMember thing, object list)
        {
            ((List<object>)list).Add(thing.Object);
        }

        /// <summary>
        /// the links structure doesn't serialize normal. still need conversion 
        /// </summary>
        /// <returns></returns>
        public void ImportCollection(List<object> l)
        {
            foreach(var item in l)
            {
                Add(item);
            }
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
            ManagedCount = item == null ? 0 : 1;
            item?.SetPrevious(null);
            while(item?.Next != null)
            {
                item = item.Next;
                ManagedCount++;
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
