using System.Drawing;
using Thingalink;

namespace CobbleApp
{

    public class PaintsList 
    {
        protected ListHead PaintsUsed = new ListHead();
        
        protected Paint selectedPaint;
        public Paint SelectedPaint => selectedPaint;

        public PaintsList()
        {
        }
        public PaintsList(Color defaultColor, int thick = 2)
        {
            selectedPaint = new Paint(defaultColor, thick);
            PaintsUsed.Add(selectedPaint);
        }

        public Paint Ask(Color color)
        {
            var usedItem = PaintsUsed.IterateFind(IsColor, color);

            if (usedItem == null)
            {
                usedItem = PaintsUsed.Add(new Paint(color));
            }

            selectedPaint = (Paint)usedItem.Object;
            return selectedPaint;
        }

        public Paint Ask(Color color, int size)
        {
            var searchParams = new ListHead();
            searchParams.Add(color);
            searchParams.Add(size);
            var usedItem = PaintsUsed.IterateFind(IsColorPen, searchParams);

            if (usedItem == null)
            {
                usedItem = PaintsUsed.Add(new Paint(color, size));
            }

            selectedPaint = (Paint)usedItem.Object;
            return selectedPaint;
        }

        private static bool IsColor(ListMember item, object passParam)
        {
            return ((Paint)item.Object).Color == (Color)passParam;
        }
        private static bool IsColorPen(ListMember item, object passParam)
        {
            var param = ((ListHead)passParam).First;
            return ((Paint)item.Object).Color == (Color)item.Object && ((Paint)param.Object).Thick == (int)param.Next?.Object;
        }
    }
    
}
