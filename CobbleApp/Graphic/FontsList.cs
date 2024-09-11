using System.Drawing;
using System.Windows.Forms;
using Thingalink;

namespace CobbleApp
{
    /// <summary>
    /// I made this and then realize(remember) the measure process makes many fonts that are not kept
    /// and afterwards now it does just look like a Dictionary I am less sure needs to exist.
    /// (commit: was certain the whole Measured Text and Theme was bad and not in the base library if at all.
    /// todo this for Paint values. becaue having too many brushes in a loop can break memory limit.
    /// </summary>
    public class FontsList
    {
        protected ListHead FontsUsed = new ListHead();

        protected string selectedFontName;
        public string SelectedFontName => selectedFontName;

        protected Font selectedFont;
        public Font SelectedFont => selectedFont;

        public void Add(Font font)
        {
            FontsUsed.Add(font);
            selectedFont = font;
        }

        public Font Ask(string name, int size)
        {
            selectedFontName = name;
            var usedFont = FontsUsed.IterateFind(IsFont, name);

            if (usedFont == null)
            {
                usedFont = FontsUsed.Add(new FontUsed(name));
            }

            selectedFont = ((FontUsed)usedFont.Object).Ask(size);
            return selectedFont;
        }
        public Font Ask(int size)
        {
            if(SelectedFontName == null)
            {
                return  ((FontUsed)FontsUsed.First?.Object).Ask(size);
            }
            selectedFont = Ask(selectedFontName, size);
            return selectedFont;
        }

        private static bool IsFont(ListMember item, object passParam)
        {
            return ((FontUsed)item.Object).FontName == (string)passParam;
        }
    }
    public class FontUsed
    {
        private string fontName;
        public string FontName => fontName;
        public ListHead SizesUsed;

        public FontUsed(string name)
        {
            fontName = name;
            SizesUsed = new ListHead();
        }

        public Font Ask(int size)
        {
            var usedFont = SizesUsed.IterateFind(IsSize, size);

            if (usedFont == null)
            {
                usedFont = SizesUsed.Add(new Font(FontName, size));
            }
            return (Font)usedFont.Object;
        }

        private static bool IsSize(ListMember item, object passParam)
        {
            return ((Font)item.Object).Size == (int)passParam;
        }
    }
    
    
}
