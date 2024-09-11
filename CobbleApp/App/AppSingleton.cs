using System.Drawing;

namespace CobbleApp
{
    public class AppSingleton
    {
        private static PaintsList paintsList;
        public static PaintsList PaintsList => paintsList;

        public static Paint DefaultTextColor;
        public static Paint DefaultBackColor;

        private static FontsList fontsList;
        public static FontsList FontsList => fontsList;

        public AppSingleton(Color defaultColor, int thick = 2) 
        {
            if (paintsList != null)
            {
                //enoforced singleton
                throw new System.Exception("Singleton Already Defined");
            }
            paintsList = new PaintsList(defaultColor, thick);

            DefaultTextColor = PaintsList.SelectedPaint;

           fontsList = new FontsList();
        }
    }
    
}
