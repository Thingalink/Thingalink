using System.Drawing;
using System.Windows.Forms;

namespace CobbleApp
{

    public class DrawScreen : DrawSurface
    {
        public static DrawScreen Instance;

        public DrawScreen(Form form) : base(form.ClientRectangle)
        {
            Graphics = Graphics.FromHwnd(form.Handle);

            Instance = this;
        }
    }
}
