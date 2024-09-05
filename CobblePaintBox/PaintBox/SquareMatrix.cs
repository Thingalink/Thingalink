using CobbleApp;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Thingalink;

namespace CobblePaintBox
{
    

    public class SquareMatrix : Zone
    {
        public static SquareMatrix Instance;

        public CellList Cells;
        public ListHead Rows;
        public ListHead Columns;
        public ListHead UpdateCells;
        public ListHead Row;
        public ListHead PreviousRow;
        public ListHead Column;
        public ListHead PreviousColumn;
        public BitmapSurface Surface0;
        public BitmapSurface Surface1;
        public BitmapSurface Compost;
        //public BitmapSurface BitmapSurface;
        public new BitmapSurface Surface => (BitmapSurface)base.Surface;

        protected int CellW;
        int RowY;
        int RowX;
        public bool Flop;
        public bool RowFlop;
        Cell Last;
        int deep;
        int depth;
        public bool Updates;

        protected bool InitColor;

        protected DateTime InitStamp;

        long lastTick;

        public ListHead Banks;
        public ListMember BankSwap;
        public LoopSet BankSet;

        public SquareMatrix(int x, int y, int w, int h, int initialCells, ContainerZone parent, Bitmap bitmap = null) : this(new Rectangle(x, y, w, h), initialCells, parent, bitmap)
        {

        }
        public SquareMatrix(Rectangle rect, int initialCells, ContainerZone parent, Bitmap bitmap = null) : base(rect, parent)
        {
            Instance = this;

            Cells = new CellList();
            Rows = new ListHead();
            Columns = new ListHead();

            UpdateCells = new ListHead();
            Row = new ListHead();
            Column = new ListHead();


            deep = Rectangle.Width / 9;
            depth = 1;

            if (initialCells == 0)
            {
                CellW = 1;
            }
            else
                CellW = Rectangle.Width / initialCells;

            RowY = Rectangle.Y;
            RowX = Rectangle.X;

            InitBank();
            if (bitmap == null)
                Surface0 = new BitmapSurface(Rectangle);
            else
                Surface0 = new BitmapSurface(bitmap, Rectangle);

            Surface1 = new BitmapSurface(Rectangle);
            Compost = new BitmapSurface(Rectangle);
            _Surface = Surface0;

            InitColor = bitmap == null;






            InitCells();

            //if (bitmap != null)
            //{
            //    Load(bitmap, 0);
            //}
        }


        public void SelectLayer(int layer)
        {
            CellPaint.Layer = layer;
            switch (layer)
            {
                case 0:
                    _Surface = Surface0;
                    break;
                case 1:
                case 2:
                    _Surface = Surface1;
                    break;
            }
        }
        protected virtual void InitCells()
        {
            ActaullyInitCells();
        }

        protected virtual void ActaullyInitCells()
        {
            InitStamp = DateTime.Now;

            int rx = RowRange();
            int cy = ColumnRange();
            Cell c;

            MakeRow();
            while (RowX <= rx)
            {
                MakeColumn();

                c = MakeCell(RowX, RowY, CellW, CellW);

                FirstRowNeighbor(c);

                NewMade(c);

                RowX += CellW;

                PerColumn(c);
            }

            RowY += CellW;

            while (RowY <= cy)
            {
                PreviousRow = Row;

                MakeRow();

                RowX = Rectangle.X;

                Column = (ListHead)Columns.First.Object;
                Last = null;

                PreviousRow.Iterate(MakeSouth);

                RowY += CellW;
            }

            //Last = ConstructCell(Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Width);
            
            //normalize bank loop
            Banks.Last.SetNext(null);

            if (!InitColor)
            {
                Load(Surface.Bitmap, 0);
            }
        }

        protected virtual int RowRange()
        {
            return Rectangle.Width + Rectangle.X - CellW;
        }
        protected virtual int ColumnRange()
        {
            return Rectangle.Height + Rectangle.Y - CellW;
        }
        protected virtual void FirstRowNeighbor(Cell cell)
        {
            if (Last != null)
            {
                ((NeighborZone)Last).NeighborE = cell;
                ((NeighborZone)cell).NeighborW = Last;
            }
            Last = cell;
        }
        public void MakeSouth(ListMember item)
        {
            NewMade(MakeSouth((Cell)item.Object));

            RowX += CellW;
            Column = Column;//.Next?.Cast.ReferenceList;

        }
        public Cell MakeSouth(Cell c)
        {
            var newC = MakeCell(RowX, RowY, CellW, CellW);
            NeighborZone.RowNeighbor(c, newC);
            Last = newC;

            return newC;
        }
        public virtual void NewMade(Cell cell)
        {
            cell.Mod = InitStamp;

            if (InitColor)
            {
                cell.Paint.Background.Paint = new Paint(255, RandomAccess.Next(0, 30), RandomAccess.Next(0, 30), RandomAccess.Next(0, 30));
                //    cell.Paint.Fore.Paint = new Paint(100, RandomAccess.Next(0, 30), RandomAccess.Next(0, 30), RandomAccess.Next(0, 30));
                Update(cell);
            }
            //cell.Paint.Background.Paint = new Paint(Color.Transparent);

            //Update(cell);
        }

        public virtual void PerColumn(Cell cell)
        {
        }
        private Cell MakeCell(int x, int y, int w, int h)
        {
            var c = ConstructCell(x, y, w, h);
            Cells.Add(c);
            Row.Add(c);
            Column.Add(c);

            ((ListHead)BankSwap.Object).Add(c);
            BankSwap = BankSwap.Next;

            return c;
        }
        public virtual void MakeRow()
        {
            Row = new ListHead();
            Rows.Add(Row);


        }
        public virtual void MakeColumn()
        {
            Column = new ListHead();
            Columns.Add(Column);
        }
        public virtual void PreInit()
        {
        }
        protected virtual void InitBank()
        {
            Banks = new ListHead();
            while (Banks.Count < 500)
            {
                Banks.Add(new ListHead());
            }
            BankSwap = Banks.First;

            Banks.Last.SetNext(Banks.First);
            BankSet = new LoopSet(Banks, 50);
        }
        protected virtual Cell ConstructCell(int x, int y, int w, int h)
        {
            var c = new Cell(x, y, w, h, this);
            // UpdateCells.Add(c);
            return c;
        }

        public override void Click(MouseEventArgs point)
        {
            if (point.Button == System.Windows.Forms.MouseButtons.Left)
            {
                Click(new Point(point.X, point.Y));
            }
        }


        public override bool KeepMove(MouseEventArgs point)
        {
            if (!base.KeepMove(point))
                return false;

            long thelastTick = lastTick;
            long thisTick = DateTime.Now.Ticks;

            var ms = (long)((thisTick - thelastTick) / 2000);

            var ts = new TimeSpan(thisTick - thelastTick);

            if (ts.TotalSeconds < 4)
            {
                if (ts.TotalMilliseconds < DrawConfig.ClickBlock.Value)
                    return false;
            }
            lastTick = thisTick;
            return true;
        }

        public override void Move(MouseEventArgs point)
        {
            if (point.Button == System.Windows.Forms.MouseButtons.Left)
            {
                Click(point);
            }
            else
            {

            }
        }
        public virtual void Click(Point point)
        {
            var c = ChildCell(point);
            if (c == null)
                return;


            Click(c);
        }
        public virtual void Click(Cell c)
        {

        }
        public Cell ChildCell(Point point)
        {
            var r = (ListHead)Rows.IterateFind(RowFind, point)?.Object;
            if (r == null)
                return null;

            var c = (Cell)r.IterateFind(CellFind, point)?.Object;
            return c;
        }
        public ListHead RowAt(Point point)
        {
            return (ListHead)Rows.IterateFind(RowFind, point)?.Object;
        }
        public bool RowFind(ListMember item, object passParam)
        {
            var testPoint = (Point)passParam;
            var cell = (Zone)((ListHead)item.Object).First.Object;
            if (cell.Rectangle.Y <= testPoint.Y && cell.Rectangle.Bottom >= testPoint.Y)
                return true;

            return false;
        }
        public bool CellFind(ListMember item, object passParam)
        {
            var testPoint = (Point)passParam;
            var cell = (Cell)item.Object;

            if (cell.Rectangle.X <= testPoint.X && cell.Rectangle.Right >= testPoint.X)
                return true;

            return false;
        }

        public virtual void DrawCell(Cell cell)
        {
            if (cell.PS != null && cell.PP != null)
            {
                //if (cell.Paint.Back.Paint.Color.A < 255)
                //{
                //    Surface.Clear(cell.Rectangle.X - Surface.Rectangle.X, cell.Rectangle.Y - Surface.Rectangle.Y, cell.Rectangle.Width, cell.Rectangle.Height);
                //    //return;
                //}
                Surface.FillRect(cell.PP, cell.Rectangle.X - Surface.Rectangle.X, cell.Rectangle.Y - Surface.Rectangle.Y, cell.Rectangle.Width, cell.Rectangle.Height);
                //                BitmapSurface.LockBitmap.SetPixel(cell.Rectangle.X - Rectangle.X, cell.Rectangle.Y - Rectangle.Y, cell.Paint.Color);
            }
        }
        public virtual void DrawCellNow(Cell cell)
        {
            if (cell.Paint != null)
                DrawScreen.Instance.FillRect(cell.Paint.Back.Paint, cell.Rectangle.X, cell.Rectangle.Y, cell.Rectangle.Width, cell.Rectangle.Height);
        }
        protected virtual void Update(Cell cell)
        {
            DrawCell(cell);
            Updates = true;
            //            UpdateCells.Add(cell);

        }
        public virtual void Refresh()
        {
            Surface.Refresh();
        }

        public virtual void ClearCells()
        {
            Cells.Iterate(ClearCells);
        }
        public virtual void ClearCells(ListMember item)
        {
            ((Cell)item.Object).PS.Paint = null;
        }
        public new virtual bool Draw()
        {
            if (Updates)
            {
                Updates = false;
                Refresh();
                return true;
            }

            //if (UpdateCells.LastCount > 0)
            //{
            //    var updates = UpdateCells;
            //    UpdateCells = new ListHead();
            //    updates.IterateUIBreather(DrawCell);
            //    return true;
            //}
            return false;
            //surface.FillBack(paint);//, Rectangle);
        }
        public virtual void DrawCell(ListMember cell)
        {
            DrawCell((Cell)cell.Object);
        }

        public void LoadScale(Bitmap image, int target, bool alpha = true, bool paintCells = true)
        {
            ryes = true;
            row1 = true;

            if (Rectangle.Width == image.Width * 2 && Rectangle.Height == image.Height * 2)
            {
                LoadX2(image, target, alpha, paintCells);

                Rows.Iterate(RowLoop3);
            }
            //else if (Rectangle.Width == image.Width * 3 && Rectangle.Height == image.Height * 3)
            //{
            //    LoadX3(image, target, alpha, paintCells);
            //    Rows.Iterate(RowLoop3);
            //}
            else
            {
                Status.Log("Scale any todo...");
                return;
            }



            //Surface.Bitmap.Save("C:\\SlabState\\Enhanced\\Frame" + Snaps.ToString("D7") + ".bmp");
            //Snaps++;

            Updates = true;
        }
        //int Snaps;
        public void LoadX2(Bitmap image, int target, bool alpha = true, bool paintCells = true)
        {
            int Depth = System.Drawing.Bitmap.GetPixelFormatSize(image.PixelFormat);

            BitmapData imageData = image.LockBits(new Rectangle(0, 0, image.Width,
      image.Height), ImageLockMode.ReadWrite, image.PixelFormat);

            byte[] imageBytes = new byte[Math.Abs(imageData.Stride) * image.Height];
            IntPtr scan0 = imageData.Scan0;

            Marshal.Copy(scan0, imageBytes, 0, imageBytes.Length);

            image.UnlockBits(imageData);

            int row = 0;
            int column = 0;
            ListMember RowItem = Rows.First;
            // ListMember NextRowItem = null;
            ListHead Row = (ListHead)RowItem.Object;
            ListMember CellItem = Row.First;
            //  ListMember NextCellItem = null;
            Cell cell = (Cell)CellItem.Object;

            int wide = System.Drawing.Bitmap.GetPixelFormatSize(image.PixelFormat);
            wide = wide == 24 ? 3 : 4;
            int pad = imageData.Stride - wide * image.Width;

            for (int i = 0; i < imageBytes.Length - 1 || cell == null; i += wide)
            {
                byte pixelB = imageBytes[i];
                byte pixelG = imageBytes[i + 1];
                byte pixelR = imageBytes[i + 2];
                byte pixelA = 255;
                if (alpha && wide > 3)
                    pixelA = imageBytes[i + 3];

                if (CellItem == null || column > image.Width || column == Rectangle.Width)
                {
                    //should be in pairs
                    RowItem = RowItem.Next.Next;

                    if (CellItem != null)
                    {

                    }
                    if (RowItem == null || ((ListHead)RowItem.Object).Count == 0)
                    {

                        //image.UnlockBits(imageData);
                        return;
                    }
                    row++;
                    Row = (ListHead)RowItem.Object;
                    column = 0;

                    CellItem = Row.First;
                    if (CellItem == null)
                    {

                    }
                    cell = (Cell)CellItem?.Object;

                    i += pad;
                }

                column++;
                //if (row == 479)
                //{

                //}
                //byte pixelB = imageBytes[i];
                //byte pixelR = imageBytes[i + 2];
                //byte pixelG = imageBytes[i + 1];

                //if (pixelR != ChoiceBack.Root.R || pixelG != ChoiceBack.Root.G || pixelB != ChoiceBack.Root.B)
                {
                    if (target == 1)
                    {
                        cell.Paint.Back.Target = new Paint(pixelA, pixelR, pixelG, pixelB);
                    }
                    else if (target == 2)
                    {

                        cell.Paint.Back.Origin = new Paint(pixelA, pixelR, pixelG, pixelB);
                    }
                    else
                    {
                        cell.Paint.Back.Paint = new Paint(pixelA, pixelR, pixelG, pixelB);
                        //DrawCell(cell);
                    }
                    if (paintCells)
                        DrawCell(cell);
                    //BitmapSurface.LockBitmap.SetPixel();
                }
                //else
                //    cell.SetOrigin(pixelR, pixelG, pixelB);


                if (CellItem.Next?.Next == null)
                {
                    CellItem = null;
                    continue;
                }
                CellItem = CellItem.Next?.Next;
                cell = (Cell)CellItem.Object;
            }
            //image.UnlockBits(imageData);
            //Marshal.Copy(imageBytes, 0, scan0, imageBytes.Length);
            //BitmapSurface.Unlock();

        }
        public void LoadX3(Bitmap image, int target, bool alpha = true, bool paintCells = true)
        {
            int Depth = System.Drawing.Bitmap.GetPixelFormatSize(image.PixelFormat);

            BitmapData imageData = image.LockBits(new Rectangle(0, 0, image.Width,
      image.Height), ImageLockMode.ReadWrite, image.PixelFormat);

            byte[] imageBytes = new byte[Math.Abs(imageData.Stride) * image.Height];
            IntPtr scan0 = imageData.Scan0;

            Marshal.Copy(scan0, imageBytes, 0, imageBytes.Length);

            image.UnlockBits(imageData);

            int row = 0;
            int column = 0;
            ListMember RowItem = Rows.First;
            // ListMember NextRowItem = null;
            ListHead Row = (ListHead)RowItem.Object;
            ListMember CellItem = Row.First;
            //  ListMember NextCellItem = null;
            Cell cell = (Cell)CellItem.Object;

            int wide = System.Drawing.Bitmap.GetPixelFormatSize(image.PixelFormat);
            wide = wide == 24 ? 3 : 4;
            int pad = imageData.Stride - wide * image.Width;

            for (int i = 0; i < imageBytes.Length - 1 || cell == null; i += wide)
            {
                byte pixelB = imageBytes[i];
                byte pixelG = imageBytes[i + 1];
                byte pixelR = imageBytes[i + 2];
                byte pixelA = 255;
                if (alpha && wide > 3)
                    pixelA = imageBytes[i + 3];

                if (CellItem == null || column > image.Width || column == Rectangle.Width)
                {
                    //should be in pairs
                    RowItem = RowItem.Next.Next;

                    if (CellItem != null)
                    {

                    }
                    if (RowItem == null || ((ListHead)RowItem.Object).Count == 0)
                    {

                        //image.UnlockBits(imageData);
                        return;
                    }
                    row++;
                    Row = (ListHead)RowItem.Object;
                    column = 0;

                    CellItem = Row.First;
                    if (CellItem == null)
                    {

                    }
                    cell = (Cell)CellItem?.Object;

                    i += pad;
                }

                column++;
                //if (row == 479)
                //{

                //}
                //byte pixelB = imageBytes[i];
                //byte pixelR = imageBytes[i + 2];
                //byte pixelG = imageBytes[i + 1];

                //if (pixelR != ChoiceBack.Root.R || pixelG != ChoiceBack.Root.G || pixelB != ChoiceBack.Root.B)
                {
                    if (target == 1)
                    {
                        cell.Paint.Back.Target = new Paint(pixelA, pixelR, pixelG, pixelB);
                    }
                    else if (target == 2)
                    {

                        cell.Paint.Back.Origin = new Paint(pixelA, pixelR, pixelG, pixelB);
                    }
                    else
                    {
                        cell.Paint.Back.Paint = new Paint(pixelA, pixelR, pixelG, pixelB);
                        //DrawCell(cell);
                    }
                    if (paintCells)
                        DrawCell(cell);
                    //BitmapSurface.LockBitmap.SetPixel();
                }
                //else
                //    cell.SetOrigin(pixelR, pixelG, pixelB);


                if (CellItem.Next?.Next == null)
                {
                    CellItem = null;
                    continue;
                }
                CellItem = CellItem.Next?.Next;
                cell = (Cell)CellItem.Object;
            }
            //image.UnlockBits(imageData);
            //Marshal.Copy(imageBytes, 0, scan0, imageBytes.Length);
            //BitmapSurface.Unlock();

        }
        bool row1;
        bool col1;
        bool ryes;
        bool cyes;
        void RowLoop(ListMember item)
        {
            cyes = true;
            col1 = true;
            ((ListHead)item.Object).Iterate(ColumnLoop);
            row1 = false;
            ryes = !ryes;
        }
        void ColumnLoop(ListMember item)
        {
            var c = (Cell)item.Object;

            if (ryes)
            {
                if (!cyes)
                {
                    if (c.NeighborE == null)
                    {
                        if (c.NeighborW.PP == null)
                        {
                            c.PS.Paint = new Paint(Color.Yellow);

                        }
                        else
                            LakeshoreConfig.Morph(c, c.NeighborW);
                    }
                    else if (c.NeighborE.PP == null)
                    {
                        c.PS.Paint = new Paint(Color.Yellow);

                    }
                    else
                    {
                        c.PS.Paint = LakeshoreConfig.Blend(c.NeighborW.PP.Color, c.NeighborE.PP.Color);
                    }

                    DrawCell(c);
                }

            }
            else
            {
                if (cyes)
                {
                    if (c.NeighborS == null)
                    {
                        LakeshoreConfig.Morph(c, c.NeighborN);
                    }
                    else
                    {
                        c.PS.Paint = LakeshoreConfig.Blend(c.NeighborN.PP.Color, c.NeighborS.PP.Color);
                    }
                }
                else
                {
                    if (c.NeighborSE == null)
                    {
                        LakeshoreConfig.Morph(c, c.NeighborNW);
                    }
                    else
                    {
                        c.PS.Paint = LakeshoreConfig.Blend(c.NeighborNW.PP.Color, c.NeighborSE.PP.Color);
                    }
                }

                //c.PS.Paint = new Paint(Color.Yellow);
                DrawCell(c);
            }

            if (col1)
            {
                col1 = false;
            }
            cyes = !cyes;
        }

        void RowLoop3(ListMember item)
        {
            cyes = true;
            col1 = true;
            ((ListHead)item.Object).Iterate(ColumnLoop3);
            row1 = false;
            ryes = !ryes;
        }


        void ColumnLoop3(ListMember item)
        {
            var c = (Cell)item.Object;

            int wander = RandomAccess.Next(0, 4);

            if (ryes && cyes && wander == 0)
            {
            }
            else
            {
                if (ryes)
                {
                    if (cyes)
                    {
                        if (RandomAccess.Fifty50 || c.NeighborNW == null)
                        {
                            LakeshoreConfig.MorphSome(c, c);
                        }
                        else
                        {
                            LakeshoreConfig.MorphTword(c, c, NeighborZone.NeighborNWSureReach(c));
                        }
                    }
                    else
                    {
                        if (wander == 1)
                        {
                            c.PS.Paint = new Paint(c.NeighborW.PP.Color);
                        }
                        else if (c.NeighborNE == null)
                        {
                            LakeshoreConfig.MorphSome(c, c.NeighborW);
                        }
                        else
                        {
                            LakeshoreConfig.MorphTword(c, c.NeighborW, NeighborZone.NeighborNESureReach(c.NeighborW));
                        }
                    }
                }
                else
                {
                    if (cyes)
                    {
                        if (wander == 2)
                        {
                            c.PS.Paint = new Paint(c.NeighborN.PP.Color);
                        }
                        else if (c.NeighborSW == null)
                        {
                            LakeshoreConfig.MorphSome(c, c.NeighborN);
                        }
                        else
                        {
                            LakeshoreConfig.MorphTword(c, c.NeighborN, NeighborZone.NeighborSWSureReach(c.NeighborN));
                        }
                    }
                    else
                    {
                        if (wander == 3)
                        {
                            c.PS.Paint = new Paint(c.NeighborNW.PP.Color);
                        }
                        else if (c.NeighborSE == null)
                        {
                            LakeshoreConfig.MorphSome(c, c.NeighborNW);
                        }
                        else
                        {
                            LakeshoreConfig.MorphTword(c, c.NeighborNW, NeighborZone.NeighborSESureReach(c.NeighborNW));
                        }
                    }
                }

                DrawCell(c);
            }

            if (col1)
            {
                col1 = false;
            }
            cyes = !cyes;

        }

        public void Load(Bitmap image, int target, bool alpha = true, bool paintCells = true)
        {
            if (image == null)
                return;

            if (Rectangle.Width != image.Width || Rectangle.Height != image.Height)
            {
                LoadScale(image, target, alpha, paintCells);
                return;
            }
            //var image = new Bitmap("C:\\SlabState\\Gut\\House.bmp");

            //BitmapSurface.Lock();


            int Depth = System.Drawing.Bitmap.GetPixelFormatSize(image.PixelFormat);

            BitmapData imageData = image.LockBits(new Rectangle(0, 0, image.Width,
      image.Height), ImageLockMode.ReadWrite, image.PixelFormat);

            byte[] imageBytes = new byte[Math.Abs(imageData.Stride) * image.Height];
            IntPtr scan0 = imageData.Scan0;

            Marshal.Copy(scan0, imageBytes, 0, imageBytes.Length);

            image.UnlockBits(imageData);

            int row = 0;
            int column = 0;
            ListMember RowItem = Rows.First;
            ListHead Row = (ListHead)RowItem.Object;
            ListMember CellItem = Row.First;
            Cell cell = (Cell)CellItem.Object;

            int wide = System.Drawing.Bitmap.GetPixelFormatSize(image.PixelFormat);
            wide = wide == 24 ? 3 : 4;
            int pad = imageData.Stride - wide * image.Width;

            for (int i = 0; i < imageBytes.Length - 1 || cell == null; i += wide)
            {
                byte pixelB = imageBytes[i];
                byte pixelG = imageBytes[i + 1];
                byte pixelR = imageBytes[i + 2];
                byte pixelA = 255;
                if (alpha && wide > 3)
                    pixelA = imageBytes[i + 3];

                row++;

                if (CellItem == null || row == image.Width || row == Rectangle.Width)
                {
                    RowItem = RowItem.Next;

                    if (RowItem == null || ((ListHead)RowItem.Object).Count == 0)
                    {

                        Updates = true;
                        //image.UnlockBits(imageData);
                        return;
                    }
                    row = 0;
                    Row = (ListHead)RowItem.Object;
                    column = 0;
                    CellItem = Row.First;
                    if (CellItem == null)
                    {

                    }
                    cell = (Cell)CellItem?.Object;

                    i += pad;
                }

                //byte pixelB = imageBytes[i];
                //byte pixelR = imageBytes[i + 2];
                //byte pixelG = imageBytes[i + 1];

                //if (pixelR != ChoiceBack.Root.R || pixelG != ChoiceBack.Root.G || pixelB != ChoiceBack.Root.B)
                {
                    if (target == 1)
                    {
                        cell.Paint.Back.Target = new Paint(pixelA, pixelR, pixelG, pixelB);
                    }
                    else if (target == 2)
                    {

                        cell.Paint.Back.Origin = new Paint(pixelA, pixelR, pixelG, pixelB);
                    }
                    else
                    {
                        cell.Paint.Back.Paint = new Paint(pixelA, pixelR, pixelG, pixelB);
                        //DrawCell(cell);
                    }
                    if (paintCells)
                        DrawCell(cell);
                    //BitmapSurface.LockBitmap.SetPixel();
                }
                //else
                //    cell.SetOrigin(pixelR, pixelG, pixelB);

                CellItem = CellItem.Next;
                if (CellItem == null)
                {

                    continue;
                }
                cell = (Cell)CellItem.Object;
            }
            //image.UnlockBits(imageData);
            //Marshal.Copy(imageBytes, 0, scan0, imageBytes.Length);
            //BitmapSurface.Unlock();

            Updates = true;
        }
    }
}
