using Thingalink;

namespace CobblePaintBox
{
    public class CellList : ListHead
    {
        public delegate void ItemMethod(Cell cell);
        public delegate bool ItemTest(Cell cell);

        public Cell CastItem(ListMember item)
        {
            return (Cell)item.Object;
        }

        public void Iterate(ItemMethod action, ListMember first = null)
        {
            var selection = first ?? First;
            while (selection != null)
            {
                action?.Invoke(CastItem(selection));

                selection = selection.Next;
            }
        }

        public ListMember IterateFind(ItemTest action, ListMember first = null, bool tillTrue = true)
        {
            var selection = first ?? First;
            while (selection != null)
            {

                if (action?.Invoke(CastItem(selection)) == tillTrue)
                {
                    return selection;
                }

                selection = selection.Next;
            }
            return null;
        }

        //add? type enforce or dont misuse
    }
}
